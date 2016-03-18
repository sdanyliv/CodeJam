using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// Represents the extenion methods for <see cref="ReaderWriterLockSlim"/>.
	/// </summary>
	[PublicAPI]
	public static class ReaderWriterLockSlimExtensions
	{
		/// <summary>
		/// Tries to enter the lock in read mode.
		/// </summary>
		/// <param name="readerWriterLock">The <see cref="ReaderWriterLockSlim"/> instance.</param>
		/// <returns>
		/// The <see cref="IDisposable"/> object that reduce the recursion count for read mode, and exits read mode if the resulting count is 0 (zero).
		/// </returns>
		[Pure]
		public static ReadLockInternal GetReadLock([NotNull] this ReaderWriterLockSlim readerWriterLock)
		{
			return new ReadLockInternal(readerWriterLock);
		}

		/// <summary>
		/// Tries to enter the lock in write mode.
		/// </summary>
		/// <param name="readerWriterLock">The <see cref="ReaderWriterLockSlim"/> instance.</param>
		/// <returns>
		/// The <see cref="IDisposable"/> object that reduce the recursion count for write mode, and exits write mode if the resulting count is 0 (zero).
		/// </returns>
		[Pure]
		public static WriteLockInternal GetWriteLock([NotNull] this ReaderWriterLockSlim readerWriterLock)
		{
			return new WriteLockInternal(readerWriterLock);
		}

		/// <summary>
		/// Tries to enter the lock in upgradeable mode.
		/// </summary>
		/// <param name="readerWriterLock">The <see cref="ReaderWriterLockSlim"/> instance.</param>
		/// <returns>
		/// The <see cref="IDisposable"/> object that reduce the recursion count for upgradeable mode, and exits upgradeable mode if the resulting count is 0 (zero).
		/// </returns>
		[Pure]
		public static UpgradeableReadLockInternal GetUpgradeableReadLock([NotNull] this ReaderWriterLockSlim readerWriterLock)
		{
			return new UpgradeableReadLockInternal(readerWriterLock);
		}

		#region Inner type: ReadLockInternal

		[EditorBrowsable(EditorBrowsableState.Never)]
		public struct ReadLockInternal : IDisposable
		{
			private readonly ReaderWriterLockSlim _readerWriterLock;

			[DebuggerStepThrough]
			public ReadLockInternal([NotNull] ReaderWriterLockSlim readerWriterLock)
			{
				_readerWriterLock = readerWriterLock;
				readerWriterLock.EnterReadLock();
			}

			[DebuggerStepThrough]
			public void Dispose() => _readerWriterLock.ExitReadLock();
		}

		#endregion

		#region Inner type: WriteLockInternal

		[EditorBrowsable(EditorBrowsableState.Never)]
		public struct WriteLockInternal : IDisposable
		{
			private readonly ReaderWriterLockSlim _readerWriterLock;

			[DebuggerStepThrough]
			public WriteLockInternal([NotNull] ReaderWriterLockSlim readerWriterLock) {
				_readerWriterLock = readerWriterLock;
				readerWriterLock.EnterWriteLock();
			}

			[DebuggerStepThrough]
			public void Dispose() => _readerWriterLock.ExitWriteLock();
		}

		#endregion

		#region Inner type: WriteLockInternal

		[EditorBrowsable(EditorBrowsableState.Never)]
		public struct UpgradeableReadLockInternal : IDisposable
		{
			private readonly ReaderWriterLockSlim _readerWriterLock;

			[DebuggerStepThrough]
			public UpgradeableReadLockInternal([NotNull] ReaderWriterLockSlim readerWriterLock)
			{
				_readerWriterLock = readerWriterLock;
				readerWriterLock.EnterUpgradeableReadLock();
			}

			[DebuggerStepThrough]
			public void Dispose() => _readerWriterLock.ExitUpgradeableReadLock();
		}

		#endregion
	}
}
