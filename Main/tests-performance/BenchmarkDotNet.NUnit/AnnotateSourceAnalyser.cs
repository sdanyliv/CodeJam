using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

using BenchmarkDotNet.Analyzers;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Helpers;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;

using CodeJam;

using JetBrains.Annotations;

// ReSharper disable CheckNamespace

namespace BenchmarkDotNet.NUnit
{
	/// <summary>
	/// Fills min..max values for [CompetitionBenchmark] attribute
	/// DANGER: this will try to update sources. May fail. 
	/// </summary>
	[PublicAPI]
	[SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
	public partial class AnnotateSourceAnalyser : IAnalyser
	{
		#region Helper types
		private class AnnotateContext
		{
			private readonly Dictionary<string, string[]> _sourceLines = new Dictionary<string, string[]>();
			private readonly Dictionary<string, XDocument> _xmlAnnotations = new Dictionary<string, XDocument>();
			private readonly HashSet<string> _changedFiles = new HashSet<string>();

			// ReSharper disable once MemberCanBePrivate.Local
			public bool HasChanges => _changedFiles.Any();

			public string[] GetFileLines(string file)
			{
				if (_xmlAnnotations.ContainsKey(file))
					throw new InvalidOperationException($"File {file} already loaded as xml annotation");

				string[] result;
				if (!_sourceLines.TryGetValue(file, out result))
				{
					result = File.ReadAllLines(file);
					_sourceLines[file] = result;
				}
				return result;
			}

			public XDocument GetXmlAnnotation(string file)
			{
				if (_sourceLines.ContainsKey(file))
					throw new InvalidOperationException($"File {file} already loaded as source lines");

				XDocument result;
				if (!_xmlAnnotations.TryGetValue(file, out result))
				{
					result = XDocument.Load(file);
					_xmlAnnotations[file] = result;
				}
				return result;
			}

			public void MarkAsChanged(string file)
			{
				if (!_sourceLines.ContainsKey(file) && !_xmlAnnotations.ContainsKey(file))
					throw new InvalidOperationException($"File {file} not loaded yet");

				_changedFiles.Add(file);
			}

			public void ReplaceLine(string file, int lineIndex, string newLine)
			{
				GetFileLines(file)[lineIndex] = newLine;
				MarkAsChanged(file);
			}

			public void Save()
			{
				if (!HasChanges)
					return;

				foreach (var pair in _sourceLines)
				{
					if (_changedFiles.Contains(pair.Key))
						BenchmarkHelpers.WriteFileContent(pair.Key, pair.Value);
				}

				var saveSettings = new XmlWriterSettings
				{
					Indent = true,
					IndentChars = "\t"
				};
				foreach (var pair in _xmlAnnotations)
				{
					if (_changedFiles.Contains(pair.Key))
					{
						using (var writer = XmlWriter.Create(pair.Key, saveSettings))
							pair.Value.Save(writer);
					}
				}
			}
		}
		#endregion

		#region Helper methods
		private static FinalRunAnalyser.TargetMinMax[] GetNewCompetitionBoundaries(
			Summary summary,
			IDictionary<MethodInfo, FinalRunAnalyser.TargetMinMax> minMaxData)
		{
			var fixedMinPairs = new HashSet<FinalRunAnalyser.TargetMinMax>();
			var fixedMaxPairs = new HashSet<FinalRunAnalyser.TargetMinMax>();
			var newValues = new Dictionary<MethodInfo, FinalRunAnalyser.TargetMinMax>();

			foreach (var benchGroup in summary.SameConditionBenchmarks())
			{
				var baselineBenchmark = benchGroup.Single(b => b.Target.Baseline);
				var baselineMetricMin = summary.GetPercentile(baselineBenchmark, 0.5);
				var baselineMetricMax = summary.GetPercentile(baselineBenchmark, 0.95);

				foreach (var benchmark in benchGroup)
				{
					FinalRunAnalyser.TargetMinMax targetMinMax;
					if (!minMaxData.TryGetValue(benchmark.Target.Method, out targetMinMax))
						continue;

					if (benchmark.Target.Baseline)
						continue;

					var minRatio = summary.GetPercentile(benchmark, 0.85) / baselineMetricMin;
					var maxRatio = summary.GetPercentile(benchmark, 0.95) / baselineMetricMax;
					if (minRatio > maxRatio)
						Algorithms.Swap(ref minRatio, ref maxRatio);

					FinalRunAnalyser.TargetMinMax newMinMax;
					if (!newValues.TryGetValue(targetMinMax.TargetMethod, out newMinMax))
					{
						newMinMax = targetMinMax.Clone();
					}

					if (newMinMax.UnionWithMin(minRatio))
					{
						fixedMinPairs.Add(newMinMax);
						newValues[newMinMax.TargetMethod] = newMinMax;
					}
					if (newMinMax.UnionWithMax(maxRatio))
					{
						fixedMaxPairs.Add(newMinMax);
						newValues[newMinMax.TargetMethod] = newMinMax;
					}
				}
			}

			foreach (var targetMinMax in fixedMinPairs)
			{
				// min = 0.97x;
				targetMinMax.UnionWithMin(
					Math.Floor(targetMinMax.Min * 97) / 100);
			}
			foreach (var targetMinMax in fixedMaxPairs)
			{
				// max = 1.03x;
				targetMinMax.UnionWithMax(
					Math.Ceiling(targetMinMax.Max * 103) / 100);
			}

			return newValues.Values.ToArray();
		}

		private static bool TryGetSourceInfo(MethodInfo method, out string fileName, out int firstCodeLine)
		{
			fileName = null;
			firstCodeLine = 0;
			var methodSymbols = SymbolHelpers.TryGetSymbols(method);
			if (methodSymbols != null)
			{
				var count = methodSymbols.SequencePointCount;
				var docs = new ISymbolDocument[count];
				var offsets = new int[count];
				var lines = new int[count];
				var columns = new int[count];
				var endlines = new int[count];
				var endcolumns = new int[count];
				methodSymbols.GetSequencePoints(offsets, docs, lines, columns, endlines, endcolumns);

				fileName = docs[0].URL;
				firstCodeLine = lines.Min();
			}
			return fileName != null;
		}
		#endregion

		#region Public api
		public bool RerunIfModified { get; set; }

		public IEnumerable<IWarning> Analyze(Summary summary)
		{
			var sharedState = summary.Config.GetAnalysers().OfType<FinalRunAnalyser>().Single();

			var warnings = new List<IWarning>();
			var logger = summary.Config.GetCompositeLogger();
			AnnotateBenchmarkFiles(summary, sharedState, logger, warnings);

			return warnings;
		}
		#endregion

		private void AnnotateBenchmarkFiles(
			Summary summary, FinalRunAnalyser sharedState,
			ILogger logger, List<IWarning> warnings)
		{
			var annContext = new AnnotateContext();
			var minMaxData = sharedState.GetCompetitionBoundaries(summary.Benchmarks);
			var updatedRecords = GetNewCompetitionBoundaries(summary, minMaxData);
			if (updatedRecords.Length == 0)
			{
				logger.WriteLineInfo("All competition benchmarks are in boundary.");
				return;
			}

			foreach (var targetMinMax in updatedRecords)
			{
				var targetMethod = targetMinMax.TargetMethod;

				logger.WriteLineInfo($"Method {targetMethod.Name}: new boundary [{targetMinMax.Min},{targetMinMax.Max}].");

				int firstCodeLine;
				string fileName;
				bool hasSource = TryGetSourceInfo(targetMethod, out fileName, out firstCodeLine);
				if (!hasSource)
				{
					throw new InvalidOperationException($"Method {targetMethod.Name}: could not annotate. Source file not found.");
				}

				if (targetMinMax.ResourceName == null)
				{
					logger.WriteLineInfo($"Method {targetMethod.Name}: annotate at line {firstCodeLine}, file {fileName}.");
					bool annotated = TryFixBenchmarkAttribute(annContext, fileName, firstCodeLine, targetMinMax);
					if (!annotated)
					{
						throw new InvalidOperationException($"Method {targetMethod.Name}: could not annotate. Source file ${fileName}.");
					}
				}
				else
				{
					var xmlFileName = Path.ChangeExtension(fileName, ".xml");
					logger.WriteLineInfo($"Method {targetMethod.Name}: annotate resource {targetMinMax.ResourceName} (file {xmlFileName}).");
					bool annotated = TryFixBenchmarkResource(annContext, xmlFileName, targetMinMax);
					if (!annotated)
					{
						throw new InvalidOperationException($"Method {targetMethod.Name}: could not annotate. Resource file ${xmlFileName}.");
					}
				}

				if (RerunIfModified && !sharedState.LastRun)
				{
					var message = $"Method {targetMethod.Name} annotation updated, benchmark has to be restarted";
					logger.WriteLineInfo(message);
					warnings.Add(new Warning(nameof(AnnotateSourceAnalyser), message, null));

					minMaxData[targetMinMax.TargetMethod] = targetMinMax;
					sharedState.RerunRequested = true;
				}
				else
				{
					var message = $"Method {targetMethod.Name} annotation updated.";
					logger.WriteLineInfo(message);
					warnings.Add(new Warning(nameof(AnnotateSourceAnalyser), message, null));
				}
			}

			annContext.Save();
		}
	}
}