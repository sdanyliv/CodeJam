using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace CodeJam.Arithmetic
{
	/// <summary>
	/// Callbacks for common arithmetic actions.
	/// Look at OperatorsPerformanceTest to see why.
	/// </summary>
	// IMPORTANT: DO NOT declare static .ctor on the type. The class should be marked as beforefieldinit. 
	[PublicAPI]
	public static class Operators<T>
	{
		// TODO: catch for T where Comparer<T> does not applies to.

		private static readonly Comparer<T> _comparer = Comparer<T>.Default;

		private static readonly EqualityComparer<T> _equalityComparer = EqualityComparer<T>.Default;

		/// <summary>
		/// Comparison callback
		/// </summary>
		public static readonly Func<T, T, int> Compare = OperatorsFactory.GetComparisonCallback<T>();

		/// <summary>
		/// Equality comparison callback
		/// </summary>
		public static readonly Func<T, T, bool> AreEqual = _equalityComparer.Equals;

		/// <summary>
		/// Inequality comparison callback
		/// </summary>
		public static readonly Func<T, T, bool> AreNotEqual = (a, b) => !_equalityComparer.Equals(a, b);

		/// <summary>
		/// Equality comparison callback
		/// </summary>
		public static readonly Func<T, T, bool> GreaterThan = (a, b) => _comparer.Compare(a, b) > 0;

		/// <summary>
		/// Equality comparison callback
		/// </summary>
		public static readonly Func<T, T, bool> GreaterThanOrEqual = (a, b) => _comparer.Compare(a, b) >= 0;

		/// <summary>
		/// Equality comparison callback
		/// </summary>
		public static readonly Func<T, T, bool> LessThan = (a, b) => _comparer.Compare(a, b) < 0;

		/// <summary>
		/// Equality comparison callback
		/// </summary>
		public static readonly Func<T, T, bool> LessThanOrEqual = (a, b) => _comparer.Compare(a, b) <= 0;

		// TODO: emit (or compile from expression trees) callbacks
		// for all operators (+, -, >, < etc).
		// Proof the efficiency with perftests.
	}
}