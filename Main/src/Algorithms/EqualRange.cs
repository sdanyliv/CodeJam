using System;
using System.Collections.Generic;

namespace CodeJam
{
	partial class Algorithms
	{
		/// <summary>
		/// Returns the tuple of [i, j] where
		///		i is the smallest index in the range [0, list.Count - 1] such that list[i] >= value or list.Count if no such i exists
		///		j is the smallest index in the range [0, list.Count - 1] such that list[i] > value or list.Count if no such j exists
		/// <remarks>Comparer&lt;T&gt;.Default is being used for comparison</remarks>
		/// </summary>
		/// <typeparam name="TElement">The list element type</typeparam>
		/// <param name="list">The sorted list</param>
		/// <param name="value">The value to compare</param>
		/// <returns>The tuple of lower bound and upper bound for the value</returns>
		public static Tuple<int, int> EqualRange<TElement>(this IList<TElement> list, TElement value)
			=> EqualRange(list, value, Comparer<TElement>.Default.Compare);

		/// <summary>
		/// Returns the tuple of [i, j] where
		///		i is the smallest index in the range [from, list.Count - 1] such that list[i] >= value or list.Count if no such i exists
		///		j is the smallest index in the range [from, list.Count - 1] such that list[i] > value or list.Count if no such j exists
		/// <remarks>Comparer&lt;T&gt;.Default is being used for comparison</remarks>
		/// </summary>
		/// <typeparam name="TElement">The list element type</typeparam>
		/// <param name="list">The sorted list</param>
		/// <param name="value">The value to compare</param>
		/// <param name="from">The minimum index</param>
		/// <returns>The tuple of lower bound and upper bound for the value</returns>
		public static Tuple<int, int> EqualRange<TElement>(this IList<TElement> list, TElement value, int from)
			=> list.EqualRange(value, @from, list.Count, Comparer<TElement>.Default.Compare);

		/// <summary>
		/// Returns the tuple of [i, j] where
		///		i is the smallest index in the range [0, list.Count - 1] such that list[i] >= value or list.Count if no such i exists
		///		j is the smallest index in the range [0, list.Count - 1] such that list[i] > value or list.Count if no such j exists
		/// </summary>
		/// <typeparam name="TElement">The list element type</typeparam>
		/// <typeparam name="TValue">The type of the value</typeparam>
		/// <param name="list">The sorted list</param>
		/// <param name="value">The value to compare</param>
		/// <param name="comparer">The function with the Comparer&lt;T&gt;.Compare semantics</param>
		/// <returns>The tuple of lower bound and upper bound for the value</returns>
		public static Tuple<int, int> EqualRange<TElement, TValue>(this IList<TElement> list, TValue value, Func<TElement, TValue, int> comparer)
			=> EqualRange(list, value, 0, list.Count, comparer);

		/// <summary>
		/// Returns the tuple of [i, j] where
		///		i is the smallest index in the range [from, to - 1] such that list[i] >= value or "to" if no such i exists
		///		j is the smallest index in the range [from, to - 1] such that list[i] > value or "to" if no such j exists
		/// </summary>
		/// <typeparam name="TElement">The list element type</typeparam>
		/// <typeparam name="TValue">The type of the value</typeparam>
		/// <param name="list">The sorted list</param>
		/// <param name="value">The value to compare</param>
		/// <param name="from">The minimum index</param>
		/// <param name="to">The upper bound for the index (not included)</param>
		/// <param name="comparer">The function with the Comparer&lt;T&gt;.Compare semantics</param>
		/// <returns>The tuple of lower bound and upper bound for the value</returns>
		public static Tuple<int, int> EqualRange<TElement, TValue>(this IList<TElement> list, TValue value, int from, int to, Func<TElement, TValue, int> comparer)
		{
			ValidateIndicesRange(from, to, list.Count);
			if (to <= from) // an empty range
			{
				return Tuple.Create(to, to);
			}
			var compareResult = comparer(list[from], value);
			if (compareResult > 0)
			{
				// the first (the smalest) value is greater than the target
				return Tuple.Create(from, from);
			}

			var upperBoundFrom = from + 1;
			var upperBoundTo = to;
			if (compareResult == 0)
			{
				// the first (the smalest) value is equal to the target
				return Tuple.Create(from, UpperBoundCore(list, value, upperBoundFrom, upperBoundTo, comparer));
			}

			// The following invariant has been verified and will be maintained in the loop:
			// 1) the range [from, to) is not empty
			// 2) list[from] < value
			// 3) Either "to" = initial value of "to" or list[to] >= value
			// 4) Either upperBoundTo == initial "to" or list[upperBoundLimit] > value
			// 5) Either upperBoundFrom == upperBoundTo or (upperBoundFrom < upperBoundTo and list[upperBoundFrom - 1] <= value)
			for (;;)
			{
				var median = (to + from) / 2;
				if (median == from)
				{
					return Tuple.Create(to, UpperBoundCore(list, value, upperBoundFrom, upperBoundTo, comparer));
				}
				compareResult = comparer(list[median], value);
				if (compareResult < 0)
				{
					from = median;
					upperBoundFrom = median + 1;
				}
				else if (compareResult == 0)
				{
					to = median;
					upperBoundFrom = median + 1;
				}
				else
				{
					to = median;
					upperBoundTo = median;
				}
			}
		}
	}
}
