using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using JetBrains.Annotations;

namespace CodeJam.Arithmetic
{
	/// <summary>
	/// Helper class to emit operators logic
	/// </summary>
	internal static class OperatorsFactory
	{
		[CanBeNull]
		public static Func<T, T, int> GetComparisonCallback<T>()
		{
			var t = typeof(T);

			// Recommendation from https://msdn.microsoft.com/en-us/library/azhsac5f.aspx
			// For string comparisons, the StringComparer class is recommended over Comparer<String>
			if (t == typeof(string))
				return (Func<T, T, int>)(object)(Func<string, string, int>)StringComparer.Ordinal.Compare;

			if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
				t = t.GetGenericArguments()[0];

			if (!typeof(IComparable<T>).IsAssignableFrom(t) &&
				!typeof(IComparable).IsAssignableFrom(t))
				return null;

			return Comparer<T>.Default.Compare;
		}

		[CanBeNull]
		public static Func<T, T, bool> GetComparisonCallback<T>(ExpressionType comparisonType) =>
			CompareUsingOperators<T>(comparisonType) ??
				CompareUsingComparer<T>(comparisonType);

		[CanBeNull]
		public static Func<T, T, bool> CompareUsingOperators<T>(ExpressionType comparisonType)
		{
			switch (comparisonType)
			{
				case ExpressionType.Equal:
				case ExpressionType.NotEqual:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
					// OK
					break;
				default:
					throw CodeExceptions.UnexpectedArgumentValue(
						nameof(comparisonType), comparisonType);
			}

			var parameter1 = Expression.Parameter(typeof(T), "arg1");
			var parameter2 = Expression.Parameter(typeof(T), "arg2");

			Expression body;
			try
			{
				body = Expression.MakeBinary(comparisonType, parameter1, parameter2);
			}
			catch (InvalidOperationException)
			{
				return null;
			}

			var result = Expression.Lambda<Func<T, T, bool>>(
				body, comparisonType.ToString(), new[] { parameter1, parameter2 });

			try
			{
				return result.Compile();
			}
			catch (Exception)
			{
				return null;
			}
		}

		[CanBeNull]
		public static Func<T, T, bool> CompareUsingComparer<T>(ExpressionType comparisonType)
		{
			switch (comparisonType)
			{
				case ExpressionType.Equal:
					var equalityComparer = EqualityComparer<T>.Default;
					return (a, b) => equalityComparer.Equals(a, b);
				case ExpressionType.NotEqual:
					equalityComparer = EqualityComparer<T>.Default;
					return (a, b) => !equalityComparer.Equals(a, b);
			}

			var comparison = GetComparisonCallback<T>();
			if (comparison == null)
				return null;

			switch (comparisonType)
			{
				case ExpressionType.GreaterThan:
					return (a, b) => comparison(a, b) > 0;
				case ExpressionType.GreaterThanOrEqual:
					return (a, b) => comparison(a, b) >= 0;
				case ExpressionType.LessThan:
					return (a, b) => comparison(a, b) < 0;
				case ExpressionType.LessThanOrEqual:
					return (a, b) => comparison(a, b) <= 0;
				default:
					throw CodeExceptions.UnexpectedArgumentValue(
						nameof(comparisonType), comparisonType);
			}
		}
	}
}