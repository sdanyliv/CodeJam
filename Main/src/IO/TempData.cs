﻿using System;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Threading;

using JetBrains.Annotations;

namespace CodeJam.IO
{
	/// <summary>
	/// Methods to work with temporary data.
	/// </summary>
	[PublicAPI]
	public static class TempData
	{
		#region Temp file|directory holders
		/// <summary>
		/// Base class for temp file|directory objects.
		/// Contains logic to proof the removal will be performed even on resource leak.
		/// </summary>
		[PublicAPI]
		public abstract class TempBase : CriticalFinalizerObject, IDisposable
		{
			private volatile string _path;

			/// <summary>
			/// Assertion on object dispose
			/// </summary>
			protected void AssertNotDisposed() => Code.DisposedIfNull(_path, this);

			/// <summary>
			/// Initialize instance.
			/// </summary>
			protected TempBase(string path)
			{
				Code.NotNullNorEmpty(path, nameof(path));

				_path = path;
			}

			/// <summary>
			/// Temp path.
			/// </summary>
			[NotNull]
			public string Path
			{
				get
				{
					AssertNotDisposed();
					return _path;
				}
			}

			/// <summary>
			/// Finalize instance
			/// </summary>
			~TempBase()
			{
				Dispose(false);
			}

			/// <summary>
			/// Delete the temp file|directory
			/// </summary>
			public void Dispose()
			{
				if (_path != null) // Fast check
				{
					Dispose(true);

					// it's safe to call SuppressFinalize multiple times so it's ok if check above will be inaccurate.
					GC.SuppressFinalize(this);
				}
			}

			/// <summary>
			/// Dispose pattern implementation - overridable part
			/// </summary>
			protected void Dispose(bool disposing)
			{
#pragma warning disable 420
				var path = Interlocked.Exchange(ref _path, null);
#pragma warning restore 420
				if (path == null)
					return;

				DisposePath(path, disposing);
			}

			/// <summary>
			/// Temp path disposal
			/// </summary>
			protected abstract void DisposePath(string path, bool disposing);
		}

		/// <summary>
		/// Wraps reference on a temp directory meant to be deleted on dispose
		/// </summary>
		[PublicAPI]
		public sealed class TempDirectory : TempBase
		{
			private DirectoryInfo _info;

			/// <summary>
			/// Initialize instance.
			/// </summary>
			public TempDirectory(string path) : base(path)
			{
			}

			/// <summary>
			/// DirectoryInfo object
			/// </summary>
			[NotNull]
			public DirectoryInfo Info
			{
				get
				{
					AssertNotDisposed();
					return _info ?? (_info = new DirectoryInfo(Path));
				}
			}

			/// <summary>
			/// Temp path disposal
			/// </summary>
			protected override void DisposePath(string path, bool disposing)
			{
				_info = null;

				try
				{
					Directory.Delete(path, true);
				}
				catch (ArgumentException) { }
				catch (IOException) { }
				catch (UnauthorizedAccessException) { }
			}
		}

		/// <summary>
		/// Wraps reference on a temp file meant to be deleted on dispose
		/// </summary>
		[PublicAPI]
		public sealed class TempFile : TempBase
		{
			private FileInfo _info;

			/// <summary>
			/// Initialize instance.
			/// </summary>
			public TempFile(string path) : base(path)
			{
			}

			/// <summary>
			/// DirectoryInfo object
			/// </summary>
			[NotNull]
			public FileInfo Info
			{
				get
				{
					AssertNotDisposed();
					return _info ?? (_info = new FileInfo(Path));
				}
			}

			/// <summary>
			/// Temp path disposal
			/// </summary>
			protected override void DisposePath(string path, bool disposing)
			{
				_info = null;

				try
				{
					File.Delete(path);
				}
				catch (ArgumentException) { }
				catch (IOException) { }
				catch (UnauthorizedAccessException) { }
			}
		}
		#endregion

		#region Factory methods
		private static string GetTempName() => Guid.NewGuid() + ".tmp";

		/// <summary>
		/// Creates temp directory and returns <see cref="IDisposable"/> to free it.
		/// </summary>
		public static TempDirectory CreateDirectory()
		{
			var directoryPath = Path.Combine(Path.GetTempPath(), GetTempName());
			Directory.CreateDirectory(directoryPath);
			return new TempDirectory(directoryPath);
		}

		/// <summary>
		/// Creates temp file and return disposable handle.
		/// </summary>
		public static TempFile CreateFile() => CreateFile(Path.GetTempPath());

		/// <summary>
		/// Creates temp file and return disposable handle.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="dirPath"/> is null.</exception>
		public static TempFile CreateFile([NotNull] string dirPath, [CanBeNull] string fileName = null)
		{
			Code.NotNull(dirPath, nameof(dirPath));

			if (fileName == null)
				fileName = GetTempName();

			var filePath = Path.Combine(dirPath, fileName);
			File.Create(filePath).Close();
			return new TempFile(filePath);
		}

		/// <summary>
		/// Creates stream and returns disposable handler.
		/// </summary>
		public static FileStream CreateFileStream(FileAccess fileAccess = FileAccess.ReadWrite) =>
			CreateFileStream(Path.GetTempPath(), null, fileAccess);

		/// <summary>
		/// Creates stream and returns disposable handler.
		/// </summary>
		public static FileStream CreateFileStream(
			[NotNull] string dirPath, [CanBeNull] string fileName = null, FileAccess fileAccess = FileAccess.ReadWrite)
		{
			const int bufferSize = 4096;

			Code.NotNull(dirPath, nameof(dirPath));

			if (fileName == null)
				fileName = GetTempName();

			var filePath = Path.Combine(dirPath, fileName);
			return new FileStream(
				filePath, FileMode.CreateNew,
				fileAccess, FileShare.Read, bufferSize,
				FileOptions.DeleteOnClose);
		}
		#endregion
	}
}