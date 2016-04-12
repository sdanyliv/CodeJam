using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.NUnit;

using JetBrains.Annotations;

using NUnit.Framework;

using static CodeJam.AssemblyWideConfig;

namespace CodeJam.Arithmetic
{
	/// <summary>
	/// Checks:
	/// 1. Proofs that there's no way to make Operators (of T).Compare faster.
	/// </summary>
	[TestFixture(Category = BenchmarkConstants.BenchmarkCategory + ": Operators")]
	[PublicAPI]
	public class OperatorsComparePerformanceTest
	{
		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkComparisonInt() =>
			CompetitionBenchmarkRunner.Run<IntCase>(RunConfig);

		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkComparisonNullableInt() =>
			CompetitionBenchmarkRunner.Run<NullableIntCase>(RunConfig);

		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkComparisonNullableDateTime() =>
			CompetitionBenchmarkRunner.Run<NullableDateTimeCase>(RunConfig);

		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkComparisonString() =>
			CompetitionBenchmarkRunner.Run<StringCase>(RunConfig);

		[PublicAPI]
		public class IntCase : IntOperatorsBenchmark<int>
		{
			private static readonly Comparer<int> _comparer = Comparer<int>.Default;
			private static readonly Func<int, int, int> _expressionFunc;

			static IntCase()
			{
				Expression<Func<int, int, int>> exp = (a, b) => a.CompareTo(b);
				_expressionFunc = exp.Compile();
			}

			[Benchmark(Baseline = true)]
			public void Test00DirectCompare()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = ValuesA[i].CompareTo(ValuesB[i]);
				}
			}

			[CompetitionBenchmark(1.68, 2.18)]
			public void Test01Operators()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = Operators<int>.Compare(ValuesA[i], ValuesB[i]);
				}
			}

			[CompetitionBenchmark(2.04, 2.70)]
			public void Test02Comparer()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _comparer.Compare(ValuesA[i], ValuesB[i]);
				}
			}

			[CompetitionBenchmark(1.30, 2.66)]
			public void Test03ExpressionFunc()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _expressionFunc(ValuesA[i], ValuesB[i]);
				}
			}
		}

		[PublicAPI]
		public class NullableIntCase : NullableIntOperatorsBenchmark<int>
		{
			private static readonly Comparer<int?> _comparer = Comparer<int?>.Default;
			private static readonly Func<int?, int?, int> _expressionFunc;

			static NullableIntCase()
			{
				Expression<Func<int?, int?, int>> exp = (a, b) => a == b ? 0 : (a > b ? 1 : -1);
				_expressionFunc = exp.Compile();
			}

			[Benchmark(Baseline = true)]
			public void Test00DirectCompare()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					var a = ValuesA[i];
					var b = ValuesB[i];
					Storage = a == b ? 0 : (a > b ? 1 : -1);
				}
			}

			[CompetitionBenchmark(0.83, 1.62)]
			public void Test01Operators()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = Operators<int?>.Compare(ValuesA[i], ValuesB[i]);
				}
			}

			[CompetitionBenchmark(0.82, 1.52)]
			public void Test02Comparer()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _comparer.Compare(ValuesA[i], ValuesB[i]);
				}
			}

			[CompetitionBenchmark(1.43, 2.44)]
			public void Test03ExpressionFunc()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _expressionFunc(ValuesA[i], ValuesB[i]);
				}
			}
		}

		[PublicAPI]
		public class NullableDateTimeCase : NullableDateTimeOperatorsBenchmark<int>
		{
			private static readonly Comparer<DateTime?> _comparer = Comparer<DateTime?>.Default;
			private static readonly Func<DateTime?, DateTime?, int> _expressionFunc;

			static NullableDateTimeCase()
			{
				Expression<Func<DateTime?, DateTime?, int>> exp = (a, b) => a == b ? 0 : (a > b ? 1 : -1);
				_expressionFunc = exp.Compile();
			}

			[Benchmark(Baseline = true)]
			public void Test00DirectCompare()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					var a = ValuesA[i];
					var b = ValuesB[i];
					Storage = a == b ? 0 : (a > b ? 1 : -1);
				}
			}

			[CompetitionBenchmark(0.30, 0.40)]
			public void Test01Operators()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = Operators<DateTime?>.Compare(ValuesA[i], ValuesB[i]);
				}
			}

			[CompetitionBenchmark(0.33, 0.44)]
			public void Test02Comparer()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _comparer.Compare(ValuesA[i], ValuesB[i]);
				}
			}

			[CompetitionBenchmark(0.52, 0.78)]
			public void Test03ExpressionFunc()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _expressionFunc(ValuesA[i], ValuesB[i]);
				}
			}
		}

		[PublicAPI]
		public class StringCase : StringOperatorsBenchmark<int>
		{
			private static readonly Comparer<string> _comparer = Comparer<string>.Default;
			private static readonly Func<string, string, int> _expressionFunc = string.CompareOrdinal;

			[Benchmark(Baseline = true)]
			public void Test00DirectCompare()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = string.Compare(ValuesA[i], ValuesB[i], StringComparison.Ordinal);
				}
			}

			[CompetitionBenchmark(0.99, 1.27)]
			public void Test01Operators()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = Operators<string>.Compare(ValuesA[i], ValuesB[i]);
				}
			}

			[CompetitionBenchmark(15.57, 18.54)]
			public void Test02Comparer()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _comparer.Compare(ValuesA[i], ValuesB[i]);
				}
			}

			[CompetitionBenchmark(0.92, 1.13)]
			public void Test03ExpressionFunc()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _expressionFunc(ValuesA[i], ValuesB[i]);
				}
			}
		}
	}
}