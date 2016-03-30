using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.NUnit;

using CodeJam.Collections;

using NUnit.Framework;

namespace CodeJam.Tests.Performance
{
	[TestFixture(Category = "Performance")]
	[Config(typeof(FastRunConfig))]
	[SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
	public class ArrayExtensionsBenchmark
	{
		[Test]
		[Explicit(
			"Autorun disabled as it takes too long to run. If needed, select the test in the Test Explorer and run it manually")]
		public void ArrayEquals()
		{
			CompetitionBenchmarkRunner.Run<ArrayEqualsBenchmark>(0.95, 1.05);
		}

		public class ArrayEqualsBenchmark
		{
			public const int RepeatCount = 1000 * 1000;

			public byte[] Data1 { get; set; }
			public byte[] Data2 { get; set; }

			public ArrayEqualsBenchmark()
			{
				var rnd = new Random(0);
				Data1 = Enumerable.Repeat(0, 100).Select(i => (byte)rnd.Next(255)).ToArray();
				Data2 = Data1.ToArray();
			}

			public static bool EqualsTo(byte[] a, byte[] b)
			{
				if (a == b)
					return true;

				if (a == null || b == null)
					return false;

				if (a.Length != b.Length)
					return false;

				for (var i = 0; i < a.Length; i++)
					if (a[i] != b[i])
						return false;

				return true;
			}

			public static bool EqualsTo<T>(T[] a, T[] b) where T : struct, IEquatable<T>
			{
				if (a == b)
					return true;

				if (a == null || b == null)
					return false;

				if (a.Length != b.Length)
					return false;

				for (var i = 0; i < a.Length; i++)
					if (!a[i].Equals(b[i]))
						return false;

				return true;
			}

			public static bool EqualsTo2<T>(T[] a, T[] b) where T : IEquatable<T>
			{
				if (a == b)
					return true;

				if (a == null || b == null)
					return false;

				if (a.Length != b.Length)
					return false;

				for (var i = 0; i < a.Length; i++)
					if (!a[i].Equals(b[i]))
						return false;

				return true;
			}

			[Benchmark(Baseline = true)]
			public bool Baseline()
			{
				bool result = false;
				for (int i = 0; i < RepeatCount; i++)
				{
					result = ArrayExtensions.EqualsTo(Data1, Data2);
				}
				return result;
			}

			[Benchmark]
			public bool GenericImpl()
			{
				bool result = false;
				for (int i = 0; i < RepeatCount; i++)
				{
					result = EqualsTo<byte>(Data1, Data2);
				}
				return result;
			}

			[Benchmark]
			public bool GenericImplAny()
			{
				bool result = false;
				for (int i = 0; i < RepeatCount; i++)
				{
					result = EqualsTo2<byte>(Data1, Data2);
				}
				return result;
			}
		}
	}
}