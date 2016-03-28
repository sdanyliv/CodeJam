using System;
using System.Diagnostics.CodeAnalysis;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.NUnit;

using NUnit.Framework;

namespace CodeJam.Assertions
{
	[TestFixture(Category =BenchmarkConstants.BenchmarkCategory)]
	[Config(typeof(FastRunConfig))]
	[SuppressMessage("ReSharper", "PassStringInterpolation")]
	public class CodeAssertionsPerformanceTest
	{
		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkCodeAssertions() => CompetitionBenchmarkRunner.Run(0.75, 1.1);

		//[Params(1000, 10 * 1000, 100 * 1000, 1000 * 1000)]
		public int Count { get; set; } = 100 * 1000;

		private static string GetArg(int i)
		{
			if (i % 10000 == 0)
				return null;

			return i % 2 == 0 ? "a" : "";
		}

		[Benchmark(Baseline = true)]
		public string Test00RunDefaultAssertion()
		{
			var result = "";
			var count = Count;
			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);
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

		[CompetitionBenchmark(0.75, 1.1)]
		public string Test01Implementation()
		{
			var result = "";
			var count = Count;
			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);
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

		[CompetitionBenchmark(0.75, 1.1)]
		public string Test02ArgAssertion()
		{
			var result = "";
			var count = Count;

			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);
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

		[CompetitionBenchmark(1.7, 2.7)]
		public string Test03HeavyAssertionFormatArgs()
		{
			var result = "";
			var count = Count;

			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);
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

		[CompetitionBenchmark(25, 35)]
		public string Test04HeavyAssertionInterpolateArgs()
		{
			var result = "";
			var count = Count;

			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);
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

		[CompetitionBenchmark(0.5, 0.9)]
		public string Test05RunWithoutAssertion()
		{
			var result = "";
			var count = Count;
			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);
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