using System;
using System.Diagnostics;

using JetBrains.Annotations;

namespace CodeJam.Ranges
{
	/// <summary>
	/// A set of initialization methods for instances of <see cref="Range{TValue}"/>.
	/// </summary>
	[PublicAPI]
	public static class Range
	{
		/// <summary>
		/// Returns empty range.
		/// </summary>
		/// <typeparam name="TValue">Type of range value</typeparam>
		/// <returns>Predefined Empty range.</returns>
		public static Range<TValue> Empty<TValue>() where TValue : IComparable<TValue> => Range<TValue>.Empty;

		/// <summary>
		/// Returns full range.
		/// </summary>
		/// <typeparam name="TValue"></typeparam>
		/// <returns>Predefined Full range.</returns>
		public static Range<TValue> Full<TValue>() where TValue : IComparable<TValue> => Range<TValue>.Full;

		/// <summary>
		/// Creates new instance of finite range<see cref="Range{TValue}"/>.
		/// </summary>
		/// <typeparam name="TValue">Type of range value</typeparam>
		/// <param name="start">Lower bound value of the range</param>
		/// <param name="end">Upper bound value of the range</param>
		/// <param name="includeStart">Indicates whenever Lower bound should be inclusive in the range.</param>
		/// <param name="includeEnd">Indicates whenever Upper bound should be inclusive in the range.</param>
		/// <returns>A new range.</returns>
		[DebuggerStepThrough]
		public static Range<TValue> Create<TValue>(TValue start, TValue end, bool includeStart, bool includeEnd)
			where TValue : IComparable<TValue> =>
				new Range<TValue>(
					start,
					end,
					RangeOptions.HasStart | RangeOptions.HasEnd
						| (includeStart ? RangeOptions.IncludingStart : RangeOptions.None)
						| (includeEnd ? RangeOptions.IncludingEnd : RangeOptions.None));

		/// <summary>
		/// Creates new instance of finite range<see cref="Range{TValue}"/>.
		/// </summary>
		/// <typeparam name="TValue">Type of range value</typeparam>
		/// <param name="start">Lower bound value of the range.</param>
		/// <param name="end">Upper bound value of the range.</param>
		/// <param name="include">Indicates whenever Lower and Upper bounds should be inclusive in the range.</param>
		/// <returns>A new range.</returns>
		[DebuggerStepThrough]
		public static Range<TValue> Create<TValue>(TValue start, TValue end, bool include = true)
			where TValue : IComparable<TValue> =>
				Create(start, end, include, include);

		/// <summary>
		/// Creates new instance of infinite range<see cref="Range{TValue}"/>.
		/// </summary>
		/// <typeparam name="TValue">Type of range value</typeparam>
		/// <param name="start">Lower bound value of the range.</param>
		/// <param name="include">Indicates whenever Lower bound should be inclusive in the range.</param>
		/// <returns>A new range.</returns>
		[DebuggerStepThrough]
		public static Range<TValue> StartsWith<TValue>(TValue start, bool include = true)
			where TValue : IComparable<TValue> =>
				new Range<TValue>(
					start,
					default(TValue),
					RangeOptions.HasStart | (include ? RangeOptions.IncludingStart : RangeOptions.None));

		[DebuggerStepThrough]
		public static Range<TValue> Simple<TValue>(TValue value)
			where TValue : IComparable<TValue> => Create(value, value, true, true);

		[DebuggerStepThrough]
		public static Range<TValue> EndsWith<TValue>(TValue end, bool include = true)
			where TValue : IComparable<TValue> =>
				new Range<TValue>(
					default(TValue),
					end,
					RangeOptions.HasEnd | (include ? RangeOptions.IncludingEnd : RangeOptions.None));
	}
}