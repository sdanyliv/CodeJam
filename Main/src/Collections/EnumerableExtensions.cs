using System;
using System.Collections.Generic;

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
		/// Prepends specified <paramref name="element"/> to the collection start.
		/// </summary>
		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T element)
		{
			yield return element;
			foreach (var item in source)
				yield return item;
		}

		/// <summary>
		/// Creates a <see cref="HashSet{T}"/> from an <see cref="IEnumerable{T}"/>.
		/// </summary>
		public static HashSet<T> ToHashSet<T>([NotNull] this IEnumerable<T> source)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			return new HashSet<T>(source);
		}
	}
}