using System;
using System.Diagnostics.CodeAnalysis;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.NUnit;

using NUnit.Framework;

namespace CodeJam.Tests.Performance
{
	[TestFixture(Category = "Performance")]
	[Config(typeof(FastRunConfig))]
	[SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
	public class GenericEqualityBenchmark
	{
		[Params(1000 * 1000)]
		public int Count { get; set; }

		[Test]
		[Explicit(
			"Autorun disabled as it takes too long to run. If needed, select the test in the Test Explorer and run it manually")]
		public void GenericEquality()
		{
			CompetitionBenchmarkRunner.Run<GenericEqualityBenchmark>(0.95, 1.05);
		}

		public bool Equals<T>(T t1, T t2) where T : IEquatable<T>
		{
			return t1.Equals(t2);
		}
		public bool Larger<T>(T t1, T t2) where T : IComparable<T>
		{
			return t1.CompareTo(t2) > 0;
		}

		[Benchmark(Baseline = true)]
		public int M_00_Baseline()
		{
			int sum = 0;
			int count = Count;
			for (int i = 0; i < count; i++)
			{
				if (count == i)
					sum += i;
			}
			return sum;
		}
		[Benchmark]
		public int M_01_Generic()
		{
			int sum = 0;
			int count = Count;
			for (int i = 0; i < count; i++)
			{
				if (Equals(count, i))
					sum += i;
			}
			return sum;
		}
		[Benchmark]
		public int M_02_LargerBaseline()
		{
			int sum = 0;
			int count = Count;
			for (int i = 0; i < count; i++)
			{
				if (count > i)
					sum += i;
			}
			return sum;
		}

		[CompetitionBenchmark(2.45, 2.55)]
		public int M_03_LargerGeneric()
		{
			int sum = 0;
			int count = Count;
			for (int i = 0; i < count; i++)
			{
				if (Larger(count, i))
					sum += i;
			}
			return sum;
		}
	}
}