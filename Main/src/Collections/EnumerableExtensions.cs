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
		public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T element)
		{
			foreach (var item in source)
				yield return item;
			yield return element;
		}

		/// <summary>
		/// Appends specified <paramref name="elements"/> to end of the collection.
		/// </summary>
		public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, params T[] elements)
		{
			foreach (var item in source)
				yield return item;
			foreach (var element in elements)
				yield return element;
		}

		/// <summary>
		/// Prepends specified <paramref name="element"/> to the collection start.
		/// </summary>
		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T element)
		{
			yield return element;
			foreach (var item in source)
				yield return item;
		}

		/// <summary>
		/// Prepends specified <paramref name="elements"/> to the collection start.
		/// </summary>
		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, params T[] elements)
		{
			foreach (var element in elements)
				yield return element;
			foreach (var item in source)
				yield return item;
		}

		/// <summary>
		/// Creates a <see cref="HashSet{T}"/> from an <see cref="IEnumerable{T}"/>.
		/// </summary>
		[NotNull]
		public static HashSet<T> ToHashSet<T>([NotNull] this IEnumerable<T> source) => new HashSet<T>(source);

		/// <summary>
		/// Creates a <see cref="HashSet{T}"/> from an <see cref="IEnumerable{T}"/>.
		/// </summary>
		[NotNull]
		public static HashSet<T> ToHashSet<T>(
			[NotNull] this IEnumerable<T> source,
			[NotNull] IEqualityComparer<T> comparer) =>
				new HashSet<T>(source, comparer);

		/// <summary>
		/// Returns first element, or specified <paramref name="defaultValue"/>, if sequence is empty.
		/// </summary>
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