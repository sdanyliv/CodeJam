using System;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.NUnit;

using JetBrains.Annotations;

using NUnit.Framework;

using static CodeJam.AssemblyWideConfig;

namespace CodeJam
{
	/// <summary>
	/// Prooftest: benchmark is not sensitive enough if OperationsPerInvoke is used instead of tight loop.
	/// </summary>
	[TestFixture(Category = BenchmarkConstants.BenchmarkCategory + ": Self-testing")]
	[PublicAPI]
	public class ProofsOpsCountNotSensitiveBenchmark
	{
		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkOperationsPerInvokeSensitivity() =>
			CompetitionBenchmarkRunner.Run(this, RunConfig);

		public const int Count = 1000 * 1000;
		private int _result;

		[Setup]
		public void SetupData() => _result = 0;

		[Benchmark(Baseline = true, OperationsPerInvoke = Count)]
		public int Test00Baseline() => _result = ++_result;

		[CompetitionBenchmark(0.32, 2.16, OperationsPerInvoke = Count)]
		public int Test01PlusTwo() => _result = ++_result + 2;
	}
}