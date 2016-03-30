using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace CodeJam.Collections
{
	partial class EnumerableExtensions
	{
		/// <summary>
		/// Executes the given action on each element in the source sequence and yields it.
		/// </summary>
		/// <remarks>
		/// The returned sequence is essentially a duplicate of the original,
		/// but with the extra action being executed while the sequence is evaluated.
		/// The action is always taken before the element is yielded,
		/// so any changes made by the action will be visible in the returned sequence.
		/// This operator uses deferred execution and streams it results.
		/// </remarks>
		/// <typeparam name="T">The type of the elements in the sequence</typeparam>
		/// <param name="source">The sequence of elements</param>
		/// <param name="action">The action to execute on each element</param>
		/// <returns>
		/// A sequence with source elements in their original order.
		/// </returns>
		[NotNull, Pure]
		public static IEnumerable<T> Pipe<T>(this IEnumerable<T> source, Action<T> action)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(action, nameof (action));
			return PipeImpl(source, action);
		}

		private static IEnumerable<T> PipeImpl<T>(IEnumerable<T> source, Action<T> action)
		{
			foreach (var element in source)
			{
				action(element);
				yield return element;
			}
		}
	}
}
