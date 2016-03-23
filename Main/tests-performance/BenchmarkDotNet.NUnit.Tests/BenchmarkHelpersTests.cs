using System;
using System.Collections.Generic;

using NUnit.Framework;

using System.Reflection;
using System.Runtime.CompilerServices;

using BenchmarkDotNet.Helpers;

namespace BenchmarkDotNet.NUnit.Tests
{
	internal class BenchmarkHelpersTests
	{
		[Test]
		[MethodImpl(MethodImplOptions.NoInlining)]
		[System.ComponentModel.Description("TryGetAttributeTest")]
		public void TryGetAttributeTest()
		{
			var method = (MethodInfo)MethodBase.GetCurrentMethod();

			Assert.IsNull(method.TryGetAttribute<AuthorAttribute>());
			Assert.IsNotNull(method.TryGetAttribute<TestAttribute>());
			Assert.AreEqual(method.MethodImplementationFlags, MethodImplAttributes.NoInlining);
			Assert.AreEqual(
				method.TryGetAttribute<System.ComponentModel.DescriptionAttribute>().Description,
				"TryGetAttributeTest");
		}

		#region Percentile
		/// <summary>
		///  Some pseudo-random values
		/// </summary>
		public double[] Data = { 1, 2, 3, 4, 1, 2, 3, 1, 3, 45, 12, 43, 11 };
		/// <summary>
		/// Output from Excel's PERCENTILE()
		/// </summary>
		public Dictionary<double, double> ExpectedPercentiles = new Dictionary<double, double>
		{
			{ 0.0, 1 },
			{ 0.1, 1 },
			{ 0.2, 1.4 },
			{ 0.3, 2 },
			{ 0.4, 2.8 },
			{ 0.5, 3 },
			{ 0.6, 3.2 },
			{ 0.7, 6.8 },
			{ 0.8, 11.6 },
			{ 0.9, 36.8 },
			{ 1.0, 45 }
		};

		private const double Delta = 1e-7;

		[Test]
		public void PercentileTest()
		{
			Assert.Throws<ArgumentNullException>(() => BenchmarkHelpers.Percentile(null, 0));
			Assert.Throws<ArgumentOutOfRangeException>(() => BenchmarkHelpers.Percentile(Data, -0.1));
			Assert.Throws<ArgumentOutOfRangeException>(() => BenchmarkHelpers.Percentile(Data, 1.1));

			Assert.AreEqual(BenchmarkHelpers.Percentile(new double[0], 0), 0);
			Assert.AreEqual(BenchmarkHelpers.Percentile(new double[0], 0.5), 0);
			Assert.AreEqual(BenchmarkHelpers.Percentile(new double[0], 1), 0);

			foreach (var pair in ExpectedPercentiles)
			{
				Assert.AreEqual(BenchmarkHelpers.Percentile(Data, pair.Key), pair.Value, Delta);
			}
		}
		#endregion
	}
}