using System;
using System.Collections.Generic;

namespace CodeJam.Arithmetic
{
	/// <summary>
	/// Helper class to emit operators logic
	/// </summary>
	internal static class OperatorsFactory
	{
		public static Func<T, T, int> GetComparisonCallback<T>()
		{
			var t = typeof(T);

			// Recommendation from https://msdn.microsoft.com/en-us/library/azhsac5f.aspx
			// For string comparisons, the StringComparer class is recommended over Comparer<String>
			if (t == typeof(string))
				return (Func<T,T, int>)(object)(Func<string, string, int>)StringComparer.Ordinal.Compare;

			if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
				t = t.GetGenericArguments()[0];

			if (!typeof(IComparable<T>).IsAssignableFrom(t) &&
				!typeof(IComparable).IsAssignableFrom(t))
				return null;

			return Comparer<T>.Default.Compare;
		}
	}
}