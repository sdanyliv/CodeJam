using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using BenchmarkDotNet.Analyzers;
using BenchmarkDotNet.Helpers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

using CodeJam;

// ReSharper disable CheckNamespace

namespace BenchmarkDotNet.NUnit
{
	using CompetitionData = IDictionary<MethodInfo, FinalRunAnalyser.TargetMinMax>;

	/// <summary>
	/// Internal class to manage consequent runs.
	/// DO NOT add this one explicitly
	/// </summary>
	[SuppressMessage("ReSharper", "ArrangeBraces_while")]
	internal class FinalRunAnalyser : IAnalyser
	{
		#region Xml metadata constants
		public const string CompetitionBenchmarksRootNode = "CompetitionBenchmarks";
		public const string CompetitionNode = "Competition";
		public const string CandidateNode = "Candidate";
		public const string TargetAttribute = "Target";
		public const string MinRatioAttribute = "MinRatio";
		public const string MaxRatioAttribute = "MaxRatio";
		#endregion

		public class TargetMinMax
		{
			private static readonly CultureInfo _culture = CultureInfo.InvariantCulture;
			public TargetMinMax() { }

			public TargetMinMax(
				MethodInfo targetMethod, double min, double max, string resourceName)
			{
				TargetMethod = targetMethod;
				ResourceName = resourceName;
				Min = min;
				Max = max;
			}

			public MethodInfo TargetMethod { get; }
			public string ResourceName { get; }

			// ReSharper disable once PossibleNullReferenceException
			public string CompetitionName => TargetMethod.DeclaringType.Name;
			public string CandidateName => TargetMethod.Name;

			public double Min { get; private set; }
			public double Max { get; private set; }

			public string MinText => Min.ToString("0.00###", _culture);
			public string MaxText => Max.ToString("0.00###", _culture);

			public TargetMinMax Clone() => new TargetMinMax(TargetMethod, Min, Max, ResourceName);

			// ReSharper disable once CompareOfFloatsByEqualityOperator
			public bool MinIsEmpty => Min == 0;
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			public bool MaxIsEmpty => Max == 0;

			public bool IsEmpty => MinIsEmpty && MaxIsEmpty;

			public bool UnionWithMin(double min)
			{
				var expanded = false;

				// ReSharper disable once CompareOfFloatsByEqualityOperator
				if (min != 0 && !double.IsInfinity(min) && (MinIsEmpty || Min > min))
				{
					expanded = true;
					Min = min;
				}

				return expanded;
			}

			public bool UnionWithMax(double max)
			{
				var expanded = false;

				// ReSharper disable once CompareOfFloatsByEqualityOperator
				if (max != 0 && !double.IsInfinity(max) && (MaxIsEmpty || Max < max))
				{
					expanded = true;
					Max = max;
				}

				return expanded;
			}
		}

		#region CompetitionData helpers
		// ReSharper disable once ParameterTypeCanBeEnumerable.Local
		private static void InitCompetitionBoundaries(CompetitionData competitionData, Benchmark[] benchmarks)
		{
			foreach (var targetMethod in benchmarks.Select(d => d.Target.Method).Distinct())
			{
				var competitionAttribute = (CompetitionBenchmarkAttribute)
					targetMethod.GetCustomAttribute(typeof(CompetitionBenchmarkAttribute));

				if (competitionAttribute != null &&
					!competitionAttribute.Baseline &&
					!competitionAttribute.DoesNotCompete)
				{
					var targetData = GetTargetMinMax(targetMethod, competitionAttribute);

					competitionData.Add(targetMethod, targetData);
				}
			}
		}

		private static TargetMinMax GetTargetMinMax(
			MethodInfo targetMethod, CompetitionBenchmarkAttribute competitionAttribute)
		{
			string resourceName = null;
			var annotatedType = targetMethod.DeclaringType;
			while (annotatedType != null && resourceName == null)
			{
				resourceName = annotatedType
					.GetCustomAttribute<CompetitionMetadataAttribute>()
					?.MetadataResourceName;

				annotatedType = annotatedType.DeclaringType;
			}

			if (resourceName != null)
				return GetMinMaxFromResource(targetMethod, resourceName);

			return new TargetMinMax(
				targetMethod,
				competitionAttribute.MinRatio,
				competitionAttribute.MaxRatio,
				null);
		}

		private static TargetMinMax GetMinMaxFromResource(MethodInfo targetMethod, string resourceName)
		{
			// ReSharper disable once PossibleNullReferenceException
			var resStream = targetMethod.DeclaringType.Assembly.GetManifestResourceStream(resourceName);
			if (resStream == null)
				throw new InvalidOperationException(
					$"Method {targetMethod.Name}: resource stream {resourceName} not found");

			var resDocument = XDocument.Load(resStream);
			// ReSharper disable once PossibleNullReferenceException
			var competitionName = targetMethod.DeclaringType.Name;
			var candidateName = targetMethod.Name;

			var rootNode = resDocument.Element(CompetitionBenchmarksRootNode);
			if (rootNode == null)
				throw new InvalidOperationException(
					$"Resource {resourceName}: root node {CompetitionBenchmarksRootNode} not found.");

			var resNodes =
				from competition in rootNode.Elements(CompetitionNode)
				where competition.Attribute(TargetAttribute)?.Value == competitionName
				from candidate in competition.Elements(CandidateNode)
				where candidate.Attribute(TargetAttribute)?.Value == candidateName
				select candidate;

			var resNode = resNodes.SingleOrDefault();
			var minText = resNode?.Attribute(MinRatioAttribute)?.Value;
			var maxText = resNode?.Attribute(MaxRatioAttribute)?.Value;

			double min;
			double max;
			var culture = CultureInfo.InvariantCulture;
			double.TryParse(minText, NumberStyles.Any, culture, out min);
			double.TryParse(maxText, NumberStyles.Any, culture, out max);

			return new TargetMinMax(targetMethod, min, max, resourceName);
		}

		private static TargetMinMax[] GetNewCompetitionBoundariesCore(Summary summary, CompetitionData competitionData)
		{
			var fixedMinPairs = new HashSet<TargetMinMax>();
			var fixedMaxPairs = new HashSet<TargetMinMax>();
			var newValues = new Dictionary<MethodInfo, TargetMinMax>();

			foreach (var benchGroup in summary.SameConditionBenchmarks())
			{
				var baselineBenchmark = benchGroup.Single(b => b.Target.Baseline);
				var baselineMetricMin = summary.GetPercentile(baselineBenchmark, 0.85);
				var baselineMetricMax = summary.GetPercentile(baselineBenchmark, 0.95);

				foreach (var benchmark in benchGroup)
				{
					TargetMinMax targetMinMax;
					if (!competitionData.TryGetValue(benchmark.Target.Method, out targetMinMax))
						continue;

					if (benchmark.Target.Baseline)
						continue;

					var minRatio = summary.GetPercentile(benchmark, 0.85) / baselineMetricMin;
					var maxRatio = summary.GetPercentile(benchmark, 0.95) / baselineMetricMax;
					if (minRatio > maxRatio)
						Algorithms.Swap(ref minRatio, ref maxRatio);

					TargetMinMax newMinMax;
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

		private static void ProcessSummaryCore(
			Summary summary, double defaultMinRatio, double defaultMaxRatio,
			CompetitionData competitionData)
		{
			// Based on 95th percentile
			const double percentileRatio = 0.95;

			var benchmarkGroups = summary.SameConditionBenchmarks();
			foreach (var benchmarkGroup in benchmarkGroups)
			{
				var baselineBenchmarks = benchmarkGroup.Where(b => b.Target.Baseline).ToArray();
				if (baselineBenchmarks.Length == 0)
					throw new InvalidOperationException("Define Baseline benchmark");
				if (baselineBenchmarks.Length != 1)
					throw new InvalidOperationException("There should be only one Baseline benchmark");

				var baselineBenchmark = baselineBenchmarks.Single();
				var baselineMetric = summary.GetPercentile(baselineBenchmark, percentileRatio);
				// ReSharper disable once CompareOfFloatsByEqualityOperator
				if (baselineMetric == 0)
					throw new InvalidOperationException($"Baseline benchmark {baselineBenchmark.ShortInfo} does not compute");

				foreach (var benchmark in benchmarkGroup)
				{
					if (benchmark == baselineBenchmark)
						continue;

					TargetMinMax targetMinMax;
					if (!competitionData.TryGetValue(benchmark.Target.Method, out targetMinMax))
						continue;

					var benchmarkMinRatio = defaultMinRatio;
					if (!targetMinMax.MinIsEmpty)
					{
						benchmarkMinRatio = targetMinMax.Min;
					}
					var benchmarkMaxRatio = defaultMaxRatio;
					if (!targetMinMax.MaxIsEmpty)
					{
						benchmarkMaxRatio = targetMinMax.Max;
					}

					var reportMetric = summary.GetPercentile(benchmark, percentileRatio);
					var actualRatio = Math.Round(reportMetric / baselineMetric, 2);
					if (actualRatio < benchmarkMinRatio)
						throw new InvalidOperationException(
							$"Method {benchmark.Target.Method.Name} runs faster than {benchmarkMinRatio}x baseline. Actual ratio: {actualRatio}x");

					// ReSharper disable once CompareOfFloatsByEqualityOperator
					if (benchmarkMaxRatio == 0)
						throw new InvalidOperationException(
							$"Method {benchmark.Target.Method.Name}: max ratio not set. Actual ratio: {actualRatio}x");

					if (actualRatio > benchmarkMaxRatio)
						throw new InvalidOperationException(
							$"Method {benchmark.Target.Method.Name} runs slower than {benchmarkMaxRatio}x baseline. Actual ratio: {actualRatio}x");
				}
			}
		}
		#endregion

		private readonly CompetitionData _competitionData = new Dictionary<MethodInfo, TargetMinMax>();

		#region Public api
		public bool LastRun { get; set; }
		public bool RerunRequested { get; set; }
		public IEnumerable<IWarning> Analyze(Summary summary) => Enumerable.Empty<IWarning>();

		public CompetitionData GetCompetitionBoundaries(Benchmark[] benchmarks)
		{
			if (_competitionData.Count == 0)
			{
				InitCompetitionBoundaries(_competitionData, benchmarks);
			}

			return _competitionData;
		}

		public TargetMinMax[] GetNewCompetitionBoundaries(Summary summary, CompetitionData minMaxData) =>
			GetNewCompetitionBoundariesCore(summary, minMaxData);

		public void ProcessSummary(Summary summary, double defaultMinRatio, double defaultMaxRatio)
		{
			var benchmarkMinMax = GetCompetitionBoundaries(summary.Benchmarks);
			ProcessSummaryCore(summary, defaultMinRatio, defaultMaxRatio, benchmarkMinMax);
		}
		#endregion
	}
}