using System;
using System.Linq.Expressions;
using System.Reflection;

using JetBrains.Annotations;

namespace CodeJam
{
	[PublicAPI]
	public static class ExpressionHelper
	{
		[NotNull, Pure]
		public static PropertyInfo GetProperty([NotNull] LambdaExpression expression) =>
			(PropertyInfo)GetMemberExpression(expression).Member;

		[NotNull, Pure]
		public static FieldInfo GetField([NotNull] LambdaExpression expression) =>
			(FieldInfo)GetMemberExpression(expression).Member;

		[NotNull, Pure]
		public static MemberInfo GetPropertyOrField([NotNull] LambdaExpression expression) =>
			GetMemberExpression(expression).Member;

		[NotNull, Pure]
		public static ConstructorInfo GetConstructor([NotNull] LambdaExpression expression) =>
			((NewExpression)expression.Body).Constructor;

		[NotNull, Pure]
		public static MethodInfo GetMethod([NotNull] LambdaExpression expression) =>
			((MethodCallExpression)expression.Body).Method;

		[NotNull, Pure]
		public static string GetPropertyName([NotNull] LambdaExpression expression) =>
			GetMemberExpression(expression).Member.Name;

		[NotNull, Pure]
		public static string GetFullPropertyName([NotNull] LambdaExpression expression) =>
			GetFullPropertyNameImpl(GetMemberExpression(expression));

		[NotNull, Pure]
		public static string GetMethodName([NotNull] LambdaExpression expression) =>
			((MethodCallExpression)expression.Body).Method.Name;

		private static string GetFullPropertyNameImpl(MemberExpression expression)
		{
			var name = expression.Member.Name;
			while ((expression = expression.Expression as MemberExpression) != null)
				name = expression.Member.Name + "." + name;

			return name;
		}

		private static MemberExpression GetMemberExpression(LambdaExpression expression)
		{
			var body = expression.Body;
			var unary = body as UnaryExpression;
			return unary != null
				? (MemberExpression)unary.Operand
				: (MemberExpression)body;
		}
	}
}
