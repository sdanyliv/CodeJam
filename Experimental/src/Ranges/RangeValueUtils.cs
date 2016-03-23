using System;

using JetBrains.Annotations;

namespace CodeJam.Ranges
{
	[PublicAPI]
	public static class RangeValueUtils
	{
		public static TValue Min<TValue>(TValue value1, TValue value2)
			where TValue : IComparable<TValue> =>
				value1.CompareTo(value2) > 0 ? value2 : value1;

		public static TValue Max<TValue>(TValue value1, TValue value2)
			where TValue : IComparable<TValue> =>
				value1.CompareTo(value2) > 0 ? value1 : value2;
	}
}