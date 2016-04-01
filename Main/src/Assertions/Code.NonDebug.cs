using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// Assertions class. Part that excluded from debug assertions generation.
	/// </summary>
	static partial class Code
	{
		#region DisposedIf assertions (DO NOT copy into DebugCode)
		// NB: ObjectDisposedException should be thrown from all builds or not thrown at all.
		// There's no point in pairing these assertions with a debug-time-only ones

		/// <summary>
		/// Assertion for object disposal
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod]
		[MethodImpl(PlatformDependent.AggressiveInlining)]
		public static void DisposedIf<TDisposable>(
			bool disposed,
			[NotNull] TDisposable thisReference)
			where TDisposable : IDisposable
		{
			if (disposed)
				throw CodeExceptions.ObjectDisposed(thisReference.GetType());
		}

		/// <summary>
		/// Assertion for object disposal
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod]
		[MethodImpl(PlatformDependent.AggressiveInlining)]
		public static void DisposedIf<TDisposable>(
			bool disposed,
			[NotNull] TDisposable thisReference,
			[NotNull] string message)
			where TDisposable : IDisposable
		{
			if (disposed)
				throw CodeExceptions.ObjectDisposed(thisReference.GetType(), message);
		}

		/// <summary>
		/// Assertion for object disposal
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod, StringFormatMethod("messageFormat")]
		[MethodImpl(PlatformDependent.AggressiveInlining)]
		public static void DisposedIf<TDisposable>(
			bool disposed,
			[NotNull] TDisposable thisReference,
			[NotNull] string messageFormat,
			[CanBeNull] params object[] args)
			where TDisposable : IDisposable
		{
			if (disposed)
				throw CodeExceptions.ObjectDisposed(thisReference.GetType(), messageFormat, args);
		}

		/// <summary>
		/// Assertion for object disposal
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod]
		[MethodImpl(PlatformDependent.AggressiveInlining)]
		public static void DisposedIfNull<TResource, TDisposable>(
			TResource resource,
			[NotNull] TDisposable thisReference)
			where TResource : class
			where TDisposable : IDisposable
		{
			if (resource == null)
				throw CodeExceptions.ObjectDisposed(thisReference.GetType());
		}

		/// <summary>
		/// Assertion for object disposal
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod]
		[MethodImpl(PlatformDependent.AggressiveInlining)]
		public static void DisposedIfNull<TResource, TDisposable>(
			TResource resource,
			[NotNull] TDisposable thisReference,
			[NotNull] string message)
			where TResource : class
			where TDisposable : IDisposable
		{
			if (resource == null)
				throw CodeExceptions.ObjectDisposed(thisReference.GetType(), message);
		}

		/// <summary>
		/// Assertion for object disposal
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod, StringFormatMethod("messageFormat")]
		[MethodImpl(PlatformDependent.AggressiveInlining)]
		public static void DisposedIfNull<TResource, TDisposable>(
			TResource resource,
			[NotNull] TDisposable thisReference,
			[NotNull] string messageFormat,
			[CanBeNull] params object[] args)
			where TResource : class
			where TDisposable : IDisposable
		{
			if (resource == null)
				throw CodeExceptions.ObjectDisposed(thisReference.GetType(), messageFormat, args);
		}
		#endregion
	}
}