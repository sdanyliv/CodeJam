using System;

using BenchmarkDotNet.Attributes;
// ReSharper disable CheckNamespace

namespace BenchmarkDotNet.NUnit
{
	// ReSharper disable once RedundantAttributeUsageProperty
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	public class CompetitionBenchmarkAttribute : BenchmarkAttribute
	{
		public CompetitionBenchmarkAttribute()
		{
		}

		public CompetitionBenchmarkAttribute(double minRatio, double maxRatio)
		{
			MinRatio = minRatio;
			MaxRatio = maxRatio;
		}

		public bool DoesNotCompete { get; set; }

		public double MaxRatio { get; private set; }

		public double MinRatio { get; private set; }
	}
}