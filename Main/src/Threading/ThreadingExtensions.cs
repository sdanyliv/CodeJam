using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace CodeJam.Threading
{
	/// <summary>
	/// Threading related extension methods.
	/// </summary>
	[PublicAPI]
	// TODO: replace Tuple class to struct for performance reasons
	public static class ThreadingExtensions
	{
		#region Memoize
		/// <summary>
		/// Caches function value for specific argument.
		/// </summary>
		/// <param name="func">Function to memoize.</param>
		/// <param name="comparer">Argument comparer</param>
		/// <typeparam name="TArg">Type of argument</typeparam>
		/// <typeparam name="TResult">Type of result</typeparam>
		/// <returns>Memoized function</returns>
		[NotNull]
		[Pure]
		public static Func<TArg, TResult> Memoize<TArg, TResult>(
			[NotNull] this Func<TArg, TResult> func,
			IEqualityComparer<TArg> comparer) =>
				new LazyDictionary<TArg, TResult>(func, comparer).Get;

		/// <summary>
		/// Caches function value for specific argument.
		/// </summary>
		/// <param name="func">Function to memoize.</param>
		/// <typeparam name="TArg">Type of argument</typeparam>
		/// <typeparam name="TResult">Type of result</typeparam>
		/// <returns>Memoized function</returns>
		[NotNull]
		[Pure]
		public static Func<TArg, TResult> Memoize<TArg, TResult>([NotNull] this Func<TArg, TResult> func) =>
			new LazyDictionary<TArg, TResult>(func).Get;

		/// <summary>
		/// Caches function value for specific arguments.
		/// </summary>
		/// <param name="func">Function to memoize.</param>
		/// <typeparam name="TArg1">Type of argument 1</typeparam>
		/// <typeparam name="TArg2">Type of argument 2</typeparam>
		/// <typeparam name="TResult">Type of result</typeparam>
		/// <returns>Memoized function</returns>
		[NotNull]
		[Pure]
		public static Func<TArg1, TArg2, TResult> Memoize<TArg1, TArg2, TResult>(
			[NotNull] this Func<TArg1, TArg2, TResult> func) =>
				(arg1, arg2) => new LazyDictionary<Tuple<TArg1, TArg2>, TResult>(
					key => func(key.Item1, key.Item2)).Get(new Tuple<TArg1, TArg2>(arg1, arg2));

		/// <summary>
		/// Caches function value for specific arguments.
		/// </summary>
		/// <param name="func">Function to memoize.</param>
		/// <typeparam name="TArg1">Type of argument 1</typeparam>
		/// <typeparam name="TArg2">Type of argument 2</typeparam>
		/// <typeparam name="TArg3">Type of argument 3</typeparam>
		/// <typeparam name="TResult">Type of result</typeparam>
		/// <returns>Memoized function</returns>
		[NotNull]
		[Pure]
		public static Func<TArg1, TArg2, TArg3, TResult> Memoize<TArg1, TArg2, TArg3, TResult>(
			[NotNull] this Func<TArg1, TArg2, TArg3, TResult> func) =>
				(arg1, arg2, arg3) => new LazyDictionary<Tuple<TArg1, TArg2, TArg3>, TResult>(
					key => func(key.Item1, key.Item2, key.Item3)).Get(new Tuple<TArg1, TArg2, TArg3>(arg1, arg2, arg3));

		/// <summary>
		/// Caches function value for specific arguments.
		/// </summary>
		/// <param name="func">Function to memoize.</param>
		/// <typeparam name="TArg1">Type of argument 1</typeparam>
		/// <typeparam name="TArg2">Type of argument 2</typeparam>
		/// <typeparam name="TArg3">Type of argument 3</typeparam>
		/// <typeparam name="TArg4">Type of argument 4</typeparam>
		/// <typeparam name="TResult">Type of result</typeparam>
		/// <returns>Memoized function</returns>
		[NotNull]
		[Pure]
		public static Func<TArg1, TArg2, TArg3, TArg4, TResult> Memoize<TArg1, TArg2, TArg3, TArg4, TResult>(
			[NotNull] this Func<TArg1, TArg2, TArg3, TArg4, TResult> func) =>
				(arg1, arg2, arg3, arg4) => new LazyDictionary<Tuple<TArg1, TArg2, TArg3, TArg4>, TResult>(
					key => func(key.Item1, key.Item2, key.Item3, key.Item4))
						.Get(new Tuple<TArg1, TArg2, TArg3, TArg4>(arg1, arg2, arg3, arg4));

		/// <summary>
		/// Caches function value for specific arguments.
		/// </summary>
		/// <param name="func">Function to memoize.</param>
		/// <typeparam name="TArg1">Type of argument 1</typeparam>
		/// <typeparam name="TArg2">Type of argument 2</typeparam>
		/// <typeparam name="TArg3">Type of argument 3</typeparam>
		/// <typeparam name="TArg4">Type of argument 4</typeparam>
		/// <typeparam name="TArg5">Type of argument 5</typeparam>
		/// <typeparam name="TResult">Type of result</typeparam>
		/// <returns>Memoized function</returns>
		[NotNull]
		[Pure]
		public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult> Memoize<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(
			[NotNull] this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult> func) =>
				(arg1, arg2, arg3, arg4, arg5) => new LazyDictionary<Tuple<TArg1, TArg2, TArg3, TArg4, TArg5>, TResult>(
					key => func(key.Item1, key.Item2, key.Item3, key.Item4, key.Item5))
						.Get(new Tuple<TArg1, TArg2, TArg3, TArg4, TArg5>(arg1, arg2, arg3, arg4, arg5));
		#endregion
	}
}