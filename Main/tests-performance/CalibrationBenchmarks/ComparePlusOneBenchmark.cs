using System;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.NUnit;

using JetBrains.Annotations;

using NUnit.Framework;

namespace CodeJam.Tests.Performance
{
	[Config(typeof (FastRunConfig))]
	public class ComparePlusOneBenchmark
	{
		[Params(1000, 10*1000, 100*1000, 1000*1000)]
		// ReSharper disable once UnusedAutoPropertyAccessor.Local
		private int Count { get; set; }

		[Test]
		[Explicit(
			"Autorun disabled as it takes too long to run. If needed, select the test in the Test Explorer and run it manually")]
		public void ComparePlusOne() => CompetitionBenchmarkRunner.Run<ComparePlusOneBenchmark>(1.5, 1.75);

		[Benchmark(Baseline = true)]
		[UsedImplicitly]
		public int Baseline()
		{
			var sum = 0;
			var count = Count;
			for (var i = 0; i < count; i++) sum += i;
			return sum;
		}

		[Benchmark]
		[UsedImplicitly]
		public int PlusOne()
		{
			var sum = 0;
			var count = Count;
			for (var i = 0; i < count; i++) sum += i + 1;
			return sum;
		}
	}
}