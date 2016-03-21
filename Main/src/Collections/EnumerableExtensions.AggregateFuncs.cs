using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace CodeJam
{
	partial class EnumerableExtensions
	{
		private static Exception NoElementsException() => new InvalidOperationException("Collection has no elements");

		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <typeparam name="TValue">Type of the value</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource Min<TSource, TValue>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, TValue> selector)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (selector == null) throw new ArgumentNullException(nameof(selector));

			var comparer = Comparer<TValue>.Default;
			var value = default(TValue);
			var item = default(TSource);
			if (value == null)
			{
				using (var e = source.GetEnumerator())
				{
					do
					{
						if (!e.MoveNext())
							return item;

						value = selector(e.Current);
						item = e.Current;
					}
					while (value == null);

					while (e.MoveNext())
					{
						var x = selector(e.Current);
						if (x != null && comparer.Compare(x, value) < 0)
						{
							value = x;
							item = e.Current;
						}
					}
				}
			}
			else
			{
				using (var e = source.GetEnumerator())
				{
					if (!e.MoveNext())
						throw NoElementsException();

					value = selector(e.Current);
					item = e.Current;
					while (e.MoveNext())
					{
						var x = selector(e.Current);
						if (comparer.Compare(x, value) < 0)
						{
							value = x;
							item = e.Current;
						}
					}
				}
			}

			return item;
		}

		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with maximum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <typeparam name="TValue">Type of the value</typeparam>
		/// <param name="source">A sequence of values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource Max<TSource, TValue>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, TValue> selector)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (selector == null) throw new ArgumentNullException(nameof(selector));

			var comparer = Comparer<TValue>.Default;
			var value = default(TValue);
			var item = default(TSource);
			if (value == null)
			{
				using (var e = source.GetEnumerator())
				{
					do
					{
						if (!e.MoveNext())
							return item;

						value = selector(e.Current);
						item = e.Current;
					}
					while (value == null);

					while (e.MoveNext())
					{
						var x = selector(e.Current);
						if (x != null && comparer.Compare(x, value) > 0)
						{
							value = x;
							item = e.Current;
						}
					}
				}
			}
			else
			{
				using (var e = source.GetEnumerator())
				{
					if (!e.MoveNext())
						throw NoElementsException();

					value = selector(e.Current);
					item = e.Current;
					while (e.MoveNext())
					{
						var x = selector(e.Current);
						if (comparer.Compare(x, value) > 0)
						{
							value = x;
							item = e.Current;
						}
					}
				}
			}

			return item;
		}
	}
}