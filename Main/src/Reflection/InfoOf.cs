using System;
using System.Linq.Expressions;
using System.Reflection;

using JetBrains.Annotations;

namespace CodeJam
{
	[PublicAPI]
	public static class InfoOf
	{
		[NotNull, Pure]
		public static PropertyInfo Property<TValue>([NotNull] Expression<Func<TValue>> expression) =>
			ExpressionHelper.GetProperty(expression);

		[NotNull, Pure]
		public static FieldInfo Field<TValue>([NotNull] Expression<Func<TValue>> expression) =>
			ExpressionHelper.GetField(expression);

		[NotNull, Pure]
		public static MemberInfo PropertyOrField<TValue>([NotNull] Expression<Func<TValue>> expression) =>
			ExpressionHelper.GetPropertyOrField(expression);

		[NotNull, Pure]
		public static MethodInfo Method([NotNull] Expression<Action> expression) =>
			ExpressionHelper.GetMethod(expression);
	}
}
