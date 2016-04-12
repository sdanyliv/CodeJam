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
	/// 1. Proofs that there's no way to make Operators (of T).GreaterThanOrEqual faster. (Fails for now)
	/// </summary>
	[TestFixture(Category = BenchmarkConstants.BenchmarkCategory + ": Operators")]
	[PublicAPI]
	public class OperatorsGreaterThanOrEqualPerformanceTest
	{
		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkGreaterThanOrEqualInt() =>
			CompetitionBenchmarkRunner.Run<IntCase>(RunConfig);

		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkGreaterThanOrEquaNullableInt() =>
			CompetitionBenchmarkRunner.Run<NullableIntCase>(RunConfig);

		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkGreaterThanOrEqualNullableDateTime() =>
			CompetitionBenchmarkRunner.Run<NullableDateTimeCase>(RunConfig);

		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkGreaterThanOrEqualString()
			=> CompetitionBenchmarkRunner.Run<StringCase>(RunConfig);

		[PublicAPI]
		public class IntCase : IntOperatorsBenchmark<bool>
		{
			private static readonly Comparer<int> _comparer = Comparer<int>.Default;
			private static readonly Func<int, int, bool> _expressionFunc;

			static IntCase()
			{
				Expression<Func<int, int, bool>> exp = (a, b) => a >= b;
				_expressionFunc = exp.Compile();
			}

			[CompetitionBaseline]
			public void Test00DirectCompare()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = ValuesA[i] >= ValuesB[i];
				}
			}

			[CompetitionBenchmark(2.10, 3.04)]
			public void Test01Operators()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = Operators<int>.GreaterThanOrEqual(ValuesA[i], ValuesB[i]);
				}
			}

			[CompetitionBenchmark(2.42, 4.02)]
			public void Test02Comparer()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _comparer.Compare(ValuesA[i], ValuesB[i]) >= 0;
				}
			}

			[CompetitionBenchmark(2.10, 3.18)]
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
				Expression<Func<int?, int?, bool>> exp = (a, b) => a >= b;
				_expressionFunc = exp.Compile();
			}

			[CompetitionBaseline]
			public void Test00DirectCompare()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = ValuesA[i] >= ValuesB[i];
				}
			}

			[CompetitionBenchmark(1.42, 3.20)]
			public void Test01Operators()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = Operators<int?>.GreaterThanOrEqual(ValuesA[i], ValuesB[i]);
				}
			}

			[CompetitionBenchmark(1.63, 2.52)]
			public void Test02Comparer()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _comparer.Compare(ValuesA[i], ValuesB[i]) >= 0;
				}
			}

			[CompetitionBenchmark(1.46, 2.29)]
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
				Expression<Func<DateTime?, DateTime?, bool>> exp = (a, b) => a >= b;
				_expressionFunc = exp.Compile();
			}

			[CompetitionBaseline]
			public void Test00DirectCompare()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = ValuesA[i] >= ValuesB[i];
				}
			}

			[CompetitionBenchmark(1.04, 1.30)]
			public void Test01Operators()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = Operators<DateTime?>.GreaterThanOrEqual(ValuesA[i], ValuesB[i]);
				}
			}

			[CompetitionBenchmark(0.81, 1.00)]
			public void Test02Comparer()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _comparer.Compare(ValuesA[i], ValuesB[i]) >= 0;
				}
			}

			[CompetitionBenchmark(0.95, 1.43)]
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
			private static readonly Func<string, string, bool> _expressionFunc = (a, b) => string.CompareOrdinal(a, b) >= 0;

			[CompetitionBaseline]
			public void Test00DirectCompare()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = string.Compare(ValuesA[i], ValuesB[i], StringComparison.Ordinal) >= 0;
				}
			}

			[CompetitionBenchmark(0.93, 1.42)]
			public void Test01Operators()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = Operators<string>.GreaterThanOrEqual(ValuesA[i], ValuesB[i]);
				}
			}

			[CompetitionBenchmark(12.24, 20.22)]
			public void Test02Comparer()
			{
				for (var i = 0; i < ValuesA.Length; i++)
				{
					Storage = _comparer.Compare(ValuesA[i], ValuesB[i]) >= 0;
				}
			}

			[CompetitionBenchmark(0.75, 1.31)]
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