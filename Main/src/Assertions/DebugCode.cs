using System;
using System.Diagnostics;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// Debug-time assertions class.
	/// </summary>
	[PublicAPI]
	public static class DebugCode
	{
		/// <summary>
		/// Conditional symbol for debug builds.
		/// </summary>
		public const string DebugCondition = "DEBUG";

		/// <summary>
		/// Ensures that <paramref name="arg" /> != <c>null</c>
		/// </summary>
		[Conditional(DebugCondition), DebuggerHidden]
		[AssertionMethod]
		public static void NotNull<T>(
			[CanBeNull, NoEnumeration] T arg,
			[NotNull] [InvokerParameterName] string argName) where T : class =>
				Code.NotNull(arg, argName);

		/// <summary>
		/// Ensures that <paramref name="arg" /> != <c>null</c>
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod]
		public static void NotNull<T>(
			[CanBeNull] T? arg,
			[NotNull] [InvokerParameterName] string argName) where T : struct =>
				// ReSharper disable once AssignNullToNotNullAttribute
				Code.NotNull(arg, argName);

		/// <summary>
		/// Ensures that <paramref name="arg" /> is not null nor empty
		/// </summary>
		[Conditional(DebugCondition), DebuggerHidden]
		[AssertionMethod]
		public static void NotNullNorEmpty(
			[CanBeNull] string arg,
			[NotNull] [InvokerParameterName] string argName) =>
				Code.NotNullNorEmpty(arg, argName);

		/// <summary>
		/// Assertion for the argument value
		/// </summary>
		[Conditional(DebugCondition), DebuggerHidden]
		[AssertionMethod]
		public static void AssertArgument(
			bool condition,
			[NotNull] [InvokerParameterName] string argName,
			[NotNull] string message) =>
				Code.AssertArgument(condition, argName, message);

		/// <summary>
		/// Assertion for the argument value
		/// </summary>
		[Conditional(DebugCondition), DebuggerHidden]
		[AssertionMethod, StringFormatMethod("messageFormat")]
		public static void AssertArgument(
			bool condition,
			[NotNull] [InvokerParameterName] string argName,
			[NotNull] string messageFormat,
			[CanBeNull] params object[] args) =>
				Code.AssertArgument(condition, argName, messageFormat, args);

		/// <summary>
		/// State assertion
		/// </summary>
		[Conditional(DebugCondition), DebuggerHidden]
		[AssertionMethod]
		public static void AssertState(
			bool condition,
			[NotNull] string message) =>
				Code.AssertState(condition, message);

		/// <summary>
		/// State assertion
		/// </summary>
		[Conditional(DebugCondition), DebuggerHidden]
		[AssertionMethod, StringFormatMethod("messageFormat")]
		public static void AssertState(
			bool condition,
			[NotNull] string messageFormat,
			[CanBeNull] params object[] args) =>
				Code.AssertState(condition, messageFormat, args);
	}
}