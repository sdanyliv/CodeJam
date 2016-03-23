using System;
using System.IO;
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

		/// <summary>
		/// Returns true if at least one attribute of type <paramref name="attrType"/> specified in <paramref name="member"/>.
		/// </summary>
		/// <param name="member">Member, on which custom attributes is looking for.</param>
		/// <param name="attrType">The type of the custom attribute.</param>
		/// <param name="inherit">When true, look up the hierarchy chain for the inherited custom attributes.</param>
		[Pure]
		public static bool HasCustomAttribute(
			[NotNull] this ICustomAttributeProvider member,
			[NotNull] Type attrType,
			bool inherit = false)
		{
			if (member == null) throw new ArgumentNullException(nameof(member));
			if (attrType == null) throw new ArgumentNullException(nameof(attrType));
			return member.GetCustomAttributes(attrType, inherit).Length > 0;
		}

		/// <summary>
		/// Returns true if at least one attribute of type <typeparamref name="T"/> specified in <paramref name="member"/>.
		/// </summary>
		/// <param name="member">Member, on which custom attributes is looking for.</param>
		/// <param name="inherit">When true, look up the hierarchy chain for the inherited custom attributes.</param>
		[Pure]
		public static bool HasCustomAttribute<T>([NotNull] this ICustomAttributeProvider member, bool inherit = false) =>
			member.HasCustomAttribute(typeof (T), inherit);

		/// <summary>
		/// Loads the specified manifest resource from this assembly, and checks if it exists.
		/// </summary>
		/// <param name="assembly">Resource assembly.</param>
		/// <param name="name">The case-sensitive name of the manifest resource being requested.</param>
		/// <returns>The manifest resource.</returns>
		/// <exception cref="ArgumentNullException">The name parameter is null.</exception>
		/// <exception cref="ArgumentException">Resource with specified name not found</exception>
		[NotNull]
		[Pure]
		public static Stream GetRequiredResourceStream([NotNull] this Assembly assembly, [NotNull] string name)
		{
			if (assembly == null) throw new ArgumentNullException(nameof(assembly));
			if (name.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(name));

			var result = assembly.GetManifestResourceStream(name);
			if (result == null)
				throw new ArgumentException("Resource with specified name not found");
			return result;
		}
	}
}