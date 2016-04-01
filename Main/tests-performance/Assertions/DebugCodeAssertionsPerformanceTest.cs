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
	[PublicAPI]
	public class DebugCodeAssertionsPerformanceTest
	{
		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkDebugCodeAssertions() => CompetitionBenchmarkRunner.Run(this, 1, 1);

		//[Params(10 * 1000, 100 * 1000, 1000 * 1000)]
		public int Count { get; set; } = 100 * 1000;
		
		[Benchmark(Baseline = true)]
		public string Test00RunWithoutAssertion()
		{
			var result = "";
			var count = Count;
			for (var i = 0; i < count; i++)
			{
				result = "!";
			}

			return result;
		}

		[CompetitionBenchmark(0.95, 1.05)]
		public string Test02CodeNotNullExcluded()
		{
			var result = "";
			var count = Count;
			for (var i = 0; i < count; i++)
			{
				result = "!";
				// ReSharper disable once InvocationIsSkipped
				DebugCode.AssertArgument(result == "!", nameof(result), $"{result} != '!'");
			}

			return result;
		}
	}
}