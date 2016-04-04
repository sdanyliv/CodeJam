﻿using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace CodeJam.Collections
{
	/// <summary>
	/// Various collections extensions.
	/// </summary>
	[PublicAPI]
	public static class CollectionExtensions
	{
		/// <summary>
		/// Indicates whether the specified collection is <c>null</c> or empty.
		/// </summary>
		/// <param name="collection">The collection to test for emptiness.</param>
		/// <returns>
		/// <c>true</c>, if the <paramref name="collection"/> parameter is <c>null</c>
		/// or empty; otherwise, <c>false</c>.
		/// </returns>
		[Pure]
		[ContractAnnotation("collection:null => true")]
		public static bool IsNullOrEmpty<T>(this ICollection<T> collection) =>
			collection == null || collection.Count == 0;

		/// <summary>
		/// Indicates whether the specified array is <c>null</c> or empty.
		/// </summary>
		/// <param name="array">The collection to test for emptiness.</param>
		/// <returns>
		/// <c>true</c>, if the <paramref name="array"/> parameter is <c>null</c>
		/// or empty; otherwise, <c>false</c>.
		/// </returns>
		[Pure]
		[ContractAnnotation("array:null => true")]
		public static bool IsNullOrEmpty<T>(this T[] array) =>
			array == null || array.Length == 0;

		/// <summary>
		/// Indicates whether the specified collection is not null nor empty.
		/// </summary>
		/// <param name="collection">The collection to test.</param>
		/// <returns>
		/// <c>true</c>, if the <paramref name="collection"/> parameter is not null nor empty; otherwise, <c>false</c>.
		/// </returns>
		[Pure]
		[ContractAnnotation("collection:null => false")]
		public static bool NotNullNorEmpty<T>(this ICollection<T> collection) =>
			collection != null && collection.Count != 0;

		/// <summary>
		/// Indicates whether the specified array is is not null nor empty.
		/// </summary>
		/// <param name="array">The collection to test.</param>
		/// <returns>
		/// <c>true</c>, if the <paramref name="array"/> parameter is not null nor empty; otherwise, <c>false</c>.
		/// </returns>
		[Pure]
		[ContractAnnotation("array:null => false")]
		public static bool NotNullNorEmpty<T>(this T[] array) =>
			array != null && array.Length != 0;

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
