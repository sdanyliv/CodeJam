using System;
using System.Linq.Expressions;
using System.Reflection;

using JetBrains.Annotations;

namespace CodeJam.Reflection
{
	[PublicAPI]
	public static class InfoOf
	{
		/// <summary>
		/// Returns the property.
		/// </summary>
		/// <param name="expression">The expression to analyze.</param>
		/// <returns>
		/// The <see cref="PropertyInfo"/> instance.
		/// </returns>
		[NotNull, Pure]
		public static PropertyInfo Property<TValue>([NotNull] Expression<Func<TValue>> expression) =>
			ExpressionHelper.GetProperty(expression);

		/// <summary>
		/// Returns the field.
		/// </summary>
		/// <param name="expression">The expression to analyze.</param>
		/// <returns>
		/// The <see cref="FieldInfo"/> instance.
		/// </returns>
		[NotNull, Pure]
		public static FieldInfo Field<TValue>([NotNull] Expression<Func<TValue>> expression) =>
			ExpressionHelper.GetField(expression);

		/// <summary>
		/// Returns the property or field.
		/// </summary>
		/// <param name="expression">The expression to analyze.</param>
		/// <returns>
		/// The <see cref="MemberInfo"/> instance.
		/// </returns>
		[NotNull, Pure]
		public static MemberInfo PropertyOrField<TValue>([NotNull] Expression<Func<TValue>> expression) =>
			ExpressionHelper.GetPropertyOrField(expression);

		/// <summary>
		/// Returns the contsructor.
		/// </summary>
		/// <param name="expression">The expression to analyze.</param>
		/// <returns>
		/// The <see cref="ConstructorInfo"/> instance.
		/// </returns>
		[NotNull, Pure]
		public static ConstructorInfo Constructor<T>([NotNull] Expression<Func<T>> expression) =>
			ExpressionHelper.GetConstructor(expression);

		/// <summary>
		/// Returns the method.
		/// </summary>
		/// <param name="expression">The expression to analyze.</param>
		/// <returns>
		/// The <see cref="MethodInfo"/> instance.
		/// </returns>
		[NotNull, Pure]
		public static MethodInfo Method([NotNull] Expression<Action> expression) =>
			ExpressionHelper.GetMethod(expression);
	}
}
