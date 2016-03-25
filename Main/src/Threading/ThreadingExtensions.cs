using System;
using System.Collections.Generic;

using CodeJam.Collections;

using JetBrains.Annotations;

namespace CodeJam.Threading
{
	/// <summary>
	/// Threading related extension methods.
	/// </summary>
	[PublicAPI]
	public static class ThreadingExtensions
	{
		#region Memoize
		/// <summary>
		/// Caches function value for specific argument.
		/// </summary>
		/// <param name="func">Function to memoize.</param>
		/// <param name="comparer">Argument comparer</param>
		/// <param name="threadSafe">If true - returns thread safe implementation</param>
		/// <typeparam name="TArg">Type of argument</typeparam>
		/// <typeparam name="TResult">Type of result</typeparam>
		/// <returns>Memoized function</returns>
		[NotNull]
		[Pure]
		public static Func<TArg, TResult> Memoize<TArg, TResult>(
			[NotNull] this Func<TArg, TResult> func,
			IEqualityComparer<TArg> comparer,
			bool threadSafe = false) =>
				LazyDictionary.Create(func, comparer, threadSafe).Get;

		/// <summary>
		/// Caches function value for specific argument.
		/// </summary>
		/// <param name="func">Function to memoize.</param>
		/// <param name="threadSafe">If true - returns thread safe implementation</param>
		/// <typeparam name="TArg">Type of argument</typeparam>
		/// <typeparam name="TResult">Type of result</typeparam>
		/// <returns>Memoized function</returns>
		[NotNull]
		[Pure]
		public static Func<TArg, TResult> Memoize<TArg, TResult>(
				[NotNull] this Func<TArg, TResult> func,
				bool threadSafe = false) =>
			LazyDictionary.Create(func, threadSafe).Get;

		/// <summary>
		/// Caches function value for specific arguments.
		/// </summary>
		/// <param name="func">Function to memoize.</param>
		/// <param name="threadSafe">If true - returns thread safe implementation</param>
		/// <typeparam name="TArg1">Type of argument 1</typeparam>
		/// <typeparam name="TArg2">Type of argument 2</typeparam>
		/// <typeparam name="TResult">Type of result</typeparam>
		/// <returns>Memoized function</returns>
		[NotNull]
		[Pure]
		public static Func<TArg1, TArg2, TResult> Memoize<TArg1, TArg2, TResult>(
			[NotNull] this Func<TArg1, TArg2, TResult> func,
			bool threadSafe = false)
		{
			var map = LazyDictionary.Create<TupleStruct<TArg1, TArg2>, TResult>(key => func(key.Item1, key.Item2), threadSafe);
			return (arg1, arg2) => map.Get(TupleStruct.Create(arg1, arg2));
		}

		/// <summary>
		/// Caches function value for specific arguments.
		/// </summary>
		/// <param name="func">Function to memoize.</param>
		/// <param name="threadSafe">If true - returns thread safe implementation</param>
		/// <typeparam name="TArg1">Type of argument 1</typeparam>
		/// <typeparam name="TArg2">Type of argument 2</typeparam>
		/// <typeparam name="TArg3">Type of argument 3</typeparam>
		/// <typeparam name="TResult">Type of result</typeparam>
		/// <returns>Memoized function</returns>
		[NotNull]
		[Pure]
		public static Func<TArg1, TArg2, TArg3, TResult> Memoize<TArg1, TArg2, TArg3, TResult>(
			[NotNull] this Func<TArg1, TArg2, TArg3, TResult> func,
			bool threadSafe = false)
		{
			var map =
				LazyDictionary.Create<TupleStruct<TArg1, TArg2, TArg3>, TResult>(
					key => func(key.Item1, key.Item2, key.Item3),
					threadSafe);
			return (arg1, arg2, arg3) => map.Get(TupleStruct.Create(arg1, arg2, arg3));
		}

		/// <summary>
		/// Caches function value for specific arguments.
		/// </summary>
		/// <param name="func">Function to memoize.</param>
		/// <param name="threadSafe">If true - returns thread safe implementation</param>
		/// <typeparam name="TArg1">Type of argument 1</typeparam>
		/// <typeparam name="TArg2">Type of argument 2</typeparam>
		/// <typeparam name="TArg3">Type of argument 3</typeparam>
		/// <typeparam name="TArg4">Type of argument 4</typeparam>
		/// <typeparam name="TResult">Type of result</typeparam>
		/// <returns>Memoized function</returns>
		[NotNull]
		[Pure]
		public static Func<TArg1, TArg2, TArg3, TArg4, TResult> Memoize<TArg1, TArg2, TArg3, TArg4, TResult>(
			[NotNull] this Func<TArg1, TArg2, TArg3, TArg4, TResult> func,
			bool threadSafe = false)
		{
			var map =
				LazyDictionary.Create<TupleStruct<TArg1, TArg2, TArg3, TArg4>, TResult>(
					key => func(key.Item1, key.Item2, key.Item3, key.Item4),
					threadSafe);
			return (arg1, arg2, arg3, arg4) => map.Get(TupleStruct.Create(arg1, arg2, arg3, arg4));
		}

		/// <summary>
		/// Caches function value for specific arguments.
		/// </summary>
		/// <param name="func">Function to memoize.</param>
		/// <param name="threadSafe">If true - returns thread safe implementation</param>
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
			[NotNull] this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult> func,
			bool threadSafe = false)
		{
			var map =
				LazyDictionary.Create<TupleStruct<TArg1, TArg2, TArg3, TArg4, TArg5>, TResult>(
					key => func(key.Item1, key.Item2, key.Item3, key.Item4, key.Item5),
					threadSafe);
			return (arg1, arg2, arg3, arg4, arg5) => map.Get(TupleStruct.Create(arg1, arg2, arg3, arg4, arg5));
		}
		#endregion
	}
}