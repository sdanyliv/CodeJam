using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.NUnit;

using JetBrains.Annotations;

using NUnit.Framework;

namespace CodeJam.Arithmetic
{
	/// <summary>
	/// Checks:
	/// 1. Proofs that there's no way to make Operators (of T).Compare faster.
	/// </summary>
	[TestFixture(Category = BenchmarkConstants.BenchmarkCategory + ": Operators")]
	[Config(typeof(FastRunConfig))]
	[PublicAPI]
	public class OperatorsComparePerformanceTest
	{
		private const int Count = 10 * 1000;

		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkIntComparison() =>
			CompetitionBenchmarkRunner.Run<IntCase>(1, 1);

		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkNullableIntComparison() =>
			CompetitionBenchmarkRunner.Run<NullableIntCase>(1, 1);

		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkNullableDateTimeComparison() =>
			CompetitionBenchmarkRunner.Run<NullableDateTimeCase>(1, 1);

		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkStringComparison()
			=> CompetitionBenchmarkRunner.Run<StringCase>(1, 1);

		[PublicAPI]
		public class IntCase
		{
			private static readonly Comparer<int> _comparer = Comparer<int>.Default;
			private static readonly Func<int, int, int> _expressionFunc;

			static IntCase()
			{
				Expression<Func<int, int, int>> exp = (a, b) => a.CompareTo(b);
				_expressionFunc = exp.Compile();
			}

			[Benchmark(Baseline = true)]
			public int Test00DirectCompare()
			{
				var result = 0;
				for (var i = 0; i < Count; i++)
				{
					result = 1.CompareTo(i % 5);
				}

				return result;
			}

			[CompetitionBenchmark(1.2, 1.3)]
			public int Test01Operators()
			{
				var result = 0;
				for (var i = 0; i < Count; i++)
				{
					result = Operators<int>.Compare(i, i % 5);
				}

				return result;
			}

			[CompetitionBenchmark(1.2, 1.95)]
			public int Test02Comparer()
			{
				var result = 0;
				for (var i = 0; i < Count; i++)
				{
					result = _comparer.Compare(i, i % 5);
				}

				return result;
			}

			[CompetitionBenchmark(1.2, 1.9)]
			public int Test02ExpressionFunc()
			{
				var result = 0;
				for (var i = 0; i < Count; i++)
				{
					result = _expressionFunc(i, i % 5);
				}

				return result;
			}
		}

		[PublicAPI]
		public class NullableIntCase
		{
			private static readonly Comparer<int?> _comparer = Comparer<int?>.Default;
			private static readonly Func<int?, int?, int> _expressionFunc;

			static NullableIntCase()
			{
				Expression<Func<int?, int?, int>> exp = (a, b) => a == b ? 0 : (a > b ? 1 : -1);
				_expressionFunc = exp.Compile();
			}

			[Benchmark(Baseline = true)]
			public int Test00DirectCompare()
			{
				var result = 0;
				for (var i = 0; i < Count; i++)
				{
					int? a = i;
					int? b = i % 5;
					if (b == 0)
						b = null;
					result = a == b ? 0 : (a > b ? 1 : -1);
				}

				return result;
			}

			[CompetitionBenchmark(0.95, 1.3)]
			public int Test01Operators()
			{
				var result = 0;
				for (var i = 0; i < Count; i++)
				{
					int? a = i;
					int? b = i % 5;
					if (b == 0)
						b = null;
					result = Operators<int?>.Compare(a, b);
				}

				return result;
			}

			[CompetitionBenchmark(0.95, 1.3)]
			public int Test02Comparer()
			{
				var result = 0;
				for (var i = 0; i < Count; i++)
				{
					int? a = i;
					int? b = i % 5;
					if (b == 0)
						b = null;
					result = _comparer.Compare(a, b);
				}

				return result;
			}

			[CompetitionBenchmark(1.2, 1.9)]
			public int Test03ExpressionFunc()
			{
				var result = 0;
				for (var i = 0; i < Count; i++)
				{
					int? a = i;
					int? b = i % 5;
					if (b == 0)
						b = null;
					result = _expressionFunc(a, b);
				}

				return result;
			}
		}

		[PublicAPI]
		public class NullableDateTimeCase
		{
			private static readonly Comparer<DateTime?> _comparer = Comparer<DateTime?>.Default;
			private static readonly Func<DateTime?, DateTime?, int> _expressionFunc;

			static NullableDateTimeCase()
			{
				Expression<Func<DateTime?, DateTime?, int>> exp = (a, b) => a == b ? 0 : (a > b ? 1 : -1);
				_expressionFunc = exp.Compile();
			}

			[Benchmark(Baseline = true)]
			public int Test00DirectCompare()
			{
				var result = 0;
				var dt = DateTime.UtcNow;
				for (var i = 0; i < Count; i++)
				{
					DateTime? a = dt;
					var i2 = i % 5;
					var b = i2 == 0 ? (DateTime?)null : dt.AddDays(i2);

					result = a == b ? 0 : (a > b ? 1 : -1);
				}

				return result;
			}

			[CompetitionBenchmark(0.7, 1.1)]
			public int Test01Operators()
			{
				var result = 0;
				var dt = DateTime.UtcNow;
				for (var i = 0; i < Count; i++)
				{
					DateTime? a = dt;
					var i2 = i % 5;
					var b = i2 == 0 ? (DateTime?)null : dt.AddDays(i2);

					result = Operators<DateTime?>.Compare(a, b);
				}

				return result;
			}

			[CompetitionBenchmark(0.7, 1.1)]
			public int Test02Comparer()
			{
				var result = 0;
				var dt = DateTime.UtcNow;
				for (var i = 0; i < Count; i++)
				{
					DateTime? a = dt;
					var i2 = i % 5;
					var b = i2 == 0 ? (DateTime?)null : dt.AddDays(i2);

					result = _comparer.Compare(a, b);
				}

				return result;
			}

			[CompetitionBenchmark(0.9, 1.3)]
			public int Test03ExpressionFunc()
			{
				var result = 0;
				var dt = DateTime.UtcNow;
				for (var i = 0; i < Count; i++)
				{
					DateTime? a = dt;
					var i2 = i % 5;
					var b = i2 == 0 ? (DateTime?)null : dt.AddDays(i2);

					result = _expressionFunc(a, b);
				}

				return result;
			}
		}

		[PublicAPI]
		public class StringCase
		{
			private static readonly Comparer<string> _comparer = Comparer<string>.Default;

			[Benchmark(Baseline = true)]
			public int Test00DirectCompare()
			{
				var result = 0;
				for (var i = 0; i < Count; i++)
				{
					var a = i.ToString();
					var b = (i % 5).ToString();
					if (a == "0")
						b = null;

					result = string.Compare(a, b, StringComparison.Ordinal);
				}

				return result;
			}

			[CompetitionBenchmark(0.95, 1.15)]
			public int Test01Operators()
			{
				var result = 0;
				for (var i = 0; i < Count; i++)
				{
					var a = i.ToString();
					var b = (i % 5).ToString();
					if (a == "0")
						b = null;

					result = Operators<string>.Compare(a, b);
				}

				return result;
			}

			[CompetitionBenchmark(1.4, 1.7)]
			public int Test02Comparer()
			{
				var result = 0;
				for (var i = 0; i < Count; i++)
				{
					var a = i.ToString();
					var b = (i % 5).ToString();
					if (a == "0")
						b = null;

					result = _comparer.Compare(a, b);
				}

				return result;
			}
		}
	}
}