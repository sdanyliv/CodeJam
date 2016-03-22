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
		[Pure]
		public static bool EqualsTo<T>([CanBeNull] this T[] a, [CanBeNull] T[] b, [NotNull] IEqualityComparer<T> comparer)
		{
			if (comparer == null) throw new ArgumentNullException(nameof(comparer));

			if (a == null && b == null)
				return true;

			if (a == null || b == null)
				return false;

			if (a.Length != b.Length)
				return false;

			for (var i = 0; i < a.Length; i++)
				if (!comparer.Equals(a[i], b[i]))
					return false;

			return true;
		}

		/// <summary>
		/// Returns true, if length and content of <paramref name="a"/> equals <paramref name="b"/>.
		/// </summary>
		[Pure]
		public static bool EqualsTo<T>([CanBeNull] this T[] a, [CanBeNull] T[] b) => EqualsTo(a, b, EqualityComparer<T>.Default);
	}
}
