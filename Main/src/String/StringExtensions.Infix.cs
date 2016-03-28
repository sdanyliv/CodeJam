using System;
using System.Collections.Generic;
using System.Globalization;

using JetBrains.Annotations;

namespace CodeJam
{
	static partial class StringExtensions
	{
		/// <summary>
		/// Infix form of <see cref="string.IsNullOrEmpty"/>.
		/// </summary>
		[Pure]
		public static bool IsNullOrEmpty([CanBeNull] this string str) => string.IsNullOrEmpty(str);

		/// <summary>
		/// Returns true if argument is not null nor empty.
		/// </summary>
		[Pure]
		public static bool NotNullNorEmpty([CanBeNull] this string str) => !string.IsNullOrEmpty(str);

		/// <summary>
		/// Infix form of <see cref="string.IsNullOrWhiteSpace"/>.
		/// </summary>
		[Pure]
		public static bool IsNullOrWhiteSpace([CanBeNull] this string str) => string.IsNullOrWhiteSpace(str);

		/// <summary>
		/// Returns true if argument is not null nor whitespace.
		/// </summary>
		[Pure]
		public static bool NotNullNorWhiteSpace([CanBeNull] this string str) => !string.IsNullOrWhiteSpace(str);

		/// <summary>
		/// Replaces the format items in a specified string with the string representations 
		/// of corresponding objects in a specified array.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <returns>A copy of format in which the format items have been replaced by the string representation of the corresponding objects in args</returns>
		[StringFormatMethod("format")]
		[NotNull, Pure]
		public static string FormatWith([NotNull] this string format, params object[] args) => string.Format(format, args);

		/// <summary>
		/// Concatenates all the elements of a string array, using the specified separator between each element. 
		/// </summary>
		/// <remarks>
		/// Infix form of <see cref="string.Join(string,string[])"/>.
		/// </remarks>
		/// <param name="values">An array that contains the elements to concatenate.</param>
		/// <param name="separator">The string to use as a separator. <paramref name="separator"/> is included in the returned string only
		/// if <paramref name="values"/> has more than one element.</param>
		/// <returns>
		/// A string that consists of the members of <paramref name="values"/> delimited by the <paramref name="separator"/> string.
		/// If <paramref name="values"/> has no members, the method returns <see cref="string.Empty"/>.
		/// </returns>
		[NotNull, Pure]
		public static string Join([NotNull] this string[] values, [CanBeNull] string separator) => string.Join(separator, values);

		/// <summary>
		/// Concatenates the members of a constructed <see cref="IEnumerable{T}"/> collection of type <see cref="string"/>,
		/// using the specified separator between each member.
		/// </summary>
		/// <remarks>
		/// Infix form of <see cref="string.Join(string,IEnumerable{string})"/>.
		/// </remarks>
		/// <param name="values">A collection that contains the strings to concatenate.</param>
		/// <param name="separator">The string to use as a separator. <paramref name="separator"/> is included in the returned string only
		/// if <paramref name="values"/> has more than one element.</param>
		/// <returns>
		/// A string that consists of the members of <paramref name="values"/> delimited by the <paramref name="separator"/> string.
		/// If <paramref name="values"/> has no members, the method returns <see cref="string.Empty"/>.
		/// </returns>
		[NotNull, Pure]
		public static string Join([NotNull, InstantHandle] this IEnumerable<string> values, [CanBeNull] string separator) =>
			string.Join(separator, values);

		/// <summary>
		/// Concatenates the members of a collection, using the specified separator between each member.
		/// </summary>
		/// <remarks>
		/// Infix form of <see cref="string.Join{T}(string, IEnumerable{T})"/>.
		/// </remarks>
		/// <param name="values">A collection that contains the strings to concatenate.</param>
		/// <param name="separator">The string to use as a separator. <paramref name="separator"/> is included in the returned string only
		/// if <paramref name="values"/> has more than one element.</param>
		/// <returns>
		/// A string that consists of the members of <paramref name="values"/> delimited by the <paramref name="separator"/> string.
		/// If <paramref name="values"/> has no members, the method returns <see cref="string.Empty"/>.
		/// </returns>
		[NotNull, Pure]
		public static string Join<T>([NotNull, InstantHandle] this IEnumerable<T> values, [CanBeNull] string separator) =>
			string.Join(separator, values);

		/// <summary>
		/// Concatenates the members of a collection.
		/// </summary>
		/// <param name="values">A collection that contains the strings to concatenate.</param>
		/// <returns>
		/// A string that consists of the members of <paramref name="values"/>.
		/// If <paramref name="values"/> has no members, the method returns <see cref="string.Empty"/>.
		/// </returns>
		[NotNull, Pure]
		public static string Join<T>([NotNull, InstantHandle] this IEnumerable<T> values) =>
			string.Join("", values);

		/// <summary>
		/// Returns length of argument, even if argument is null.
		/// </summary>
		[Pure]
		public static int Length([CanBeNull] this string str) => str?.Length ?? 0;

		/// <summary>
		/// Culture invariant version of <see cref="IFormattable.ToString(string,System.IFormatProvider)"/>
		/// </summary>
		[NotNull, Pure]
		public static string ToInvariantString<T>([NotNull] this T s) where T : IFormattable =>
			s.ToString(null, CultureInfo.InvariantCulture);

		/// <summary>
		/// Culture invariant version of <see cref="IFormattable.ToString(string,System.IFormatProvider)"/>
		/// </summary>
		[NotNull, Pure]
		public static string ToInvariantString<T>([NotNull] this T s, string format) where T : IFormattable =>
			s.ToString(format, CultureInfo.InvariantCulture);
	}
}
