using System;
using System.Diagnostics;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// Assertions class.
	/// </summary>
	public static class Code
	{
		/// <summary>
		/// Ensures that <paramref name="arg" /> != <c>null</c>
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod]
		public static void NotNull<T>(
			[CanBeNull] [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] T arg,
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
			// ReSharper disable once ConvertNullableToShortForm
			// Will conflict with C# vNext nullable references
			[CanBeNull] [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] Nullable<T> arg,
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
			[CanBeNull] [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] string arg,
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
			[AssertionCondition(AssertionConditionType.IS_TRUE)] bool condition,
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
			[AssertionCondition(AssertionConditionType.IS_TRUE)] bool condition,
			[NotNull] [InvokerParameterName] string argName,
			[NotNull] string messageFormat,
			[CanBeNull] params object[] args)
		{
			if (!condition)
				throw CodeExceptions.Argument(argName, messageFormat, args);
		}

		/// <summary>
		/// State assertion
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod]
		public static void AssertState(
			[AssertionCondition(AssertionConditionType.IS_TRUE)] bool condition,
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
			[AssertionCondition(AssertionConditionType.IS_TRUE)] bool condition,
			[NotNull] string messageFormat,
			[CanBeNull] params object[] args)
		{
			if (!condition)
				throw CodeExceptions.InvalidOperation(messageFormat, args);
		}
	}
}