using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Helpers;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

using JetBrains.Annotations;

using NUnit.Framework;

// ReSharper disable CheckNamespace

namespace BenchmarkDotNet.NUnit
{
	[PublicAPI]
	public static class CompetitionBenchmarkRunner
	{
		#region Public API
		/// <summary>
		/// Runs the competition benchmark from a type of a callee
		/// </summary>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Run<T>(T thisReference) where T : class =>
			RunCompetition(thisReference.GetType(), null, 0, 0);

		/// <summary>
		/// Runs the competition benchmark from a type of a callee
		/// </summary>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Run<T>(T thisReference, IConfig config) where T : class =>
			RunCompetition(thisReference.GetType(), config, 0, 0);

		/// <summary>
		/// Runs the competition benchmark
		/// </summary>
		public static void Run<T>(IConfig config) where T : class => 
			RunCompetition(typeof(T), config, 0, 0);
		#endregion

		#region Core logic
		/// <summary>
		/// Runs the competition benchmark
		/// </summary>
		// BASEDON: https://github.com/PerfDotNet/BenchmarkDotNet/blob/master/BenchmarkDotNet.IntegrationTests/PerformanceUnitTest.cs
		public static void RunCompetition(
			Type benchmarkType, IConfig config, double minRatio, double maxRatio)
		{
			var currentDirectory = Environment.CurrentDirectory;
			try
			{
				// WORKAROUND: fixing the https://github.com/nunit/nunit3-vs-adapter/issues/96
				Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;

				RunCompetitionUnderSetup(benchmarkType, config, minRatio, maxRatio);
			}
			finally
			{
				Environment.CurrentDirectory = currentDirectory;
			}
		}

		private static void RunCompetitionUnderSetup(
			Type benchmarkType, IConfig config, double minRatio, double maxRatio)
		{
			ValidateCompetitionSetup(benchmarkType);

			// Capturing the output
			var logger = InitAccumulationLogger();
			// Competition analyzer
			var competitionAnalyser = new CompetitionAnalyser();
			// Final config
			var runConfig = CreateRunConfig(config, competitionAnalyser, logger);
			Summary summary = null;
			try
			{
				summary = RunCore(benchmarkType, runConfig, competitionAnalyser);
			}
			finally
			{
				DumpOutputSummaryAtTop(summary, logger);
			}

			competitionAnalyser.ValidateSummary(summary, minRatio, maxRatio);
		}

		private static void ValidateCompetitionSetup(Type benchmarkType)
		{
			if (!Debugger.IsAttached)
			{
				var assembly = benchmarkType.Assembly;
				if (assembly.IsDebugAssembly())
					throw new InvalidOperationException(
						$"Set the solution configuration into Release mode. Assembly {assembly.GetName().Name} was build as debug.");

				foreach (var referencedAssemblyName in assembly.GetReferencedAssemblies())
				{
					var refAssembly = Assembly.Load(referencedAssemblyName);
					if (refAssembly.IsDebugAssembly())
						throw new InvalidOperationException(
							$"Set the solution configuration into Release mode. Assembly {refAssembly.GetName().Name} was build as debug.");
				}
			}
		}

		private static AccumulationLogger InitAccumulationLogger()
		{
			var logger = new AccumulationLogger();
			logger.WriteLine();
			logger.WriteLine();
			logger.WriteLine(new string('=', 40));
			logger.WriteLine();
			return logger;
		}

		private static IConfig CreateRunConfig(IConfig config, CompetitionAnalyser runState, AccumulationLogger logger)
		{
			// TODO: better setup?
			var result = BenchmarkHelpers.CreateUnitTestConfig(config ?? DefaultConfig.Instance);
			result.Add(runState);
			result.Add(logger);
			result.Add(
				StatisticColumn.Min,
				ScaledPercentileColumn.S0Column,
				ScaledPercentileColumn.S50Column,
				ScaledPercentileColumn.S85Column,
				ScaledPercentileColumn.S95Column,
				ScaledPercentileColumn.S100Column,
				StatisticColumn.Max);
			return result;
		}

		private static Summary RunCore(Type benchmarkType, IConfig runConfig, CompetitionAnalyser runState)
		{
			Summary summary = null;

			const int rerunCount = 10;
			for (var i = 0; i < rerunCount; i++)
			{
				runState.LastRun = i == rerunCount - 1;
				runState.RerunRequested = false;

				// Running the benchmark
				summary = BenchmarkRunner.Run(benchmarkType, runConfig);

				// Rerun if annotated
				if (!runState.RerunRequested)
				{
					break;
				}
			}

			return summary;
		}

		private static void DumpOutputSummaryAtTop(Summary summary, AccumulationLogger logger)
		{
			if (summary != null)
			{
				// Dumping the benchmark results to console
				MarkdownExporter.Default.ExportToLog(summary, ConsoleLogger.Default);
			}

			// Dumping all captured output below the benchmark results
			ConsoleLogger.Default.WriteLine(logger.GetLog());
		}
		#endregion
	}
}