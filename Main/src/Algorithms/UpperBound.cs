using System;
using System.Collections.Generic;

namespace CodeJam
{
	partial class Algorithms
	{
		/// <summary>
		/// Returns the minimum index i in the range [0, list.Count - 1] such that list[i] > value
		/// or list.Count if no such i exists
		/// <remarks>Comparer&lt;T&gt;.Default is being used for comparison</remarks>
		/// </summary>
		/// <typeparam name="TElement">The list element type</typeparam>
		/// <param name="list">The sorted list</param>
		/// <param name="value">The value to compare</param>
		/// <returns>The upper bound for the value</returns>
		public static int UpperBound<TElement>(this IList<TElement> list, TElement value)
			=> UpperBound(list, value, Comparer<TElement>.Default.Compare);

		/// <summary>
		/// Returns the minimum index i in the range [from, list.Count - 1] such that list[i] > value
		/// or list.Count if no such i exists
		/// <remarks>Comparer&lt;T&gt;.Default is being used for comparison</remarks>
		/// </summary>
		/// <typeparam name="TElement">The list element type</typeparam>
		/// <param name="list">The sorted list</param>
		/// <param name="value">The value to compare</param>
		/// <param name="from">The minimum index</param>
		/// <returns>The upper bound for the value</returns>
		public static int UpperBound<TElement>(this IList<TElement> list, TElement value, int from)
			=> list.UpperBound(value, @from, list.Count, Comparer<TElement>.Default.Compare);

		/// <summary>
		/// Returns the minimum index i in the range [0, list.Count - 1] such that list[i] > value
		/// or list.Count if no such i exists
		/// </summary>
		/// <typeparam name="TElement">The list element type</typeparam>
		/// <typeparam name="TValue">The type of the value</typeparam>
		/// <param name="list">The sorted list</param>
		/// <param name="value">The value to compare</param>
		/// <param name="comparer">The function with the Comparer&lt;T&gt;.Compare semantics</param>
		/// <returns>The upper bound for the value</returns>
		public static int UpperBound<TElement, TValue>(this IList<TElement> list, TValue value, Func<TElement, TValue, int> comparer)
			=> UpperBound(list, value, 0, list.Count, comparer);

		/// <summary>
		/// Returns the minimum index i in the range [from, to - 1] such that list[i] > value
		/// or "to" if no such i exists
		/// </summary>
		/// <typeparam name="TElement">The list element type</typeparam>
		/// <typeparam name="TValue">The type of the value</typeparam>
		/// <param name="list">The sorted list</param>
		/// <param name="value">The value to compare</param>
		/// <param name="from">The minimum index</param>
		/// <param name="to">The upper bound for the index (not included)</param>
		/// <param name="comparer">The function with the Comparer&lt;T&gt;.Compare semantics</param>
		/// <returns>The upper bound for the value</returns>
		public static int UpperBound<TElement, TValue>(this IList<TElement> list, TValue value, int from, int to, Func<TElement, TValue, int> comparer)
		{
			ValidateIndicesRange(from, to, list.Count);
			if (to <= from) // an empty range
			{
				return to;
			}
			if (comparer(list[from], value) > 0)
			{
				// the first (the smalest) value is greater than the target
				return from;
			}
			// The following invariant has been verified and will be maintained in the loop:
			// 1) the range [from, to) is not empty
			// 2) list[from] <= value
			// 3) Either "to" = initial value of "to" or list[to] > value
			for (;;)
			{
				var median = (to + from) / 2;
				if (median == from)
				{
					return to;
				}
				var compareResult = comparer(list[median], value);
				if (compareResult <= 0)
				{
					// cuts the range since median > from
					from = median;
				}
				else
				{
					// Keeps the loop invariant since median < to, so the range is not empty
					// and list[median] > value
					// Also, custs the range since median > from
					to = median;
				}
			}
		}
	}
}
