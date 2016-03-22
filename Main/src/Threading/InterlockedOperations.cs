using System;
using System.Diagnostics;
using System.Threading;

using JetBrains.Annotations;

namespace CodeJam.Threading
{
	/// <summary>
	/// Provides a helper class for initializing a values in a thread-safe manner.
	/// </summary>
	[PublicAPI]
	public static class InterlockedOperations
	{
		/// <summary>
		/// Initialize the value referenced by <paramref name="target"/> in a thread-safe manner.
		/// The value is changed to <paramref name="value"/> only if the current value is null.
		/// </summary>
		/// <typeparam name="T">Type of value.</typeparam>
		/// <param name="target">Reference to the target location.</param>
		/// <param name="value">The value to use if the target is currently null.</param>
		/// <returns>
		/// The new value referenced by <paramref name="target"/>.
		/// Note that this is nearly always more useful than the usual
		/// return from <see cref="Interlocked.CompareExchange{T}(ref T, T, T)"/>
		/// because it saves another read to <paramref name="target"/>.
		/// </returns>
		public static T Initialize<T>(ref T target, [NotNull] T value) where T : class
		{
			Debug.Assert(value != null);
			return Interlocked.CompareExchange(ref target, value, null) ?? value;
		}

		/// <summary>
		/// Initialize the value referenced by <paramref name="target"/> in a thread-safe manner.
		/// The value is changed to <paramref name="initializedValue"/> only if the current value
		/// is <paramref name="uninitializedValue"/>.
		/// </summary>
		/// <typeparam name="T">Type of value.</typeparam>
		/// <param name="target">Reference to the target location.</param>
		/// <param name="initializedValue">The value to use if the target is currently uninitialized.</param>
		/// <param name="uninitializedValue">The uninitialized value.</param>
		/// <returns>
		/// The new value referenced by <paramref name="target"/>.
		/// Note that this is nearly always more useful than the usual
		/// return from <see cref="Interlocked.CompareExchange{T}(ref T, T, T)"/>
		/// because it saves another read to <paramref name="target"/>.
		/// </returns>
		public static T Initialize<T>(ref T target, T initializedValue, T uninitializedValue) where T : class
		{
			Debug.Assert(initializedValue != uninitializedValue);
			var oldValue = Interlocked.CompareExchange(ref target, initializedValue, uninitializedValue);
			return oldValue == uninitializedValue ? initializedValue : oldValue;
		}
	}
}
