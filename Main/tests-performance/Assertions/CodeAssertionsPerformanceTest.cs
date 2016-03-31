using System;
using System.Diagnostics.CodeAnalysis;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.NUnit;

using JetBrains.Annotations;

using NUnit.Framework;

namespace CodeJam.Assertions
{
	[TestFixture(Category = BenchmarkConstants.BenchmarkCategory)]
	[Config(typeof(FastRunConfig))]
	[SuppressMessage("ReSharper", "PassStringInterpolation")]
	[PublicAPI]
	public class CodeAssertionsPerformanceTest
	{
		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkCodeAssertions() => CompetitionBenchmarkRunner.Run(this, 0.75, 1.1);

		//[Params(10 * 1000, 100 * 1000, 1000 * 1000)]
		public int Count { get; set; } = 100 * 1000;

		private static string GetArg(int i) => i % 2 == 0 ? "0" : "1";

		[Benchmark(Baseline = true)]
		public string Test00RunWithoutAssertion()
		{
			var result = "";
			var count = Count;
			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);
				result = arg;
			}

			return result;
		}

		[CompetitionBenchmark(0.85, 1.1)]
		public string Test01RunDefaultAssertion()
		{
			var result = "";
			var count = Count;
			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);

				if (arg == null)
					throw new ArgumentNullException(nameof(arg));
				result = arg;
			}

			return result;
		}

		[CompetitionBenchmark(0.85, 1.1)]
		public string Test02CodeNotNull()
		{
			var result = "";
			var count = Count;
			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);

				Code.NotNull(arg, nameof(arg));
				result = arg;
			}

			return result;
		}

		[CompetitionBenchmark(0.85, 1.1)]
		public string Test03CodeAssertArgument()
		{
			var result = "";
			var count = Count;

			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);

				Code.AssertArgument(arg != null, nameof(arg), "Argument should be not null");
				result = arg;
			}

			return result;
		}

		[CompetitionBenchmark(3.9, 8.9)]
		public string Test04CodeAssertArgumentFormat()
		{
			var result = "";
			var count = Count;

			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);

				Code.AssertArgument(arg != null, nameof(arg), "Argument {0} should be not null", nameof(arg));
				result = arg;
			}

			return result;
		}

		[CompetitionBenchmark(3.9, 8.9)]
		public string Test05CodeAssertArgumentInterpolateArgs()
		{
			var result = "";
			var count = Count;

			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);

				Code.AssertArgument(arg != null, nameof(arg), $"Argument {nameof(arg)} should be not null");
				result = arg;
			}

			return result;
		}
	}
}