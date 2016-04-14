using System;

using BenchmarkDotNet.Attributes;

using JetBrains.Annotations;

// ReSharper disable CheckNamespace
// ReSharper disable once RedundantAttributeUsageProperty

namespace BenchmarkDotNet.NUnit
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	[PublicAPI, MeansImplicitUse]
	public class CompetitionBenchmarkAttribute : BenchmarkAttribute
	{
		public CompetitionBenchmarkAttribute() { }

		public CompetitionBenchmarkAttribute(double maxRatio)
		{
			MinRatio = -1;
			MaxRatio = maxRatio;
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