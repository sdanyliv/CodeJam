using System;
using System.Linq;
using System.Runtime.CompilerServices;

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Helpers;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

using NUnit.Framework;

// ReSharper disable CheckNamespace
// ReSharper disable ConvertMethodToExpressionBody
// ReSharper disable ArrangeBraces_ifelse

namespace BenchmarkDotNet.NUnit
{
	public static class CompetitionBenchmarkRunner
	{
		#region Public API overloads
		// Benchmarks do not compile, disabled for now
#if DISABLED_FEATURES
	/// <summary>
	/// Runs the competition benchmark from source
	/// </summary>
		public static void RunFromSource(double maxRatio, [CallerFilePath]string callerFile = null)
		{
			RunCompetition(0, maxRatio, null, File.ReadAllText(callerFile), null);
		}
		/// <summary>
		/// Runs the competition benchmark from source
		/// </summary>
		public static void RunFromSource(double minRatio, double maxRatio, [CallerFilePath]string callerFile = null)
		{
			RunCompetition(minRatio, maxRatio, null, File.ReadAllText(callerFile), null);
		}
		/// <summary>
		/// Runs the competition benchmark from source
		/// </summary>
		public static void RunFromSource(double minRatio, double maxRatio, IConfig config, [CallerFilePath]string callerFile = null)
		{
			RunCompetition(minRatio, maxRatio, null, File.ReadAllText(callerFile), config);
		}
#endif

		/// <summary>
		/// Runs the competition benchmark from a type of a callee
		/// </summary>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Run<T>(T thisReference, double maxRatio) where T: class 
		{
			RunCompetition(0, maxRatio, thisReference.GetType(), null, null);
		}

		/// <summary>
		/// Runs the competition benchmark from a type of a callee
		/// </summary>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Run<T>(T thisReference, double minRatio, double maxRatio) where T : class
		{
			RunCompetition(minRatio, maxRatio, thisReference.GetType(), null, null);
		}

		/// <summary>
		/// Runs the competition benchmark from a type of a callee
		/// </summary>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Run<T>(T thisReference, double minRatio, double maxRatio, IConfig config) where T : class
		{
			RunCompetition(minRatio, maxRatio, thisReference.GetType(), null, config);
		}

		/// <summary>
		/// Runs the competition benchmark
		/// </summary>
		public static void Run<T>(double maxRatio) where T : class
		{
			RunCompetition(0, maxRatio, typeof(T), null, null);
		}

		/// <summary>
		/// Runs the competition benchmark 
		/// </summary>
		public static void Run<T>(double minRatio, double maxRatio) where T : class
		{
			RunCompetition(minRatio, maxRatio, typeof(T), null, null);
		}

		/// <summary>
		/// Runs the competition benchmark
		/// </summary>
		public static void Run<T>(double minRatio, double maxRatio, IConfig config) where T : class
		{
			RunCompetition(minRatio, maxRatio, typeof(T), null, config);
		}
		#endregion

		/// <summary>
		/// Runs the competition benchmark
		/// </summary>
		// BASEDON: https://github.com/PerfDotNet/BenchmarkDotNet/blob/master/BenchmarkDotNet.IntegrationTests/PerformanceUnitTest.cs
		public static void RunCompetition(
			double minRatio, double maxRatio, Type benchType, string benchSource, IConfig config)
		{
			var currentDirectory = Environment.CurrentDirectory;
			try
			{
				// WORKAROUND: fixing the https://github.com/nunit/nunit3-vs-adapter/issues/96
				Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;

				RunCompetitionUnderSetup(minRatio, maxRatio, benchType, benchSource, config);
			}
			finally
			{
				Environment.CurrentDirectory = currentDirectory;
			}
		}

		private static void RunCompetitionUnderSetup(
			double minRatio, double maxRatio, Type benchType, string benchSource, IConfig config)
		{
			// Based on 95th percentile
			const double percentileRatio = 0.95;

			var summary = RunComparisonCore(benchType, benchSource, config);

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
					var benchmarkMinRatio = minRatio;
					var benchmarkMaxRatio = maxRatio;

					var benchOptions = benchmark.Target.Method.TryGetAttribute<CompetitionBenchmarkAttribute>();
					if (benchOptions != null)
					{
						if (benchOptions.DoesNotCompete)
							continue;

						// ReSharper disable once CompareOfFloatsByEqualityOperator
						if (benchOptions.MinRatio != 0)
						{
							benchmarkMinRatio = benchOptions.MinRatio;
						}
						// ReSharper disable once CompareOfFloatsByEqualityOperator
						if (benchOptions.MaxRatio != 0)
						{
							benchmarkMaxRatio = benchOptions.MaxRatio;
						}
					}

					Assert.That(
						ratio >= benchmarkMinRatio,
						$"Bench {benchmark.ShortInfo} runs faster than {benchmarkMinRatio}x baseline. Actual ratio: {ratio}x");
					Assert.That(
						ratio <= benchmarkMaxRatio,
						$"Bench {benchmark.ShortInfo} runs slower than {benchmarkMaxRatio}x baseline. Actual ratio: {ratio}x");
				}
			}
		}

		private static Summary RunComparisonCore(Type benchType, string benchSource, IConfig config)
		{
			// Capturing the output
			var logger = new AccumulationLogger();
			logger.WriteLine();
			logger.WriteLine();
			logger.WriteLine(new string('=', 40));
			logger.WriteLine();

			// TODO: better setup?
			config = BenchmarkHelpers.CreateUnitTestConfig(config ?? DefaultConfig.Instance)
				.With(logger)
				.With(
					StatisticColumn.Min,
					ScaledPercentileColumn.S0Column,
					ScaledPercentileColumn.S50Column,
					ScaledPercentileColumn.S85Column,
					ScaledPercentileColumn.S95Column,
					ScaledPercentileColumn.S100Column,
					StatisticColumn.Max);

			// Running the benchmark
			var summary = benchSource == null
				? BenchmarkRunner.Run(benchType, config)
				: BenchmarkRunner.RunSource(benchSource, config);

			// Dumping the benchmark results to console
			MarkdownExporter.Default.ExportToLog(summary, ConsoleLogger.Default);
			// Dumping all captured output below the benchmark results
			ConsoleLogger.Default.WriteLine(logger.GetLog());

			return summary;
		}
	}
}