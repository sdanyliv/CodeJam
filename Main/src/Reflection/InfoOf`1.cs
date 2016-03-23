using System;
using System.Linq.Expressions;
using System.Reflection;

using JetBrains.Annotations;

namespace CodeJam
{
	[PublicAPI]
	public static class InfoOf<T>
	{
		[NotNull, Pure]
		public static PropertyInfo Property<TValue>([NotNull] Expression<Func<T, TValue>> expression) =>
			ExpressionHelper.GetProperty(expression);

		[NotNull, Pure]
		public static FieldInfo Field<TValue>([NotNull] Expression<Func<T, TValue>> expression) =>
			ExpressionHelper.GetField(expression);

		[NotNull, Pure]
		public static MemberInfo PropertyOrField<TValue>([NotNull] Expression<Func<T, TValue>> expression) =>
			ExpressionHelper.GetPropertyOrField(expression);

		[NotNull, Pure]
		public static ConstructorInfo Constructor([NotNull] Expression<Func<T>> expression) =>
			ExpressionHelper.GetConstructor(expression);

		[NotNull, Pure]
		public static MethodInfo Method([NotNull] Expression<Action<T>> expression) =>
			ExpressionHelper.GetMethod(expression);
	}
}
