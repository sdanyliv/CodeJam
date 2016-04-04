﻿using System;

namespace CodeJam
{
	/// <summary>
	/// Class with constants used to be used in a benchmark code
	/// </summary>
	public static class BenchmarkConstants
	{
		/// <summary>
		/// Please, mark all benchmark classes with [TestFixture(Category = BenchmarkConstatnts.BenchmarkCategory)].
		/// That way it's easier to sort out them in a Test Explorer window
		/// </summary>
		public const string BenchmarkCategory = "Performance";

		/// <summary>
		/// Please, mark all benchmark classes with [Explicit(BenchmarkConstatnts.ExplicitExcludeReason)]
		/// Explanation: read the constant' value;)
		/// </summary>
		public const string ExplicitExcludeReason = @"Autorun disabled as it takes too long to run.
Also, running this on debug builds may produce inaccurate results.
Please, run it manually from the Test Explorer window. Remember to use release builds. Thanks and have a nice day:)";
	}
}