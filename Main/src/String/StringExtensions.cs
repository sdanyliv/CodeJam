using System;
using System.Collections.Generic;

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
		[NotNull]
		[Pure]
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
		[NotNull]
		[Pure]
		public static string Prefix([NotNull] this string str, int length) => str.Substring(StringOrigin.Begin, length);

		/// <summary>
		/// Retireves prefix of length <paramref name="length"/>.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="length">The number of characters in the substring.</param>
		[NotNull]
		[Pure]
		public static string Suffix([NotNull] this string str, int length) => str.Substring(StringOrigin.End, length);

		/// <summary>
		/// Trims <paramref name="str"/> prefix if it equals to <paramref name="prefix"/>.
		/// </summary>
		[NotNull]
		[Pure]
		public static string TrimPrefix(
			[NotNull] this string str,
			[CanBeNull] string prefix,
			[NotNull] IEqualityComparer<string> comparer)
		{
			if (str == null) throw new ArgumentNullException(nameof(str));
			if (comparer == null) throw new ArgumentNullException(nameof(comparer));

			// FastPath
			if (prefix == null)
				return str;
			var prefixLen = prefix.Length;
			if (prefixLen == 0 || str.Length < prefixLen)
				return str;

			var actPrefix = str.Prefix(prefixLen);
			return !comparer.Equals(prefix, actPrefix) ? str : str.Substring(prefixLen);
		}

		/// <summary>
		/// Trims <paramref name="str"/> prefix if it equals to <paramref name="prefix"/>.
		/// </summary>
		[NotNull]
		[Pure]
		public static string TrimPrefix([NotNull] this string str, [CanBeNull] string prefix) =>
			TrimPrefix(str, prefix, StringComparer.CurrentCulture);

		/// <summary>
		/// Trims <paramref name="str"/> suffix if it equals to <paramref name="suffix"/>.
		/// </summary>
		[NotNull]
		[Pure]
		public static string TrimSuffix(
			[NotNull] this string str,
			[CanBeNull] string suffix,
			[NotNull] IEqualityComparer<string> comparer)
		{
			if (str == null) throw new ArgumentNullException(nameof(str));
			if (comparer == null) throw new ArgumentNullException(nameof(comparer));

			// FastPath
			if (suffix == null)
				return str;
			var strLen = str.Length;
			var suffixLen = suffix.Length;
			if (suffixLen == 0 || strLen < suffixLen)
				return str;

			var actPrefix = str.Suffix(suffixLen);
			return !comparer.Equals(suffix, actPrefix) ? str : str.Substring(0, strLen - suffixLen);
		}

		/// <summary>
		/// Trims <paramref name="str"/> prefix if it equals to <paramref name="suffix"/>.
		/// </summary>
		[NotNull]
		[Pure]
		public static string TrimSuffix([NotNull] this string str, [CanBeNull] string suffix) =>
			TrimSuffix(str, suffix, StringComparer.CurrentCulture);

		private static readonly string[] _sizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB" };

		/// <summary>
		/// Returns size in bytes string representation.
		/// </summary>
		public static string ToByteSizeString(this long value)
		{
			if (value < 0) { return "-" + (-value).ToByteSizeString(); }

			if (value == 0)
				return "0";

			var i = 0;
			var dValue = (decimal)value;
			while (Math.Round(dValue / 1024) >= 1)
			{
				dValue /= 1024;
				i++;
			}

			return $"{dValue.ToString(i == 0 ? "n0" : "n1")} {_sizeSuffixes[i]}";
		}
	}
}