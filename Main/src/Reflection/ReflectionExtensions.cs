using System;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

namespace CodeJam.Reflection
{
	/// <summary>
	/// Reflection extension methods.
	/// </summary>
	[PublicAPI]
	public static class ReflectionExtensions
	{
		/// <summary>
		/// Returns custom attribute defined on this <paramref name="member"/>, identified by type <typeparamref name="T"/>,
		/// or null if there are no custom attribute of that type.
		/// </summary>
		/// <typeparam name="T">The type of the custom attribute.</typeparam>
		/// <param name="member">Member, on which custom attribute is looking for.</param>
		/// <param name="inherit">When true, look up the hierarchy chain for the inherited custom attribute.</param>
		/// <returns>An instance of custom attribute, or null.</returns>
		[CanBeNull]
		[Pure]
		public static T GetCustomAttribute<T>([NotNull] this ICustomAttributeProvider member, bool inherit = false)
			where T: Attribute
		{
			if (member == null) throw new ArgumentNullException(nameof(member));
			return member.GetCustomAttributes(typeof (T), inherit).Cast<T>().FirstOrDefault();
		}

		/// <summary>
		/// Returns an array of custom attributes defined on this <paramref name="member"/>,
		/// identified by type <typeparamref name="T"/>,  or empty array if there are no custom attribute of that type.
		/// </summary>
		/// <typeparam name="T">The type of the custom attributes.</typeparam>
		/// <param name="member">Member, on which custom attributes is looking for.</param>
		/// <param name="inherit">When true, look up the hierarchy chain for the inherited custom attributes.</param>
		/// <returns>An array of custom attributes, or empty array.</returns>
		[NotNull]
		[Pure]
		public static T[] GetCustomAttributes<T>([NotNull] this ICustomAttributeProvider member, bool inherit = false)
			where T : Attribute
		{
			if (member == null) throw new ArgumentNullException(nameof(member));
			return member.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
		}
	}
}