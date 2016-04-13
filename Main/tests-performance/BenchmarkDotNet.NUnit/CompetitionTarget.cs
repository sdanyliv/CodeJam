using System;
using System.Globalization;

using BenchmarkDotNet.Running;

using JetBrains.Annotations;

// ReSharper disable CheckNamespace

namespace BenchmarkDotNet.NUnit
{
	/// <summary>
	/// Information for competition target
	/// </summary>
	[PublicAPI]
	public class CompetitionTarget
	{
		#region Fields & .ctor
		private static readonly CultureInfo _culture = CultureInfo.InvariantCulture;
		public CompetitionTarget() { }

		public CompetitionTarget(
			Target target, double min, double max, bool usesResourceAnnotation)
		{
			Target = target;
			UsesResourceAnnotation = usesResourceAnnotation;
			Min = min;
			Max = max;
		}
		#endregion

		#region Properties
		public Target Target { get; }
		public bool UsesResourceAnnotation { get; }

		public string CompetitionName => Target.Type.Name;
		public string CandidateName => Target.Method.Name;

		public double Min { get; private set; }
		public double Max { get; private set; }

		public string MinText => Min.ToString("0.00###", _culture);
		public string MaxText => Max.ToString("0.00###", _culture);
		#endregion

		public CompetitionTarget Clone() => new CompetitionTarget(Target, Min, Max, UsesResourceAnnotation);

		// ReSharper disable once CompareOfFloatsByEqualityOperator
		public bool MinIsEmpty => Min == 0;
		// ReSharper disable once CompareOfFloatsByEqualityOperator
		public bool MaxIsEmpty => Max == 0;

		public bool IsEmpty => MinIsEmpty && MaxIsEmpty;

		internal bool UnionWithMin(double min)
		{
			var expanded = false;

			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if (min != 0 && !double.IsInfinity(min) && (MinIsEmpty || Min > min))
			{
				expanded = true;
				Min = min;
			}

			return expanded;
		}

		internal bool UnionWithMax(double max)
		{
			var expanded = false;

			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if (max != 0 && !double.IsInfinity(max) && (MaxIsEmpty || Max < max))
			{
				expanded = true;
				Max = max;
			}

			return expanded;
		}
	}
}