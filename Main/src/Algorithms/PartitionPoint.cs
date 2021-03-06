﻿using System;
using System.Collections.Generic;

namespace CodeJam
{
	partial class Algorithms
	{
		/// <summary>
		/// Returns the index i in the range [0, list.Count - 1] such that
		///		predicate(list[j]) = true for j &lt; i
		///		and predicate(list[k]) = false for k >= i
		/// or list.Count if no such i exists
		/// <remarks>The list should be partitioned according to the predicate</remarks>
		/// </summary>
		/// <typeparam name="T">The list element type</typeparam>
		/// <param name="list">The sorted list</param>
		/// <param name="predicate">The predicate</param>
		/// <returns>The partition point</returns>
		public static int PartitionPoint<T>(this IList<T> list, Predicate<T> predicate)
			=> PartitionPoint(list, 0, list.Count, predicate);

		/// <summary>
		/// Returns the index i in the range [from, list.Count - 1] such that
		///		predicate(list[j]) = true for j &lt; i
		///		and predicate(list[k]) = false for k >= i
		/// or list.Count if no such i exists
		/// <remarks>The list should be partitioned according to the predicate</remarks>
		/// </summary>
		/// <typeparam name="T">The list element type</typeparam>
		/// <param name="list">The sorted list</param>
		/// <param name="from">The minimum index</param>
		/// <param name="predicate">The predicate</param>
		/// <returns>The partition point</returns>
		public static int PartitionPoint<T>(this IList<T> list, int from, Predicate<T> predicate)
			=> PartitionPoint(list, from, list.Count, predicate);

		/// <summary>
		/// Returns the index i in the range [from, to - 1] such that
		///		predicate(list[j]) = true for j &lt; i
		///		and predicate(list[k]) = false for k >= i
		/// or "to" if no such i exists
		/// <remarks>The list should be partitioned according to the predicate</remarks>
		/// </summary>
		/// <typeparam name="T">The list element type</typeparam>
		/// <param name="list">The sorted list</param>
		/// <param name="from">The minimum index</param>
		/// <param name="to">The upper bound for the index (not included)</param>
		/// <param name="predicate">The predicate</param>
		/// <returns>The partition point</returns>
		public static int PartitionPoint<T>(this IList<T> list, int from, int to, Predicate<T> predicate)
		{
			ValidateIndicesRange(from, to, list.Count);
			while (from < to)
			{
				var median = from + (to - from) / 2;
				var testResult = predicate(list[median]);
				if (!testResult)
				{
					to = median;
				}
				else
				{
					from = median + 1;
				}
			}
			return from;
		}
	}
}
