using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace CodeJam.Collections
{
	partial class EnumerableExtensions
	{
		/// <summary>
		/// Performs topological sort on <paramref name="source"/>.
		/// </summary>
		[NotNull]
		[Pure]
		public static List<T> TopoSort<T>(
			[NotNull] this IEnumerable<T> source,
			[NotNull, InstantHandle] Func<T, IEnumerable<T>> dependsOnGetter) =>
				TopoSort(source, dependsOnGetter, EqualityComparer<T>.Default);

		/// <summary>
		/// Performs topological sort on <paramref name="source"/>.
		/// </summary>
		[NotNull]
		[Pure]
		public static List<T> TopoSort<T>(
			[NotNull] this IEnumerable<T> source,
			[NotNull, InstantHandle] Func<T, IEnumerable<T>> dependsOnGetter,
			[NotNull] IEqualityComparer<T> equalityComparer)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (dependsOnGetter == null) throw new ArgumentNullException(nameof(dependsOnGetter));
			if (equalityComparer == null) throw new ArgumentNullException(nameof(equalityComparer));

			var result = new List<T>();
			var visited = new HashSet<T>(equalityComparer);
			foreach (var item in source)
				SortLevel(item, dependsOnGetter, visited, new HashSet<T>(equalityComparer), result);

			return result;
		}

		private static void SortLevel<T>(
			T item,
			Func<T, IEnumerable<T>> dependsOnGetter,
			HashSet<T> visited,
			HashSet<T> cycleDetector,
			List<T> result)
		{
			// TODO: replace recursive algorythm by linear
			if (cycleDetector.Contains(item))
				throw new ArgumentException("There is a cycle in supplied source.");
			cycleDetector.Add(item);
			if (visited.Contains(item))
				return;
			foreach (var depItem in dependsOnGetter(item).Where(dt => !visited.Contains(dt)))
				SortLevel(depItem, dependsOnGetter, visited, cycleDetector, result);

			visited.Add(item);
			result.Add(item);
		}
	}
}