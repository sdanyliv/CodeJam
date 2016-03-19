using System;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// <see cref="string"/> class extensions.
	/// </summary>
	[PublicAPI]
	public partial class StringExtensions
	{
		/// <summary>
		/// Retrieves a substring from <paramref name="str"/>.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="origin">
		/// Specifies the beginning, or the end as a reference point for offset, using a value of type
		/// <see cref="StringOrigin"/>.
		/// </param>
		/// <param name="length">The number of characters in the substring.</param>
		/// <returns>
		/// A string that is equivalent to the substring of length <paramref name="length"/> that begins at
		/// <paramref name="origin"/> in  <paramref name="str"/>, or Empty if length of <paramref name="str"/>
		/// or <paramref name="length"/> is zero.
		/// </returns>
		public static string Substring([NotNull] this string str, StringOrigin origin, int length)
		{
			if (str == null) throw new ArgumentNullException(nameof(str));

			// Fast path
			var strLen = str.Length;
			if (strLen == 0 || length == 0)
				return "";
			if (length >= strLen)
				return str;
			switch (origin)
			{
				case StringOrigin.Begin:
					return str.Substring(0, length);
				case StringOrigin.End:
					return str.Substring(strLen - length, length);
				default:
					throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
			}
		}

		/// <summary>
		/// Retireves prefix of length <paramref name="length"/>.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="length">The number of characters in the substring.</param>
		public static string Prefix([NotNull] this string str, int length) => str.Substring(StringOrigin.Begin, length);

		/// <summary>
		/// Retireves prefix of length <paramref name="length"/>.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="length">The number of characters in the substring.</param>
		public static string Suffix([NotNull] this string str, int length) => str.Substring(StringOrigin.End, length);
	}
}