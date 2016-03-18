using System.Collections.Generic;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// Various collections extensions.
	/// </summary>
	[PublicAPI]
	public static class CollectionExtensions
	{
		/// <summary>
		/// Adds the elements to the end of the <see cref="ICollection{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the items that the collection contains.</typeparam>
		/// <param name="source">The collection to add the elements to.</param>
		/// <param name="items">The items to add to the collection.</param>
		public static void AddRange<T>([NotNull] this ICollection<T> source, [NotNull] T[] items)
		{
			foreach (var item in items)
				source.Add(item);
		}

		/// <summary>
		/// Adds the elements to the end of the <see cref="ICollection{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the items that the collection contains.</typeparam>
		/// <param name="source">The collection to add the elements to.</param>
		/// <param name="items">The items to add to the collection.</param>
		public static void AddRange<T>([NotNull] this ICollection<T> source, [NotNull] IList<T> items)
		{
			for (int i = 0, count = items.Count; i < count; i++)
				source.Add(items[i]);
		}

		/// <summary>
		/// Adds the elements to the end of the <see cref="ICollection{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the items that the collection contains.</typeparam>
		/// <param name="source">The collection to add the elements to.</param>
		/// <param name="items">The items to add to the collection.</param>
		public static void AddRange<T>([NotNull] this ICollection<T> source, [NotNull] IEnumerable<T> items)
		{
			foreach (var item in items)
				source.Add(item);
		}
	}
}
