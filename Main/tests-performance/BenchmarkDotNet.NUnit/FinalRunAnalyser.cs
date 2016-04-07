using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using BenchmarkDotNet.Analyzers;
using BenchmarkDotNet.Helpers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

// ReSharper disable CheckNamespace

namespace BenchmarkDotNet.NUnit
{
	/// <summary>
	/// Internal class to manage consequent runs.
	/// DO NOT add this one explicitly
	/// </summary>
	internal class FinalRunAnalyser : IAnalyser
	{
		public class TargetMinMax
		{
			public TargetMinMax() { }

			public TargetMinMax(MethodInfo targetMethod, double min, double max, bool doesNotCompete)
			{
				TargetMethod = targetMethod;
				DoesNotCompete = doesNotCompete;
				Min = min;
				Max = max;
			}

			public MethodInfo TargetMethod { get; }
			public bool DoesNotCompete { get; }
			public double Min { get; private set; }
			public double Max { get; private set; }

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

		private readonly Dictionary<MethodInfo, TargetMinMax> _competitionData = new Dictionary<MethodInfo, TargetMinMax>();

		#region Public api
		public bool LastRun { get; set; }
		public bool RerunRequested { get; set; }
		public IEnumerable<IWarning> Analyze(Summary summary) => Enumerable.Empty<IWarning>();

		public IDictionary<MethodInfo, TargetMinMax> GetCompetitionBoundaries(Benchmark[] benchmarks)
		{
			if (_competitionData.Count == 0)
			{
				foreach (var targetMethod in benchmarks.Select(d => d.Target.Method).Distinct())
				{
					var competitionAttribute = (CompetitionBenchmarkAttribute)
						targetMethod.GetCustomAttribute(typeof(CompetitionBenchmarkAttribute));

					if (competitionAttribute != null && !competitionAttribute.Baseline)
					{
						var targetData = new TargetMinMax(
							targetMethod,
							competitionAttribute.MinRatio,
							competitionAttribute.MaxRatio,
							competitionAttribute.DoesNotCompete);

						_competitionData.Add(targetMethod, targetData);
					}
				}
			}

			return _competitionData;
		}

		public void ProcessSummary(
			Summary summary, double defaultMinRatio, double defaultMaxRatio)
		{
			// Based on 95th percentile
			const double percentileRatio = 0.95;

			var benchmarkMinMax = GetCompetitionBoundaries(summary.Benchmarks);
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

					var reportMetric = summary.GetPercentile(benchmark, percentileRatio);
					var ratio = Math.Round(reportMetric / baselineMetric, 2);
					var benchmarkMinRatio = defaultMinRatio;
					var benchmarkMaxRatio = defaultMaxRatio;

					TargetMinMax targetMinMax;
					if (benchmarkMinMax.TryGetValue(benchmark.Target.Method, out targetMinMax))
					{
						if (targetMinMax.DoesNotCompete)
							continue;

						// ReSharper disable once CompareOfFloatsByEqualityOperator
						if (!targetMinMax.MinIsEmpty)
						{
							benchmarkMinRatio = targetMinMax.Min;
						}
						// ReSharper disable once CompareOfFloatsByEqualityOperator
						if (!targetMinMax.MaxIsEmpty)
						{
							benchmarkMaxRatio = targetMinMax.Max;
						}
					}

					if (ratio < benchmarkMinRatio)
						throw new InvalidOperationException(
							$"Method {benchmark.Target.Method.Name} runs faster than {benchmarkMinRatio}x baseline. Actual ratio: {ratio}x");

					// ReSharper disable once CompareOfFloatsByEqualityOperator
					if (benchmarkMaxRatio == 0)
						throw new InvalidOperationException(
							$"Method {benchmark.Target.Method.Name}: max ratio not set. Actual ratio: {ratio}x");

					if (ratio > benchmarkMaxRatio)
						throw new InvalidOperationException(
							$"Method {benchmark.Target.Method.Name} runs slower than {benchmarkMaxRatio}x baseline. Actual ratio: {ratio}x");
				}
			}
		}
		#endregion
	}
}