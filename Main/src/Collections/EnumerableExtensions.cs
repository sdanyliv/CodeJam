using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// Extensions for <see cref="IEnumerable{T}"/>
	/// </summary>
	[PublicAPI]
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Appends specified <paramref name="element"/> to end of the collection.
		/// </summary>
		[Pure, NotNull]
		public static IEnumerable<T> Concat<T>([NotNull] this IEnumerable<T> source, T element)
		{
			foreach (var item in source)
				yield return item;
			yield return element;
		}

		/// <summary>
		/// Appends specified <paramref name="elements"/> to end of the collection.
		/// </summary>
		[Pure, NotNull]
		public static IEnumerable<T> Concat<T>([NotNull] this IEnumerable<T> source, params T[] elements)
		{
			foreach (var item in source)
				yield return item;
			foreach (var element in elements)
				yield return element;
		}

		/// <summary>
		/// Prepends specified <paramref name="element"/> to the collection start.
		/// </summary>
		[Pure, NotNull]
		public static IEnumerable<T> Prepend<T>([NotNull] this IEnumerable<T> source, T element)
		{
			yield return element;
			foreach (var item in source)
				yield return item;
		}

		/// <summary>
		/// Prepends specified <paramref name="elements"/> to the collection start.
		/// </summary>
		[Pure, NotNull]
		public static IEnumerable<T> Prepend<T>([NotNull] this IEnumerable<T> source, params T[] elements)
		{
			foreach (var element in elements)
				yield return element;
			foreach (var item in source)
				yield return item;
		}

		/// <summary>
		/// Creates a <see cref="HashSet{T}"/> from an <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the elements of source.</typeparam>
		/// <param name="source">The <see cref="IEnumerable{T}"/> to create a <see cref="HashSet{T}"/> from.</param>
		/// <returns>
		/// A <see cref="HashSet{T}"/> that contains elements from the input sequence.
		/// </returns>
		[Pure, NotNull]
		public static HashSet<T> ToHashSet<T>([NotNull] this IEnumerable<T> source) => new HashSet<T>(source);

		/// <summary>
		/// Creates a <see cref="HashSet{T}"/> from an <see cref="IEnumerable{T}"/> with the specified equality comparer.
		/// </summary>
		/// <typeparam name="T">The type of the elements of source.</typeparam>
		/// <param name="source">The <see cref="IEnumerable{T}"/> to create a <see cref="HashSet{T}"/> from.</param>
		/// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use
		/// to comparing values in the set, or <c>null</c> to use tghe default implementation for the set type.</param>
		/// <returns>
		/// A <see cref="HashSet{T}"/> that contains elements from the input sequence.
		/// </returns>
		[Pure, NotNull]
		public static HashSet<T> ToHashSet<T>(
			[NotNull] this IEnumerable<T> source,
			[NotNull] IEqualityComparer<T> comparer) =>
				new HashSet<T>(source, comparer);

		/// <summary>
		/// Returns first element, or specified <paramref name="defaultValue"/>, if sequence is empty.
		/// </summary>
		[Pure]
		public static T FirstOrDefault<T>([NotNull] this IEnumerable<T> source, T defaultValue)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			foreach (var item in source)
				return item;
			return defaultValue;
		}

		/// <summary>
		/// Returns first element, or specified <paramref name="defaultValue"/>, if sequence is empty.
		/// </summary>
		[Pure]
		public static T FirstOrDefault<T>(
			[NotNull] this IEnumerable<T> source,
			T defaultValue,
			[NotNull] Func<T, bool> predicate)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));
			foreach (var item in source)
				if (predicate(item))
					return item;
			return defaultValue;
		}
	}
}