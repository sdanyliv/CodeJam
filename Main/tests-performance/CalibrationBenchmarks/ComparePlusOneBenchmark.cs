using System.Diagnostics.CodeAnalysis;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.NUnit;

using NUnit.Framework;

namespace CodeJam.Tests.Performance
{
	[Config(typeof(FastRunConfig))]
	[SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
	public class ComparePlusOneBenchmark
	{
		[Params(1000, 10 * 1000, 100 * 1000, 1000 * 1000)]
		public int Count { get; set; }

		[Test]
		[Explicit(
			"Autorun disabled as it takes too long to run. If needed, select the test in the Test Explorer and run it manually")]
		public void ComparePlusOne()
		{
			CompetitionBenchmarkRunner.Run<ComparePlusOneBenchmark>(1.5, 1.75);
		}

		[Benchmark(Baseline = true)]
		public int Baseline()
		{
			int sum = 0;
			int count = Count;
			for (int i = 0; i < count; i++)
			{
				sum += i;
			}

			return sum;
		}

		[Benchmark]
		public int PlusOne()
		{
			int sum = 0;
			int count = Count;
			for (int i = 0; i < count; i++)
			{
				sum += i + 1;
			}

			return sum;
		}
	}
}