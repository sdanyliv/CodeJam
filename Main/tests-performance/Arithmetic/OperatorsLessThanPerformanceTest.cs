using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using BenchmarkDotNet.NUnit;

using JetBrains.Annotations;

using NUnit.Framework;

using static CodeJam.AssemblyWideConfig;

namespace CodeJam.Arithmetic
{
	/// <summary>
	/// Checks:
	/// 1. Proofs that there's no way to make Operators (of T).LessThan faster. (Fails for now)
	/// </summary>
	[TestFixture(Category = BenchmarkConstants.BenchmarkCategory + ": Operators")]
	[PublicAPI]
	public class OperatorsLessThanPerformanceTest
	{
		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkLessThanInt() =>
			CompetitionBenchmarkRunner.Run<IntCase>(RunConfig);

		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkLessThanNullableInt() =>
			CompetitionBenchmarkRunner.Run<NullableIntCase>(RunConfig);

		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkLessThanNullableDateTime() =>
			CompetitionBenchmarkRunner.Run<NullableDateTimeCase>(RunConfig);

		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkLessThanString()
			=> CompetitionBenchmarkRunner.Run<StringCase>(RunConfig);

		[PublicAPI]
		public class IntCase : IntOperatorsBenchmark<bool>
		{
			private static readonly Comparer<int> _comparer = Comparer<int>.Default;
			private static readonly Func<int, int, bool> _expressionFunc;

			static IntCase()
			{
				Expression<Func<int, int, bool>> exp = (a, b) => a < b;
				_expressionFunc = exp.Compile();
			}

			[CompetitionBaseline]
			public void Test00DirectCompare()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = ValuesA[i] < ValuesB[i];
				}
			}

			[CompetitionBenchmark(1.73, 2.77)]
			public void Test01Operators()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = Operators<int>.LessThan(ValuesA[i], ValuesB[i]);
				}
			}

			[CompetitionBenchmark(2.69, 5.23)]
			public void Test02Comparer()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _comparer.Compare(ValuesA[i], ValuesB[i]) < 0;
				}
			}

			[CompetitionBenchmark(1.77, 3.21)]
			public void Test03ExpressionFunc()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _expressionFunc(ValuesA[i], ValuesB[i]);
				}
			}
		}

		[PublicAPI]
		public class NullableIntCase : NullableIntOperatorsBenchmark<bool>
		{
			private static readonly Comparer<int?> _comparer = Comparer<int?>.Default;
			private static readonly Func<int?, int?, bool> _expressionFunc;

			static NullableIntCase()
			{
				Expression<Func<int?, int?, bool>> exp = (a, b) => a < b;
				_expressionFunc = exp.Compile();
			}

			[CompetitionBaseline]
			public void Test00DirectCompare()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = ValuesA[i] < ValuesB[i];
				}
			}

			[CompetitionBenchmark(1.4, 2.44)]
			public void Test01Operators()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = Operators<int?>.LessThan(ValuesA[i], ValuesB[i]);
				}
			}

			[CompetitionBenchmark(1.41, 3.48)]
			public void Test02Comparer()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _comparer.Compare(ValuesA[i], ValuesB[i]) < 0;
				}
			}

			[CompetitionBenchmark(1.34, 3.59)]
			public void Test03ExpressionFunc()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _expressionFunc(ValuesA[i], ValuesB[i]);
				}
			}
		}

		[PublicAPI]
		public class NullableDateTimeCase : NullableDateTimeOperatorsBenchmark<bool>
		{
			private static readonly Comparer<DateTime?> _comparer = Comparer<DateTime?>.Default;
			private static readonly Func<DateTime?, DateTime?, bool> _expressionFunc;

			static NullableDateTimeCase()
			{
				Expression<Func<DateTime?, DateTime?, bool>> exp = (a, b) => a < b;
				_expressionFunc = exp.Compile();
			}

			[CompetitionBaseline]
			public void Test00DirectCompare()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = ValuesA[i] < ValuesB[i];
				}
			}

			[CompetitionBenchmark(0.52, 1.42)]
			public void Test01Operators()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = Operators<DateTime?>.LessThan(ValuesA[i], ValuesB[i]);
				}
			}

			[CompetitionBenchmark(0.45, 1.10)]
			public void Test02Comparer()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _comparer.Compare(ValuesA[i], ValuesB[i]) < 0;
				}
			}

			[CompetitionBenchmark(0.53, 1.33)]
			public void Test03ExpressionFunc()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _expressionFunc(ValuesA[i], ValuesB[i]);
				}
			}
		}

		[PublicAPI]
		public class StringCase : StringOperatorsBenchmark<bool>
		{
			private static readonly Comparer<string> _comparer = Comparer<string>.Default;
			private static readonly Func<string, string, bool> _expressionFunc = (a, b) => string.CompareOrdinal(a, b) < 0;

			[CompetitionBaseline]
			public void Test00DirectCompare()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = string.Compare(ValuesA[i], ValuesB[i], StringComparison.Ordinal) < 0;
				}
			}

			[CompetitionBenchmark(1.05, 1.5)]
			public void Test01Operators()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = Operators<string>.LessThan(ValuesA[i], ValuesB[i]);
				}
			}

			[CompetitionBenchmark(12.4, 26.6)]
			public void Test02Comparer()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _comparer.Compare(ValuesA[i], ValuesB[i]) < 0;
				}
			}

			[CompetitionBenchmark(0.85, 1.43)]
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