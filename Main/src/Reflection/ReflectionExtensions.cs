using System;
using System.Reflection;

using JetBrains.Annotations;

namespace CodeJam.Reflection
{
	/// <summary>
	/// Reflection extension methods.
	/// </summary>
	[PublicAPI]
	public static partial class ReflectionExtensions
	{
		/// <summary>
		/// Gets a value indicating whether the <paramref name="type"/> can be instantiated.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> to test.</param>
		/// <returns>
		/// A value indicating whether the <paramref name="type"/> can be instantiated.
		/// </returns>
		[Pure]
		public static bool IsInstantiable([NotNull] this Type type) =>
			!(type.IsAbstract || type.IsInterface || type.IsArray || type.ContainsGenericParameters);

		/// <summary>
		/// Gets a value indicating whether the <paramref name="type"/> is declared static.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> to test.</param>
		/// <returns>
		/// A value indicating whether the <paramref name="type"/> is declared static.
		/// </returns>
		[Pure]
		public static bool IsStatic([NotNull] this Type type) =>
			type.IsClass && type.IsAbstract && type.IsSealed;

		/// <summary>
		/// Gets a value indicating whether the <paramref name="type"/> is Nullable&#60;&#62; type.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> to test.</param>
		/// <returns>
		/// A value indicating whether the <paramref name="type"/> is Nullable&#60;&#62;.
		/// </returns>
		[Pure]
		public static bool IsNullable([CanBeNull] this Type type) =>
			type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

		/// <summary>
		/// Determines whether the <paramref name="type"/> derives from the specified <paramref name="check"/>.
		/// </summary>
		/// <remarks>
		/// This method also returns false if <paramref name="type"/> and the <paramref name="check"/> are equal.
		/// </remarks>
		/// <param name="type">The type to test.</param>
		/// <param name="check">The type to compare with. </param>
		/// <returns>
		/// true if the <paramref name="type"/> derives from <paramref name="check"/>; otherwise, false.
		/// </returns>
		[Pure]
		public static bool IsSubClass([NotNull] this Type type, [NotNull] Type check)
		{
			if (type == check)
				return false;

			while (true)
			{
				if (check.IsInterface)
					// ReSharper disable once LoopCanBeConvertedToQuery
					foreach (var interfaceType in type.GetInterfaces())
						if (interfaceType == check || interfaceType.IsSubClass(check))
							return true;

				if (type.IsGenericType && !type.IsGenericTypeDefinition)
				{
					var definition = type.GetGenericTypeDefinition();
					if (definition == check || definition.IsSubClass(check))
						return true;
				}

				type = type.BaseType;

				if (type == null)
					return false;

				if (type == check)
					return true;
			}
		}

		/// <summary>
		/// Returns delegate parameter infos.
		/// </summary>
		/// <param name="delegateType">Type of delegate</param>
		/// <returns>Array of <see cref="ParameterInfo"/>.</returns>
		[NotNull]
		public static ParameterInfo[] GetDelegateParams([NotNull] Type delegateType)
		{
			if (delegateType == null)
				throw new ArgumentNullException(nameof(delegateType));
			return delegateType.GetMethod("Invoke").GetParameters();
		}

		/// <summary>
		/// Returns the underlying type argument of the specified type.
		/// </summary>
		/// <param name="type">A <see cref="System.Type"/> instance. </param>
		/// <returns><list>
		/// <item>The type argument of the type parameter,
		/// if the type parameter is a closed generic nullable type.</item>
		/// <item>The underlying Type if the type parameter is an enum type.</item>
		/// <item>Otherwise, the type itself.</item>
		/// </list>
		/// </returns>
		[Pure]
		public static Type ToUnderlying([NotNull] this Type type)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));

			if (type.IsNullable()) type = type.GetGenericArguments()[0];
			if (type.IsEnum)       type = Enum.GetUnderlyingType(type);

			return type;
		}

		/// <summary>
		/// Gets the type of this member.
		/// </summary>
		/// <param name="memberInfo">A <see cref="System.Reflection.MemberInfo"/> instance. </param>
		/// <returns>
		/// <list>
		/// <item>
		/// If the member is a property, returns <see cref="System.Reflection.PropertyInfo.PropertyType"/>.
		/// If the member is a field, returns <see cref="System.Reflection.FieldInfo.FieldType"/>.
		/// If the member is a method, returns <see cref="System.Reflection.MethodInfo.ReturnType"/>.
		/// If the member is a constructor, returns <see cref="System.Reflection.MemberInfo.DeclaringType"/>.
		/// If the member is an event, returns <see cref="System.Reflection.EventInfo.EventHandlerType"/>.
		/// </item>
		/// </list>
		/// </returns>
		public static Type GetMemberType([NotNull] this MemberInfo memberInfo)
		{
			if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

			switch (memberInfo.MemberType)
			{
				case MemberTypes.Property    : return ((PropertyInfo)memberInfo).PropertyType;
				case MemberTypes.Field       : return ((FieldInfo)   memberInfo).FieldType;
				case MemberTypes.Method      : return ((MethodInfo)  memberInfo).ReturnType;
				case MemberTypes.Constructor : return                memberInfo. DeclaringType;
				case MemberTypes.Event       : return ((EventInfo)   memberInfo).EventHandlerType;
			}

			throw new InvalidOperationException();
		}
	}
}
