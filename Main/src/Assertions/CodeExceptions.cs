using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

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
		/// If true, breaks execution on assertion failure.
		/// Enabled by default.
		/// </summary>
		public static bool BreakOnException { get; set; } = true;

		/// <summary>
		/// BreaksExecution if debugger attached
		/// </summary>
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void BreakIfAttached()
		{
			if (BreakOnException && Debugger.IsAttached)
				Debugger.Break();
		}

		/// <summary>
		/// Formats message or returns <paramref name="messageFormat"/> as it is if <paramref name="args"/> are null or empty
		/// </summary>
		[StringFormatMethod("messageFormat")]
		[NotNull]
		[SuppressMessage("ReSharper", "ArrangeRedundantParentheses")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static string FormatMessage([NotNull] string messageFormat, [CanBeNull] params object[] args) =>
			(args == null || args.Length == 0) ? messageFormat : string.Format(messageFormat, args);
		#endregion

		#region General purpose exceptions
		/// <summary>
		/// Creates <seealso cref="ArgumentNullException"/>
		/// </summary>
		[DebuggerHidden]
		[NotNull]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static InvalidOperationException InvalidOperation(
			[NotNull] string messageFormat, [CanBeNull] params object[] args)
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static InvalidOperationException UnexpectedValue<T>([CanBeNull] T value)
		{
			BreakIfAttached();
			var valueType = value?.GetType() ?? typeof(T);
			return new InvalidOperationException($"Unexpected value '{value}' of type '{valueType.FullName}'");
		}

		/// <summary>
		/// Creates <seealso cref="InvalidOperationException"/>.
		/// Used to be thrown from default: switch clause
		/// </summary>
		[DebuggerHidden]
		[StringFormatMethod("messageFormat")]
		[NotNull]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static InvalidOperationException UnexpectedValue(
			[NotNull] string messageFormat, [CanBeNull] params object[] args)
		{
			BreakIfAttached();
			return new InvalidOperationException(FormatMessage(messageFormat, args));
		}

		/// <summary>
		/// Throw this if the object is disposed.
		/// </summary>
		[DebuggerHidden]
		[StringFormatMethod("messageFormat")]
		[NotNull]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ObjectDisposedException ObjectDisposed([CanBeNull] Type typeofDisposedObject)
		{
			BreakIfAttached();
			return new ObjectDisposedException(typeofDisposedObject?.FullName);
		}

		/// <summary>
		/// Throw this if the object is disposed.
		/// </summary>
		[DebuggerHidden]
		[StringFormatMethod("messageFormat")]
		[NotNull]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ObjectDisposedException ObjectDisposed(
			[CanBeNull] Type typeofDisposedObject, [NotNull] string messageFormat, [CanBeNull] params object[] args)
		{
			BreakIfAttached();
			return new ObjectDisposedException(typeofDisposedObject?.FullName, FormatMessage(messageFormat, args));
		}

		/// <summary>
		/// Used to be thrown in places expected to be unreachable.
		/// </summary>
		[DebuggerHidden]
		[StringFormatMethod("messageFormat")]
		[NotNull]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static NotSupportedException Unreachable([NotNull] string messageFormat, [CanBeNull] params object[] args)
		{
			BreakIfAttached();
			return new NotSupportedException(FormatMessage(messageFormat, args));
		}
		#endregion
	}
}