using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace CodeJam.Collections
{
	/// <summary>
	/// <see cref="Array" /> class extensions.
	/// </summary>
	[PublicAPI]
	public partial class ArrayExtensions
	{
		/// <summary>
		/// Returns true, if length and content of <paramref name="a"/> equals <paramref name="b"/>.
		/// </summary>
		/// <param name="a">The first array to compare.</param>
		/// <param name="b">The second array to compare.</param>
		/// <param name="comparer">Instance of <see cref="IComparer{T}"/> to compare values.</param>
		[Pure]
		public static bool EqualsTo<T>([CanBeNull] this T[] a, [CanBeNull] T[] b, [NotNull] IEqualityComparer<T> comparer)
		{
			if (comparer == null) throw new ArgumentNullException(nameof(comparer));

			if (a == b)
				return true;

			if (a == null || b == null)
				return false;

			if (a.Length != b.Length)
				return false;

			// ReSharper disable once LoopCanBeConvertedToQuery
			for (var i = 0; i < a.Length; i++)
				if (!comparer.Equals(a[i], b[i]))
					return false;

			return true;
		}

		/// <summary>
		/// Returns true, if length and content of <paramref name="a"/> equals <paramref name="b"/>.
		/// </summary>
		/// <param name="a">The first array to compare.</param>
		/// <param name="b">The second array to compare.</param>
		[Pure]
		public static bool EqualsTo<T>([CanBeNull] this T[] a, [CanBeNull] T[] b) =>
			EqualsTo(a, b, EqualityComparer<T>.Default);

		/// <summary>
		/// Checks if any element in array exists.
		/// </summary>
		/// <remarks>This method performs fast check instead of creating enumerator</remarks>
		public static bool Any<T>(this T[] array) => array.Length != 0;
	}
}
