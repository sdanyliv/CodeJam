using System;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// Extensions for <see cref="Option{T}"/>
	/// </summary>
	[PublicAPI]
	public static class OptionExtensions
	{
		/// <summary>
		/// Calls <paramref name="someAction"/> if <paramref name="option"/> has value,
		/// and <paramref name="noneAction"/> otherwise.
		/// </summary>
		public static void Match<T>(
			this Option<T> option,
			[InstantHandle]Action<Option<T>> someAction,
			[InstantHandle]Action noneAction)
		{
			if (option.HasValue)
				someAction(option);
			else
				noneAction();
		}

		/// <summary>
		/// Calls <paramref name="someFunc"/> if <paramref name="option"/> has value,
		/// and <paramref name="noneFunc"/> otherwise.
		/// </summary>
		[Pure]
		public static TResult Match<T, TResult>(
			this Option<T> option,
			[InstantHandle] Func<Option<T>, TResult> someFunc,
			[InstantHandle] Func<TResult> noneFunc) =>
				option.HasValue ? someFunc(option) : noneFunc();

		/// <summary>
		/// Returns value of <paramref name="option"/>, or <paramref name="defaultValue"/> if <paramref name="option"/>
		/// hasn't it.
		/// </summary>
		[Pure]
		public static T GetValueOrDefault<T>(this Option<T> option, T defaultValue = default(T)) =>
			option.HasValue ? option.Value : defaultValue;

		/// <summary>
		/// Converts <paramref name="option"/> value to another option with <paramref name="selectFunc"/>.
		/// </summary>
		[Pure]
		public static Option<TResult> Map<T, TResult>(
			this Option<T> option,
			[InstantHandle] Func<T, TResult> selectFunc) =>
				option.HasValue ? new Option<TResult>(selectFunc(option.Value)) : new Option<TResult>();
	}
}