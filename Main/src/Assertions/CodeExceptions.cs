using System;
using System.Diagnostics;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// Exception factory class
	/// </summary>
	[PublicAPI]
	public static class CodeExceptions
	{
		#region Behavior setup and implementation helpers
		/// <summary>
		/// Init the default assertion handling behavior
		/// </summary>
		// ReSharper disable once EmptyConstructor
		static CodeExceptions()
		{
			// Break on exceptions: debug builds only
#if DEBUG
			BreakOnException = true;
#endif
		}

		/// <summary>
		/// Enables Break execution on assertion failure feature
		/// </summary>
		public static bool BreakOnException { get; set; }

		/// <summary>
		/// BreaksExecution if debugger attached
		/// </summary>
		[DebuggerHidden]
		public static void BreakIfAttached()
		{
			if (BreakOnException && Debugger.IsAttached)
				Debugger.Break();
		}

		/// <summary>
		/// Formats message or returs <paramref name="messageFormat"/> as it is if <paramref name="args"/> are null or empty
		/// </summary>
		[StringFormatMethod("messageFormat")]
		[NotNull]
		private static string FormatMessage([NotNull] string messageFormat, [CanBeNull] params object[] args) =>
			// ReSharper disable ArrangeRedundantParentheses
			(args == null || args.Length == 0) ? messageFormat : string.Format(messageFormat, args);
			// ReSharper restore ArrangeRedundantParentheses
		#endregion

		#region General purpose exceptions
		/// <summary>
		/// Creates <seealso cref="ArgumentNullException"/>
		/// </summary>
		[DebuggerHidden]
		[NotNull]
		public static ArgumentNullException ArgumentNull([NotNull] [InvokerParameterName] string argumentName)
		{
			BreakIfAttached();
			return new ArgumentNullException(argumentName);
		}

		/// <summary>
		/// Creates <seealso cref="ArgumentException"/> for empty string
		/// </summary>
		[DebuggerHidden]
		[NotNull]
		public static ArgumentException ArgumentNullOrEmpty([NotNull] [InvokerParameterName] string argumentName)
		{
			BreakIfAttached();
			return new ArgumentException($"String '{argumentName}' should be neither null or empty", argumentName);
		}

		/// <summary>
		/// Creates <seealso cref="ArgumentException"/>
		/// </summary>
		[DebuggerHidden]
		[StringFormatMethod("messageFormat")]
		[NotNull]
		public static ArgumentException Argument(
			[NotNull] [InvokerParameterName] string argumentName,
			[NotNull] string messageFormat,
			[CanBeNull] params object[] args)
		{
			BreakIfAttached();
			return new ArgumentException(FormatMessage(messageFormat, args), argumentName);
		}

		/// <summary>
		/// Creates <seealso cref="InvalidOperationException"/>
		/// </summary>
		[DebuggerHidden]
		[StringFormatMethod("messageFormat")]
		[NotNull]
		public static InvalidOperationException InvalidOperation([NotNull] string messageFormat, [CanBeNull] params object[] args)
		{
			BreakIfAttached();
			return new InvalidOperationException(FormatMessage(messageFormat, args));
		}
		#endregion

		#region Exceptions for specific scenarios
		/// <summary>
		/// Creates <seealso cref="InvalidOperationException"/>.
		/// Used to be thrown from the default: switch clause
		/// </summary>
		[DebuggerHidden]
		[NotNull]
		public static InvalidOperationException UnexpectedValue([CanBeNull] object value)
		{
			BreakIfAttached();
			return
				new InvalidOperationException($"Unexpected value '{value}' of type '{value?.GetType().FullName ?? "<unknown>"}'");
		}

		/// <summary>
		/// Creates <seealso cref="InvalidOperationException"/>.
		/// Used to be thrown from default: switch clause
		/// </summary>
		[DebuggerHidden]
		[StringFormatMethod("messageFormat")]
		[NotNull]
		public static InvalidOperationException UnexpectedValue([NotNull] string messageFormat, [CanBeNull] params object[] args)
		{
			BreakIfAttached();
			return new InvalidOperationException(FormatMessage(messageFormat, args));
		}

		/// <summary>
		/// Used to be thrown in places expected to be unreachable.
		/// </summary>
		[DebuggerHidden]
		[StringFormatMethod("messageFormat")]
		[NotNull]
		public static NotSupportedException Unreachable([NotNull] string messageFormat, [CanBeNull] params object[] args)
		{
			BreakIfAttached();
			return new NotSupportedException(FormatMessage(messageFormat, args));
		}
		#endregion
	}
}