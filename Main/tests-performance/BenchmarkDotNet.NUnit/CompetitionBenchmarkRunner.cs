using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

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
// ReSharper disable ConvertMethodToExpressionBody

namespace BenchmarkDotNet.NUnit
{
	[PublicAPI]
	public static class CompetitionBenchmarkRunner
	{
		#region Public API overloads
		/// <summary>
		/// Runs the competition benchmark from a type of a callee
		/// </summary>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Run<T>(T thisReference) where T : class
		{
			RunCompetition(0, 0, thisReference.GetType(), null);
		}

		/// <summary>
		/// Runs the competition benchmark from a type of a callee
		/// </summary>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Run<T>(T thisReference, IConfig config) where T : class
		{
			RunCompetition(0, 0, thisReference.GetType(), config);
		}

		/// <summary>
		/// Runs the competition benchmark
		/// </summary>
		public static void Run<T>(IConfig config) where T : class
		{
			RunCompetition(0, 0, typeof(T), config);
		}
		#endregion

		#region Core logic
		/// <summary>
		/// Runs the competition benchmark
		/// </summary>
		// BASEDON: https://github.com/PerfDotNet/BenchmarkDotNet/blob/master/BenchmarkDotNet.IntegrationTests/PerformanceUnitTest.cs
		public static void RunCompetition(
			double minRatio, double maxRatio, Type benchType, IConfig config)
		{
			var currentDirectory = Environment.CurrentDirectory;
			try
			{
				// WORKAROUND: fixing the https://github.com/nunit/nunit3-vs-adapter/issues/96
				Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;

				RunCompetitionUnderSetup(minRatio, maxRatio, benchType, config);
			}
			finally
			{
				Environment.CurrentDirectory = currentDirectory;
			}
		}

		private static void RunCompetitionUnderSetup(
			double minRatio, double maxRatio, Type benchType, IConfig config)
		{
			ValidateCompetition(benchType);

			// Capturing the output
			var logger = InitAccumulationLogger();
			// Shared state
			var runState = new FinalRunAnalyser();
			// Final config
			var runConfig = CreateRunConfig(config, runState, logger);
			Summary summary = null;
			try
			{
				summary = RunCore(benchType, runConfig, runState);
			}
			finally
			{
				DumpOutputSummaryAtTop(summary, logger);
			}

			var culture = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
				runState.ProcessSummary(summary, minRatio, maxRatio);
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = culture;
			}
		}

		private static void ValidateCompetition(Type benchType)
		{
			if (!Debugger.IsAttached)
			{
				var assembly = benchType.Assembly;
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

		private static IConfig CreateRunConfig(IConfig config, FinalRunAnalyser runState, AccumulationLogger logger)
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

		private static Summary RunCore(Type benchType, IConfig runConfig, FinalRunAnalyser runState)
		{
			const int rerunCount = 10;
			Summary summary = null;
			for (var i = 0; i < rerunCount; i++)
			{
				runState.LastRun = i == rerunCount - 1;
				runState.RerunRequested = false;

				// Running the benchmark
				summary = BenchmarkRunner.Run(benchType, runConfig);

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