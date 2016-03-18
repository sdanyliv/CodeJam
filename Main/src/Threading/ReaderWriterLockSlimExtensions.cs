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
		public static ReadLockScope GetReadLock([NotNull] this ReaderWriterLockSlim readerWriterLock)
		{
			return new ReadLockScope(readerWriterLock);
		}

		/// <summary>
		/// Tries to enter the lock in write mode.
		/// </summary>
		/// <param name="readerWriterLock">The <see cref="ReaderWriterLockSlim"/> instance.</param>
		/// <returns>
		/// The <see cref="IDisposable"/> object that reduce the recursion count for write mode, and exits write mode if the resulting count is 0 (zero).
		/// </returns>
		[Pure]
		public static WriteLockScope GetWriteLock([NotNull] this ReaderWriterLockSlim readerWriterLock)
		{
			return new WriteLockScope(readerWriterLock);
		}

		/// <summary>
		/// Tries to enter the lock in upgradeable mode.
		/// </summary>
		/// <param name="readerWriterLock">The <see cref="ReaderWriterLockSlim"/> instance.</param>
		/// <returns>
		/// The <see cref="IDisposable"/> object that reduce the recursion count for upgradeable mode, and exits upgradeable mode if the resulting count is 0 (zero).
		/// </returns>
		[Pure]
		public static UpgradeableReadLockScope GetUpgradeableReadLock([NotNull] this ReaderWriterLockSlim readerWriterLock)
		{
			return new UpgradeableReadLockScope(readerWriterLock);
		}

		#region Inner type: ReadLockScope

		[EditorBrowsable(EditorBrowsableState.Never)]
		public struct ReadLockScope : IDisposable
		{
			private readonly ReaderWriterLockSlim _readerWriterLock;

			[DebuggerStepThrough]
			public ReadLockScope([NotNull] ReaderWriterLockSlim readerWriterLock)
			{
				_readerWriterLock = readerWriterLock;
				readerWriterLock.EnterReadLock();
			}

			[DebuggerStepThrough]
			public void Dispose() => _readerWriterLock.ExitReadLock();
		}

		#endregion

		#region Inner type: WriteLockScope

		[EditorBrowsable(EditorBrowsableState.Never)]
		public struct WriteLockScope : IDisposable
		{
			private readonly ReaderWriterLockSlim _readerWriterLock;

			[DebuggerStepThrough]
			public WriteLockScope([NotNull] ReaderWriterLockSlim readerWriterLock) {
				_readerWriterLock = readerWriterLock;
				readerWriterLock.EnterWriteLock();
			}

			[DebuggerStepThrough]
			public void Dispose() => _readerWriterLock.ExitWriteLock();
		}

		#endregion

		#region Inner type: UpgradeableReadLockScope

		[EditorBrowsable(EditorBrowsableState.Never)]
		public struct UpgradeableReadLockScope : IDisposable
		{
			private readonly ReaderWriterLockSlim _readerWriterLock;

			[DebuggerStepThrough]
			public UpgradeableReadLockScope([NotNull] ReaderWriterLockSlim readerWriterLock)
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
