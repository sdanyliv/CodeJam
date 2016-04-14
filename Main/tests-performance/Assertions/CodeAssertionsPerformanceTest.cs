using System;
using System.Diagnostics.CodeAnalysis;

using BenchmarkDotNet.NUnit;

using JetBrains.Annotations;

using NUnit.Framework;

using static CodeJam.AssemblyWideConfig;

namespace CodeJam.Assertions
{
	/// <summary>
	/// Checks:
	/// 1. Assertion implementation methods should be NOT SLOWER then usual if-then-throw approach
	/// 2. Assertion should add no more than 20% penalty on tight loop use-case.
	/// </summary>
	[TestFixture(Category = BenchmarkConstants.BenchmarkCategory)]
	[SuppressMessage("ReSharper", "PassStringInterpolation")]
	[PublicAPI]
	public class CodeAssertionsPerformanceTest
	{
		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkCodeAssertions() =>
			CompetitionBenchmarkRunner.Run(this, RunConfig);

		//[Params(10 * 1000, 100 * 1000, 1000 * 1000)]
		public int Count { get; set; } = 100 * 1000;

		private static string GetArg(int i) => i % 2 == 0 ? "0" : "1";

		[CompetitionBaseline]
		public string Test00RunWithoutAssertion()
		{
			var result = "";
			var count = Count;
			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);
				result = arg;
			}

			return result;
		}

		[CompetitionBenchmark(0.94, 1.14)]
		public string Test01RunDefaultAssertion()
		{
			var result = "";
			var count = Count;
			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);

				if (arg == null)
					throw new ArgumentNullException(nameof(arg));
				result = arg;
			}

			return result;
		}

		[CompetitionBenchmark(0.93, 1.14)]
		public string Test02CodeNotNull()
		{
			var result = "";
			var count = Count;
			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);

				Code.NotNull(arg, nameof(arg));
				result = arg;
			}

			return result;
		}

		[CompetitionBenchmark(0.95, 1.13)]
		public string Test03CodeAssertArgument()
		{
			var result = "";
			var count = Count;

			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);

				Code.AssertArgument(arg != null, nameof(arg), "Argument should be not null");
				result = arg;
			}

			return result;
		}

		[CompetitionBenchmark(7.41, 8.48)]
		public string Test04CodeAssertArgumentFormat()
		{
			var result = "";
			var count = Count;

			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);

				Code.AssertArgument(arg != null, nameof(arg), "Argument {0} should be not null", nameof(arg));
				result = arg;
			}

			return result;
		}

		[CompetitionBenchmark(148.94, 175.52)]
		public string Test05CodeAssertArgumentInterpolateArgs()
		{
			var result = "";
			var count = Count;

			for (var i = 0; i < count; i++)
			{
				var arg = GetArg(i);

				Code.AssertArgument(arg != null, nameof(arg), $"Argument {nameof(arg)} should be not null");
				result = arg;
			}

			return result;
		}
	}
}