using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

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

			// ReSharper disable once MemberCanBePrivate.Local
			public bool HasChanges { get; private set; }

			public string[] GetFileLines(string file)
			{
				string[] result;
				if (!_sourceLines.TryGetValue(file, out result))
				{
					result = File.ReadAllLines(file);
					_sourceLines[file] = result;
				}
				return result;
			}

			public void ReplaceLine(string file, int lineIndex, string newLine)
			{
				GetFileLines(file)[lineIndex] = newLine;
				HasChanges = true;
			}

			public void Save()
			{
				if (!HasChanges)
					return;

				foreach (var pair in _sourceLines)
				{
					BenchmarkHelpers.WriteFileContent(pair.Key, pair.Value);
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
						newMinMax = new FinalRunAnalyser.TargetMinMax(
							targetMinMax.TargetMethod,
							targetMinMax.Min,
							targetMinMax.Max,
							targetMinMax.DoesNotCompete);
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

		#region Fix benchmark annotation
		private static readonly Regex _breakIfRegex = new Regex(
			@"///|\sclass\s|\}|\;",
			RegexOptions.CultureInvariant | RegexOptions.Compiled);

		private static readonly Regex _attributeRegex = new Regex(
			@"
				(\[CompetitionBenchmark\(?)
				(
					\d+\.?\d*
					\,\s*
					\d+\.?\d*
					\s*
				)?
				(.*?\])",
			RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant | RegexOptions.Compiled);

		private static bool TryFixBenchmarkAttribute(
			AnnotateContext annotateContext,
			string fileName, int firstCodeLine,
			FinalRunAnalyser.TargetMinMax targetMinMax)
		{
			var result = false;
			var sourceFileLines = annotateContext.GetFileLines(fileName);

			for (int i = firstCodeLine - 2; i >= 0; i--)
			{
				var line = sourceFileLines[i];
				if (_breakIfRegex.IsMatch(line))
					break;

				var line2 = _attributeRegex.Replace(
					line,
					m => FixAttributeContent(m, targetMinMax), 1);
				if (line2 != line)
				{
					annotateContext.ReplaceLine(fileName, i, line2);
					result = true;
					break;
				}
			}
			return result;
		}

		private static string FixAttributeContent(Match m, FinalRunAnalyser.TargetMinMax targetMinMax)
		{
			var culture = CultureInfo.InvariantCulture;
			var attributeStartText = m.Groups[1].Value;
			var attributeEndText = m.Groups[3].Value;

			bool attributeWithoutBraces = !attributeStartText.EndsWith("(");
			bool attributeWithoutMinMax = !m.Groups[2].Success;
			bool attributeHasAdditionalContent = !attributeEndText.StartsWith(")");

			var result = new StringBuilder(m.Length + 10);
			result.Append(attributeStartText);

			if (attributeWithoutBraces)
			{
				result.Append('(');
				result.AppendFormat(
					culture, "{0:0.00###}, {1:0.00###}",
					targetMinMax.Min,
					targetMinMax.Max);
				result.Append(')');
			}
			else
			{
				result.AppendFormat(
					culture, "{0:0.00###}, {1:0.00###}",
					targetMinMax.Min,
					targetMinMax.Max);
				if (attributeWithoutMinMax && attributeHasAdditionalContent)
				{
					result.Append(", ");
				}
			}

			result.Append(attributeEndText);
			return result.ToString();
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

				logger.WriteLineInfo($"Method {targetMethod.Name}: annotate at line {firstCodeLine}, file {fileName}.");
				bool annotated = TryFixBenchmarkAttribute(annContext, fileName, firstCodeLine, targetMinMax);
				if (!annotated)
				{
					throw new InvalidOperationException($"Method {targetMethod.Name}: could not annotate. Source file ${fileName}.");
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