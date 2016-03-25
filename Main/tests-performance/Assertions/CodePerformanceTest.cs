using System;
using System.Diagnostics.CodeAnalysis;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.NUnit;

using NUnit.Framework;

namespace CodeJam.Tests.Performance
{
	[TestFixture(Category = "Performance")]
	[Config(typeof(FastRunConfig))]
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	[SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
	public class CodePerformanceTest
	{
		//[Params(1000, 10 * 1000, 100 * 1000, 1000 * 1000)]
		public int Count { get; set; } = 100 * 1000;

		private static string GetArg(int i)
		{
			if (i % 10000 == 0)
				return null;

			return i % 2 == 0 ? "a" : "";
		}

		[Test]
		[Explicit(
			"Autorun disabled as it takes too long to run. If needed, select the test in the Test Explorer and run it manually")]
		public void WarrantCodePerformance()
		{
			CompetitionBenchmarkRunner.Run<CodePerformanceTest>(0.85, 1.05);
		}

		[Benchmark(Baseline = true)]
		public string M_00_RunDefaultAssertion()
		{
			string result = "";
			int count = Count;
			for (int i = 0; i < count; i++)
			{
				string arg = GetArg(i);
				try
				{
					if (arg == null)
						throw new ArgumentNullException(nameof(arg));
					result = arg;
				}
				catch (ArgumentException)
				{
				}
			}

			return result;
		}

		[CompetitionBenchmark(0.85, 1.05)]
		public string M_01_Implementation()
		{
			string result = "";
			int count = Count;
			for (int i = 0; i < count; i++)
			{
				string arg = GetArg(i);
				try
				{
					Code.NotNull(arg, nameof(arg));
					result = arg;
				}
				catch (ArgumentException)
				{
				}
			}

			return result;
		}

		[CompetitionBenchmark(0.85, 1.05)]
		public string M_02_ArgAssertion()
		{
			string result = "";
			int count = Count;

			for (int i = 0; i < count; i++)
			{
				string arg = GetArg(i);
				try
				{
					Code.AssertArgument(arg != null, nameof(arg), "Argument should be not null");
					result = arg;
				}
				catch (ArgumentException)
				{
				}
			}

			return result;
		}

		[CompetitionBenchmark(2.15, 2.4)]
		public string M_03_HeavyAssertionFormatArgs()
		{
			string result = "";
			int count = Count;

			for (int i = 0; i < count; i++)
			{
				string arg = GetArg(i);
				try
				{
					Code.AssertArgument(arg != null, nameof(arg), "Argument {0} should be not null", nameof(arg));
					result = arg;
				}
				catch (ArgumentException)
				{
				}
			}

			return result;
		}

		[CompetitionBenchmark(30, 35)]
		public string M_04_HeavyAssertionInterpolateArgs()
		{
			string result = "";
			int count = Count;

			for (int i = 0; i < count; i++)
			{
				string arg = GetArg(i);
				try
				{
					Code.AssertArgument(arg != null, nameof(arg), $"Argument {nameof(arg)} should be not null");
					result = arg;
				}
				catch (ArgumentException)
				{
				}
			}

			return result;
		}

		[CompetitionBenchmark(0.6, 0.9)]
		public string M_05_RunWithoutAssertion()
		{
			string result = "";
			int count = Count;
			for (int i = 0; i < count; i++)
			{
				string arg = GetArg(i);
				try
				{
					result = arg;
				}
				catch (ArgumentException)
				{
				}
			}

			return result;
		}
	}
}