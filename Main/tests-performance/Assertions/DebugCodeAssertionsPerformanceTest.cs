using System;

using BenchmarkDotNet.NUnit;

using JetBrains.Annotations;

using NUnit.Framework;

using static CodeJam.AssemblyWideConfig;

namespace CodeJam.Assertions
{
	/// <summary>
	/// Checks:
	/// 1. Heavy DebugCode assertions has no impact on release build
	/// </summary>
	[TestFixture(Category = BenchmarkConstants.BenchmarkCategory)]
	[PublicAPI]
	public class DebugCodeAssertionsPerformanceTest
	{
		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkDebugCodeAssertions() =>
			CompetitionBenchmarkRunner.Run(this, RunConfig);

		//[Params(10 * 1000, 100 * 1000, 1000 * 1000)]
		public int Count { get; set; } = 100 * 1000;

		[CompetitionBaseline]
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

		[CompetitionBenchmark(0.82, 1.04)]
		public string Test02AssertionExcluded()
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