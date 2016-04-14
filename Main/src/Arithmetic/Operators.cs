using System;
using System.Linq.Expressions;

using JetBrains.Annotations;

using static CodeJam.Arithmetic.OperatorsFactory;

namespace CodeJam.Arithmetic
{
	/// <summary>
	/// Callbacks for common arithmetic actions.
	/// Look at OperatorsPerformanceTest to see why.
	/// </summary>
	// IMPORTANT: DO NOT declare static .ctor on the type. The class should be marked as beforefieldinit. 
	[PublicAPI]
	public static partial class Operators<T>
	{
		/// <summary>
		/// Comparison callback
		/// </summary>
		public static readonly Func<T, T, int> Compare = GetComparisonCallback<T>();

		/// <summary>
		/// Equality comparison callback
		/// </summary>
		public static readonly Func<T, T, bool> AreEqual = GetComparisonCallback<T>(ExpressionType.Equal);

		/// <summary>
		/// Inequality comparison callback
		/// </summary>
		public static readonly Func<T, T, bool> AreNotEqual = GetComparisonCallback<T>(ExpressionType.NotEqual);

		/// <summary>
		/// Equality comparison callback
		/// </summary>
		public static readonly Func<T, T, bool> GreaterThan = GetComparisonCallback<T>(ExpressionType.GreaterThan);

		/// <summary>
		/// Equality comparison callback
		/// </summary>
		public static readonly Func<T, T, bool> GreaterThanOrEqual = GetComparisonCallback<T>(ExpressionType.GreaterThanOrEqual);

		/// <summary>
		/// Equality comparison callback
		/// </summary>
		public static readonly Func<T, T, bool> LessThan = GetComparisonCallback<T>(ExpressionType.LessThan);

		/// <summary>
		/// Equality comparison callback
		/// </summary>
		public static readonly Func<T, T, bool> LessThanOrEqual = GetComparisonCallback<T>(ExpressionType.LessThanOrEqual);

		// TODO: emit (or compile from expression trees) callbacks
		// for all operators (+, -, *, |, & etc).
		// Proof the efficiency with perftests.
	}
}