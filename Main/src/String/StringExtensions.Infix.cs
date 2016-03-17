using System.Collections.Generic;

using JetBrains.Annotations;

namespace CodeJam
{
	[PublicAPI]
	public static partial class StringExtensions
	{
		/// <summary>
		/// Infix form of <see cref="string.IsNullOrEmpty"/>.
		/// </summary>
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		/// <summary>
		/// Returns true if argument is not null nor empty.
		/// </summary>
		public static bool NotNullNorEmpty(this string str)
		{
			return !string.IsNullOrEmpty(str);
		}

		/// <summary>
		/// Infix form of <see cref="string.IsNullOrWhiteSpace"/>.
		/// </summary>
		public static bool IsNullOrWhiteSpace(this string str)
		{
			return string.IsNullOrWhiteSpace(str);
		}

		/// <summary>
		/// Returns true if argument is not null nor whitespace.
		/// </summary>
		public static bool NotNullNorWhiteSpace(this string str)
		{
			return !string.IsNullOrWhiteSpace(str);
		}

		/// <summary>
		/// Replaces the format items in a specified string with the string representations 
		///  of corresponding objects in a specified array.
		/// A parameter supplies culture-specific formatting information.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <returns></returns>
		[StringFormatMethod("format")]
		public static string Args(this string format, params object[] args)
		{
			return string.Format(format, args);
		}

		/// <summary>
		/// Infix form of <see cref="string.Join(string,IEnumerable{string})"/>.
		/// </summary>
		public static string Join(this IEnumerable<string> values, string separator)
		{
			return string.Join(separator, values);
		}
	}
}