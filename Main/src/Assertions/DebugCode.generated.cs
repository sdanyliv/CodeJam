using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// Assertions class.
	/// </summary>
	[PublicAPI]
	public static partial class DebugCode
	{
		#region Argument validation
		/// <summary>
		/// Ensures that <paramref name="arg" /> != <c>null</c>
		/// </summary>
		[Conditional(DebugCode.DebugCondition), DebuggerHidden]
		[AssertionMethod]
		[MethodImpl(PlatformDependent.AggressiveInlining)]
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
		[Conditional(DebugCode.DebugCondition), DebuggerHidden]
		[AssertionMethod]
		[MethodImpl(PlatformDependent.AggressiveInlining)]
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
		[Conditional(DebugCode.DebugCondition), DebuggerHidden]
		[AssertionMethod]
		[MethodImpl(PlatformDependent.AggressiveInlining)]
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
		[Conditional(DebugCode.DebugCondition), DebuggerHidden]
		[AssertionMethod]
		[MethodImpl(PlatformDependent.AggressiveInlining)]
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
		[Conditional(DebugCode.DebugCondition), DebuggerHidden]
		[AssertionMethod, StringFormatMethod("messageFormat")]
		[MethodImpl(PlatformDependent.AggressiveInlining)]
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
		[Conditional(DebugCode.DebugCondition), DebuggerHidden]
		[AssertionMethod]
		[MethodImpl(PlatformDependent.AggressiveInlining)]
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
		[Conditional(DebugCode.DebugCondition), DebuggerHidden]
		[AssertionMethod, StringFormatMethod("messageFormat")]
		[MethodImpl(PlatformDependent.AggressiveInlining)]
		public static void AssertState(
			bool condition,
			[NotNull] string messageFormat,
			[CanBeNull] params object[] args)
		{
			if (!condition)
				throw CodeExceptions.InvalidOperation(messageFormat, args);
		}
		#endregion
	}
}