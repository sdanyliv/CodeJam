using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.NUnit;

using JetBrains.Annotations;

using NUnit.Framework;

namespace CodeJam.Arithmetic
{
	/// <summary>
	/// Checks:
	/// 1. Proofs that there's no way to make Operators (of T).GreaterThanOrEqual faster. (Fails for now)
	/// </summary>
	[TestFixture(Category = BenchmarkConstants.BenchmarkCategory + ": Operators")]
	[Config(typeof(FastRunConfig))]
	[PublicAPI]
	[SuppressMessage("ReSharper", "ArrangeRedundantParentheses")]
	public class OperatorsGreaterThanOrEqualTest
	{
		private const int Count = 10 * 1000;

		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkIntGreaterThanOrEqual() =>
			CompetitionBenchmarkRunner.Run<IntCase>(1, 1);

		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkNullableIntGreaterThanOrEqual() =>
			CompetitionBenchmarkRunner.Run<NullableIntCase>(1, 1);

		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkNullableDateTimeGreaterThanOrEqual() =>
			CompetitionBenchmarkRunner.Run<NullableDateTimeCase>(1, 1);

		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkStringGreaterThanOrEqual()
			=> CompetitionBenchmarkRunner.Run<StringCase>(1, 1);

		[PublicAPI]
		public class IntCase
		{
			private static readonly Comparer<int> _comparer = Comparer<int>.Default;
			private static readonly Func<int, int, bool> _expressionFunc;

			static IntCase()
			{
				Expression<Func<int, int, bool>> exp = (a, b) => a > b;
				_expressionFunc = exp.Compile();
			}

			[Benchmark(Baseline = true)]
			public bool Test00DirectCompare()
			{
				var result = false;
				for (var i = 0; i < Count; i++)
				{
					result = 1 >= (i % 5);
				}

				return result;
			}

			[CompetitionBenchmark(0.9, 1.2)]
			public bool Test01Operators()
			{
				var result = false;
				for (var i = 0; i < Count; i++)
				{
					result = Operators<int>.GreaterThanOrEqual(i, i % 5);
				}

				return result;
			}

			[CompetitionBenchmark(1.2, 1.8)]
			public bool Test02Comparer()
			{
				var result = false;
				for (var i = 0; i < Count; i++)
				{
					result = _comparer.Compare(i, i % 5) >= 0;
				}

				return result;
			}

			[CompetitionBenchmark(0.9, 1.2)]
			public bool Test02ExpressionFunc()
			{
				var result = false;
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
			private static readonly Func<int?, int?, bool> _expressionFunc;

			static NullableIntCase()
			{
				Expression<Func<int?, int?, bool>> exp = (a, b) => a >= b;
				_expressionFunc = exp.Compile();
			}

			[Benchmark(Baseline = true)]
			public bool Test00DirectCompare()
			{
				var result = false;
				for (var i = 0; i < Count; i++)
				{
					int? a = i;
					int? b = i % 5;
					if (b == 0)
						b = null;
					result = a >= b;
				}

				return result;
			}

			[CompetitionBenchmark(1, 1.8)]
			public bool Test01Operators()
			{
				var result = false;
				for (var i = 0; i < Count; i++)
				{
					int? a = i;
					int? b = i % 5;
					if (b == 0)
						b = null;
					result = Operators<int?>.GreaterThanOrEqual(a, b);
				}

				return result;
			}

			[CompetitionBenchmark(1, 1.8)]
			public bool Test02Comparer()
			{
				var result = false;
				for (var i = 0; i < Count; i++)
				{
					int? a = i;
					int? b = i % 5;
					if (b == 0)
						b = null;
					result = _comparer.Compare(a, b) >= 0;
				}

				return result;
			}

			[CompetitionBenchmark(1, 1.8)]
			public bool Test03ExpressionFunc()
			{
				var result = false;
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
			private static readonly Func<DateTime?, DateTime?, bool> _expressionFunc;

			static NullableDateTimeCase()
			{
				Expression<Func<DateTime?, DateTime?, bool>> exp = (a, b) => a >= b;
				_expressionFunc = exp.Compile();
			}

			[Benchmark(Baseline = true)]
			public bool Test00DirectCompare()
			{
				var result = false;
				var dt = DateTime.UtcNow;
				for (var i = 0; i < Count; i++)
				{
					DateTime? a = dt;
					var i2 = i % 5;
					var b = i2 == 0 ? (DateTime?)null : dt.AddDays(i2);

					result = a >= b;
				}

				return result;
			}

			[CompetitionBenchmark(0.9, 1.25)]
			public bool Test01Operators()
			{
				var result = false;
				var dt = DateTime.UtcNow;
				for (var i = 0; i < Count; i++)
				{
					DateTime? a = dt;
					var i2 = i % 5;
					var b = i2 == 0 ? (DateTime?)null : dt.AddDays(i2);

					result = Operators<DateTime?>.GreaterThanOrEqual(a, b);
				}

				return result;
			}

			[CompetitionBenchmark(0.9, 1.4)]
			public bool Test02Comparer()
			{
				var result = false;
				var dt = DateTime.UtcNow;
				for (var i = 0; i < Count; i++)
				{
					DateTime? a = dt;
					var i2 = i % 5;
					var b = i2 == 0 ? (DateTime?)null : dt.AddDays(i2);

					result = _comparer.Compare(a, b) >= 0;
				}

				return result;
			}

			[CompetitionBenchmark(0.9, 1.25)]
			public bool Test03ExpressionFunc()
			{
				var result = false;
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
			public bool Test00DirectCompare()
			{
				var result = false;
				for (var i = 0; i < Count; i++)
				{
					var a = i.ToString();
					var b = (i % 5).ToString();
					if (a == "0")
						b = null;

					result = string.Compare(a, b, StringComparison.Ordinal) >= 0;
				}

				return result;
			}

			[CompetitionBenchmark(0.9, 1.3)]
			public bool Test01Operators()
			{
				var result = false;
				for (var i = 0; i < Count; i++)
				{
					var a = i.ToString();
					var b = (i % 5).ToString();
					if (a == "0")
						b = null;

					result = Operators<string>.GreaterThanOrEqual(a, b);
				}

				return result;
			}

			[CompetitionBenchmark(1.4, 1.7)]
			public bool Test02Comparer()
			{
				var result = false;
				for (var i = 0; i < Count; i++)
				{
					var a = i.ToString();
					var b = (i % 5).ToString();
					if (a == "0")
						b = null;

					result = _comparer.Compare(a, b) >= 0;
				}

				return result;
			}
		}
	}
}