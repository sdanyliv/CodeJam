using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace CodeJam.Collections
{
	partial class EnumerableExtensions
	{
		/// <summary>
		/// Returns a specified number of contiguous elements from the end of a sequence.
		/// </summary>
		/// <remarks>
		/// This operator uses deferred execution and streams its results.
		/// </remarks>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <param name="source">The sequence to return the last element of.</param>
		/// <param name="count">The number of elements to return.</param>
		/// <returns>
		/// An <see cref="IEnumerable{T}"/> that contains the specified number of elements from the end of the input sequence.
		/// </returns>
		[NotNull, Pure]
		public static IEnumerable<TSource> TakeLast<TSource>([NotNull] this IEnumerable<TSource> source, int count)
		{
			Code.NotNull(source, nameof (source));
			return count > 0
				? TakeLastImpl(source, count)
				: Enumerable.Empty<TSource>();
		}

		private static IEnumerable<T> TakeLastImpl<T>(IEnumerable<T> source, int count)
		{
			var queue = new Queue<T>(count);
			foreach (var item in source)
			{
				if (queue.Count == count)
					queue.Dequeue();

				queue.Enqueue(item);
			}

			foreach (var item in queue)
				yield return item;
		}
	}
}
