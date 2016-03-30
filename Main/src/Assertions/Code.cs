using System;
using System.Diagnostics;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// Assertions class.
	/// </summary>
	[PublicAPI]
	public static class Code
	{
		#region Argument validation
		/// <summary>
		/// Ensures that <paramref name="arg" /> != <c>null</c>
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod]
		public static void NotNull<T>(
			[CanBeNull, NoEnumeration] T arg,
			[NotNull] [InvokerParameterName] string argName) where T : class
		{
			if (arg == null)
				throw CodeExceptions.ArgumentNull(argName);
		}

		/// <summary>
		/// Ensures that <paramref name="arg" /> != <c>null</c>
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod]
		public static void NotNull<T>(
			[CanBeNull] T? arg,
			[NotNull] [InvokerParameterName] string argName) where T : struct
		{
			if (arg == null)
				throw CodeExceptions.ArgumentNull(argName);
		}

		/// <summary>
		/// Ensures that <paramref name="arg" /> is not null nor empty
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod]
		public static void NotNullNorEmpty(
			[CanBeNull] string arg,
			[NotNull] [InvokerParameterName] string argName)
		{
			if (string.IsNullOrEmpty(arg))
				throw CodeExceptions.ArgumentNullOrEmpty(argName);
		}

		/// <summary>
		/// Assertion for the argument value
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod]
		public static void AssertArgument(
			bool condition,
			[NotNull] [InvokerParameterName] string argName,
			[NotNull] string message)
		{
			if (!condition)
				throw CodeExceptions.Argument(argName, message);
		}

		/// <summary>
		/// Assertion for the argument value
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod, StringFormatMethod("messageFormat")]
		public static void AssertArgument(
			bool condition,
			[NotNull] [InvokerParameterName] string argName,
			[NotNull] string messageFormat,
			[CanBeNull] params object[] args)
		{
			if (!condition)
				throw CodeExceptions.Argument(argName, messageFormat, args);
		}
		#endregion

		#region State validation
		/// <summary>
		/// State assertion
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod]
		public static void AssertState(
			bool condition,
			[NotNull] string message)
		{
			if (!condition)
				throw CodeExceptions.InvalidOperation(message);
		}

		/// <summary>
		/// State assertion
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod, StringFormatMethod("messageFormat")]
		public static void AssertState(
			bool condition,
			[NotNull] string messageFormat,
			[CanBeNull] params object[] args)
		{
			if (!condition)
				throw CodeExceptions.InvalidOperation(messageFormat, args);
		}
		#endregion

		#region DisposedIf assertions (DO NOT copy into DebugCode)
		// NB: ObjectDisposedException should be thrown from all builds or not thrown at all.
		// There's no point in pairing these assertions with a debug-time-only ones

		/// <summary>
		/// Assertion for object disposal
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod]
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