using System;

using BenchmarkDotNet.Attributes;

using JetBrains.Annotations;

// ReSharper disable CheckNamespace
// ReSharper disable once RedundantAttributeUsageProperty

namespace BenchmarkDotNet.NUnit
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	[PublicAPI, MeansImplicitUse]
	public class CompetitionBaselineAttribute : BenchmarkAttribute
	{
		public CompetitionBaselineAttribute()
		{
			Baseline = true;
		}
	}
}