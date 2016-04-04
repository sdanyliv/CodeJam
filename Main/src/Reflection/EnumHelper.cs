using System;

using JetBrains.Annotations;

namespace CodeJam.Reflection
{
	/// <summary>
	/// Helper methods for enumeration.
	/// </summary>
	[PublicAPI]
	public static class EnumHelper
	{
		/// <summary>
		/// Retrieves an array of the names of the constants in a specified enumeration.
		/// </summary>
		/// <typeparam name="T">An enumeration type.</typeparam>
		/// <returns>A string array of the names of the constants in enumType.</returns>
		[NotNull]
		[Pure]
		public static string[] GetNames<T>() => Enum.GetNames(typeof(T));

		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value.
		/// </summary>
		/// <param name="value">The value of a particular enumerated constant in terms of its underlying type.</param>
		/// <typeparam name="T">An enumeration type.</typeparam>
		/// <returns>
		/// A string containing the name of the enumerated constant in enumType whose value is value;
		/// or null if no such constant is found.
		/// </returns>
		[CanBeNull]
		[Pure]
		public static string GetName<T>(T value) => Enum.GetName(typeof(T), value);

		/// <summary>
		/// Retrieves an array of the values of the constants in a specified enumeration.
		/// </summary>
		/// <typeparam name="T">An enumeration type.</typeparam>
		/// <returns>An array that contains the values of the constants in enumType.</returns>
		[NotNull]
		[Pure]
		public static T[] GetValues<T>() => (T[])Enum.GetValues(typeof (T));

		/// <summary>
		/// Returns an indication whether a constant with a specified value exists in a specified enumeration.
		/// </summary>
		/// <param name="value">The value or name of a constant in <typeparamref name="T"/>.</param>
		/// <typeparam name="T">An enumeration type.</typeparam>
		/// <returns>true if a constant in enumType has a value equal to value; otherwise, false.</returns>
		[Pure]
		public static bool IsDefined<T>(T value) => Enum.IsDefined(typeof (T), value);

		/// <summary>
		/// Converts the string representation of the name or numeric <paramref name="value"/> of one or more
		/// enumerated constants to an equivalent enumerated object. A parameter <paramref name="ignoreCase"/> specifies
		/// whether the operation is case-insensitive.
		/// </summary>
		/// <param name="value">A string containing the name or value to convert.</param>
		/// <param name="ignoreCase">true to ignore case; false to regard case.</param>
		/// <typeparam name="T">An enumeration type.</typeparam>
		/// <returns>An object of type enumType whose value is represented by value.</returns>
		[Pure]
		public static T Parse<T>([NotNull] string value, bool ignoreCase = true) =>
			(T)Enum.Parse(typeof (T), value, ignoreCase);
	}
}