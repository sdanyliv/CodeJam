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
		/// Returns the input typed as <typeparamref name="T"/>[].
		/// </summary>
		public static T[] AsArray<T>(this T[] source)
		{
			return source;
		}

		/// <summary>
		/// Returns the input typed as <see cref="List{T}"/>.
		/// </summary>
		public static List<T> AsList<T>(this List<T> source)
		{
			return source;
		}

		/// <summary>
		/// Returns the input typed as <see cref="HashSet{T}"/>.
		/// </summary>
		public static HashSet<T> AsHashSet<T>(this HashSet<T> source)
		{
			return source;
		}
	}
}