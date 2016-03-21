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
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource Min<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, int> selector)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (selector == null) throw new ArgumentNullException(nameof(selector));
			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					throw NoElementsException();

				var value = selector(e.Current);
				item = e.Current;
				while (e.MoveNext())
				{
					var x = selector(e.Current);
					if (x < value)
					{
						value = x;
						item = e.Current;
					}
				}
			}

			return item;
		}

		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource Min<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, int?> selector)
			where TSource : class
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (selector == null) throw new ArgumentNullException(nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				int? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return default(TSource);

					value = selector(e.Current);
				}
				while (!value.HasValue);

				// Keep hold of the wrapped value, and do comparisons on that, rather than
				// using the lifted operation each time.
				var valueVal = value.GetValueOrDefault();
				item = e.Current;
				while (e.MoveNext())
				{
					var cur = selector(e.Current);
					var x = cur.GetValueOrDefault();

					// Do not replace & with &&. The branch prediction cost outweighs the extra operation
					// unless nulls either never happen or always happen.
					if (cur.HasValue & x < valueVal)
					{
						valueVal = x;
						item = e.Current;
					}
				}
			}

			return item;
		}

		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource Min<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, long> selector)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (selector == null) throw new ArgumentNullException(nameof(selector));
			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					throw NoElementsException();

				var value = selector(e.Current);
				item = e.Current;
				while (e.MoveNext())
				{
					var x = selector(e.Current);
					if (x < value)
					{
						value = x;
						item = e.Current;
					}
				}
			}

			return item;
		}

		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource Min<TSource>(
				[NotNull] this IEnumerable<TSource> source,
				[NotNull, InstantHandle] Func<TSource, long?> selector)
			where TSource : class
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (selector == null) throw new ArgumentNullException(nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				long? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return default(TSource);

					value = selector(e.Current);
				}
				while (!value.HasValue);

				// Keep hold of the wrapped value, and do comparisons on that, rather than
				// using the lifted operation each time.
				var valueVal = value.GetValueOrDefault();
				item = e.Current;
				while (e.MoveNext())
				{
					var cur = selector(e.Current);
					var x = cur.GetValueOrDefault();

					// Do not replace & with &&. The branch prediction cost outweighs the extra operation
					// unless nulls either never happen or always happen.
					if (cur.HasValue & x < valueVal)
					{
						valueVal = x;
						item = e.Current;
					}
				}
			}

			return item;
		}

		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource Min<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, float> selector)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (selector == null) throw new ArgumentNullException(nameof(selector));
			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					throw NoElementsException();

				var value = selector(e.Current);
				item = e.Current;
				while (e.MoveNext())
				{
					var x = selector(e.Current);
					if (x < value)
					{
						value = x;
						item = e.Current;
					}
				}
			}

			return item;
		}

		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource Min<TSource>(
				[NotNull] this IEnumerable<TSource> source,
				[NotNull, InstantHandle] Func<TSource, float?> selector)
			where TSource : class
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (selector == null) throw new ArgumentNullException(nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				float? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return default(TSource);

					value = selector(e.Current);
				}
				while (!value.HasValue);

				// Keep hold of the wrapped value, and do comparisons on that, rather than
				// using the lifted operation each time.
				var valueVal = value.GetValueOrDefault();
				item = e.Current;
				while (e.MoveNext())
				{
					var cur = selector(e.Current);
					var x = cur.GetValueOrDefault();

					// Do not replace & with &&. The branch prediction cost outweighs the extra operation
					// unless nulls either never happen or always happen.
					if (cur.HasValue & x < valueVal)
					{
						valueVal = x;
						item = e.Current;
					}
				}
			}

			return item;
		}

		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource Min<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, double> selector)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (selector == null) throw new ArgumentNullException(nameof(selector));
			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					throw NoElementsException();

				var value = selector(e.Current);
				item = e.Current;
				while (e.MoveNext())
				{
					var x = selector(e.Current);
					if (x < value)
					{
						value = x;
						item = e.Current;
					}
				}
			}

			return item;
		}

		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource Min<TSource>(
				[NotNull] this IEnumerable<TSource> source,
				[NotNull, InstantHandle] Func<TSource, double?> selector)
			where TSource : class
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (selector == null) throw new ArgumentNullException(nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				double? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return default(TSource);

					value = selector(e.Current);
				}
				while (!value.HasValue);

				// Keep hold of the wrapped value, and do comparisons on that, rather than
				// using the lifted operation each time.
				var valueVal = value.GetValueOrDefault();
				item = e.Current;
				while (e.MoveNext())
				{
					var cur = selector(e.Current);
					var x = cur.GetValueOrDefault();

					// Do not replace & with &&. The branch prediction cost outweighs the extra operation
					// unless nulls either never happen or always happen.
					if (cur.HasValue & x < valueVal)
					{
						valueVal = x;
						item = e.Current;
					}
				}
			}

			return item;
		}

		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource Min<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, decimal> selector)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (selector == null) throw new ArgumentNullException(nameof(selector));
			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					throw NoElementsException();

				var value = selector(e.Current);
				item = e.Current;
				while (e.MoveNext())
				{
					var x = selector(e.Current);
					if (x < value)
					{
						value = x;
						item = e.Current;
					}
				}
			}

			return item;
		}

		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource Min<TSource>(
				[NotNull] this IEnumerable<TSource> source,
				[NotNull, InstantHandle] Func<TSource, decimal?> selector)
			where TSource : class
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (selector == null) throw new ArgumentNullException(nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				decimal? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return default(TSource);

					value = selector(e.Current);
				}
				while (!value.HasValue);

				// Keep hold of the wrapped value, and do comparisons on that, rather than
				// using the lifted operation each time.
				var valueVal = value.GetValueOrDefault();
				item = e.Current;
				while (e.MoveNext())
				{
					var cur = selector(e.Current);
					var x = cur.GetValueOrDefault();

					// Do not replace & with &&. The branch prediction cost outweighs the extra operation
					// unless nulls either never happen or always happen.
					if (cur.HasValue & x < valueVal)
					{
						valueVal = x;
						item = e.Current;
					}
				}
			}

			return item;
		}

		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <typeparam name="TValue">Type of the value</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
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
	}
}