﻿using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace CodeJam.Collections
{
	partial class EnumerableExtensions
	{
		#region MinBy

		#region byte
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, byte> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, byte> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

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
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, byte?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				byte? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, byte?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				byte? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
		#endregion

		#region sbyte
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, sbyte> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, sbyte> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

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
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, sbyte?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				sbyte? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, sbyte?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				sbyte? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
		#endregion

		#region short
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, short> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, short> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

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
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, short?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				short? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, short?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				short? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
		#endregion

		#region ushort
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, ushort> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, ushort> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

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
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, ushort?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				ushort? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, ushort?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				ushort? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
		#endregion

		#region int
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, int> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, int> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

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
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, int?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				int? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, int?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				int? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
		#endregion

		#region uint
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, uint> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, uint> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

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
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, uint?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				uint? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, uint?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				uint? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
		#endregion

		#region long
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, long> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, long> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

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
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, long?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				long? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, long?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				long? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
		#endregion

		#region ulong
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, ulong> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, ulong> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

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
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, ulong?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				ulong? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, ulong?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				ulong? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
		#endregion

		#region float
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, float> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, float> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

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
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, float?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				float? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, float?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				float? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
		#endregion

		#region double
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, double> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, double> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

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
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, double?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				double? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, double?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				double? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
		#endregion

		#region decimal
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with minimum value in the sequence.</returns>
		[Pure]
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, decimal> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, decimal> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

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
		public static TSource MinBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, decimal?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				decimal? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with minimum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MinByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, decimal?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				decimal? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
		#endregion

		#endregion

		#region MaxBy

		#region byte
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, byte> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
					if (x > value)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, byte> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

				var value = selector(e.Current);
				item = e.Current;
				while (e.MoveNext())
				{
					var x = selector(e.Current);
					if (x > value)
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
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, byte?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				byte? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
					if (cur.HasValue & x > valueVal)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, byte?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				byte? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
					if (cur.HasValue & x > valueVal)
					{
						valueVal = x;
						item = e.Current;
					}
				}
			}

			return item;
		}
		#endregion

		#region sbyte
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, sbyte> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
					if (x > value)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, sbyte> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

				var value = selector(e.Current);
				item = e.Current;
				while (e.MoveNext())
				{
					var x = selector(e.Current);
					if (x > value)
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
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, sbyte?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				sbyte? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
					if (cur.HasValue & x > valueVal)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, sbyte?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				sbyte? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
					if (cur.HasValue & x > valueVal)
					{
						valueVal = x;
						item = e.Current;
					}
				}
			}

			return item;
		}
		#endregion

		#region short
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, short> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
					if (x > value)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, short> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

				var value = selector(e.Current);
				item = e.Current;
				while (e.MoveNext())
				{
					var x = selector(e.Current);
					if (x > value)
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
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, short?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				short? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
					if (cur.HasValue & x > valueVal)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, short?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				short? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
					if (cur.HasValue & x > valueVal)
					{
						valueVal = x;
						item = e.Current;
					}
				}
			}

			return item;
		}
		#endregion

		#region ushort
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, ushort> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
					if (x > value)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, ushort> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

				var value = selector(e.Current);
				item = e.Current;
				while (e.MoveNext())
				{
					var x = selector(e.Current);
					if (x > value)
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
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, ushort?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				ushort? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
					if (cur.HasValue & x > valueVal)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, ushort?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				ushort? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
					if (cur.HasValue & x > valueVal)
					{
						valueVal = x;
						item = e.Current;
					}
				}
			}

			return item;
		}
		#endregion

		#region int
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, int> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
					if (x > value)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, int> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

				var value = selector(e.Current);
				item = e.Current;
				while (e.MoveNext())
				{
					var x = selector(e.Current);
					if (x > value)
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
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, int?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				int? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
					if (cur.HasValue & x > valueVal)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, int?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				int? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
					if (cur.HasValue & x > valueVal)
					{
						valueVal = x;
						item = e.Current;
					}
				}
			}

			return item;
		}
		#endregion

		#region uint
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, uint> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
					if (x > value)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, uint> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

				var value = selector(e.Current);
				item = e.Current;
				while (e.MoveNext())
				{
					var x = selector(e.Current);
					if (x > value)
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
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, uint?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				uint? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
					if (cur.HasValue & x > valueVal)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, uint?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				uint? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
					if (cur.HasValue & x > valueVal)
					{
						valueVal = x;
						item = e.Current;
					}
				}
			}

			return item;
		}
		#endregion

		#region long
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, long> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
					if (x > value)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, long> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

				var value = selector(e.Current);
				item = e.Current;
				while (e.MoveNext())
				{
					var x = selector(e.Current);
					if (x > value)
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
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, long?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				long? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
					if (cur.HasValue & x > valueVal)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, long?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				long? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
					if (cur.HasValue & x > valueVal)
					{
						valueVal = x;
						item = e.Current;
					}
				}
			}

			return item;
		}
		#endregion

		#region ulong
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, ulong> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
					if (x > value)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, ulong> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

				var value = selector(e.Current);
				item = e.Current;
				while (e.MoveNext())
				{
					var x = selector(e.Current);
					if (x > value)
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
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, ulong?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				ulong? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
					if (cur.HasValue & x > valueVal)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, ulong?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				ulong? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
					if (cur.HasValue & x > valueVal)
					{
						valueVal = x;
						item = e.Current;
					}
				}
			}

			return item;
		}
		#endregion

		#region float
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, float> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
					if (x > value)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, float> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

				var value = selector(e.Current);
				item = e.Current;
				while (e.MoveNext())
				{
					var x = selector(e.Current);
					if (x > value)
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
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, float?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				float? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
					if (cur.HasValue & x > valueVal)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, float?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				float? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
					if (cur.HasValue & x > valueVal)
					{
						valueVal = x;
						item = e.Current;
					}
				}
			}

			return item;
		}
		#endregion

		#region double
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, double> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
					if (x > value)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, double> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

				var value = selector(e.Current);
				item = e.Current;
				while (e.MoveNext())
				{
					var x = selector(e.Current);
					if (x > value)
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
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, double?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				double? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
					if (cur.HasValue & x > valueVal)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, double?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				double? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
					if (cur.HasValue & x > valueVal)
					{
						valueVal = x;
						item = e.Current;
					}
				}
			}

			return item;
		}
		#endregion

		#region decimal
		/// <summary>
		/// Invokes a <paramref name="selector"/> on each element of a <paramref name="source"/>
		/// and returns the item with minimum value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, decimal> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

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
					if (x > value)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, decimal> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					return defaultValue;

				var value = selector(e.Current);
				item = e.Current;
				while (e.MoveNext())
				{
					var x = selector(e.Current);
					if (x > value)
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
		/// <returns>The item with maximum value in the sequence.</returns>
		[Pure]
		public static TSource MaxBy<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, decimal?> selector)
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				decimal? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						throw NoElementsException();

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
					if (cur.HasValue & x > valueVal)
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
		/// <param name="defaultValue">Value returned if collection contains no not null elements.</param>
		/// <returns>
		/// The item with maximum value in the sequence or <typeparamref name="TSource"/> default value if
		/// <paramref name="source"/> has no not null elements.
		/// </returns>
		[Pure]
		public static TSource MaxByOrDefault<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull, InstantHandle] Func<TSource, decimal?> selector,
			TSource defaultValue = default (TSource))
		{
			Code.NotNull(source, nameof (source));
			Code.NotNull(selector, nameof(selector));

			TSource item;
			using (var e = source.GetEnumerator())
			{
				decimal? value;
				// Start off knowing that we've a non-null value (or exit here, knowing we don't)
				// so we don't have to keep testing for nullity.
				do
				{
					if (!e.MoveNext())
						return defaultValue;

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
					if (cur.HasValue & x > valueVal)
					{
						valueVal = x;
						item = e.Current;
					}
				}
			}

			return item;
		}
		#endregion

		#endregion

	}
}