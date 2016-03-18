using System;
using System.Collections.Generic;
using System.Globalization;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// <see cref="string"/> class extensions.
	/// </summary>
	[PublicAPI]
	public static class StringExtensions
	{
		/// <summary>
		/// Infix form of <see cref="string.IsNullOrEmpty"/>.
		/// </summary>
		public static bool IsNullOrEmpty([CanBeNull] this string str) => string.IsNullOrEmpty(str);

		/// <summary>
		/// Returns true if argument is not null nor empty.
		/// </summary>
		public static bool NotNullNorEmpty([CanBeNull] this string str) => !string.IsNullOrEmpty(str);

		/// <summary>
		/// Infix form of <see cref="string.IsNullOrWhiteSpace"/>.
		/// </summary>
		public static bool IsNullOrWhiteSpace([CanBeNull] this string str) => string.IsNullOrWhiteSpace(str);

		/// <summary>
		/// Returns true if argument is not null nor whitespace.
		/// </summary>
		public static bool NotNullNorWhiteSpace([CanBeNull] this string str) => !string.IsNullOrWhiteSpace(str);

		/// <summary>
		/// Replaces the format items in a specified string with the string representations 
		///  of corresponding objects in a specified array.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <returns>A copy of format in which the format items have been replaced by the string representation of the corresponding objects in args</returns>
		[StringFormatMethod("format")]
		[NotNull]
		//TODO: The nethod name looks ugly. May be better to name it Expand or ExpandArgs or Apply or Substitute or Evaluate
		//NOTTODO: IT: The name is the best ever. Don't change it.
		public static string Args([NotNull] this string format, params object[] args) => string.Format(format, args);

		/// <summary>
		/// Infix form of <see cref="string.Join(string,IEnumerable{string})"/>.
		/// </summary>
		public static string Join(this IEnumerable<string> values, string separator) => string.Join(separator, values);

		/// <summary>
		/// Returns length of argument, even if argument is null.
		/// </summary>
		public static int Length([CanBeNull] this string str) => str?.Length ?? 0;

		/// <summary>
		/// Culture invariant version of <see cref="IFormattable.ToString(string,System.IFormatProvider)"/>
		/// </summary>
		public static string ToInvariantString<T>(this T s) where T : IFormattable =>
			s.ToString(null, CultureInfo.InvariantCulture);

		/// <summary>
		/// Culture invariant version of <see cref="IFormattable.ToString(string,System.IFormatProvider)"/>
		/// </summary>
		public static string ToInvariantString<T>(this T s, string format) where T : IFormattable =>
			s.ToString(format, CultureInfo.InvariantCulture);
	}
}