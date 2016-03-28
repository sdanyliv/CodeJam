using System;
using System.IO;

using JetBrains.Annotations;

namespace CodeJam.IO
{
	/// <summary>
	/// Methods to work with temporary data.
	/// </summary>
	[PublicAPI]
	public static class TempData
	{
		#region Dir
		/// <summary>
		/// Creates temp directory and returns <see cref="IDisposable"/> to free it.
		/// </summary>
		public static TempDir GetDir()
		{
			var dirName = new Guid() + ".tmp";
			var dirPath = Path.Combine(Path.GetTempPath(), dirName);
			Directory.CreateDirectory(dirPath);
			return new TempDir(dirPath);
		}

		/// <summary>
		/// Temp directory.
		/// </summary>
		[PublicAPI]
		public struct TempDir : IDisposable
		{
			/// <summary>
			/// Initialize instance.
			/// </summary>
			public TempDir(string path)
			{
				Path = path;
			}

			/// <summary>
			/// Path to directory.
			/// </summary>
			public string Path { get; }

			/// <summary>
			/// Delete directory and all its content.
			/// </summary>
			public void Dispose()
			{
				try
				{
					Directory.Delete(Path, true);
				}
					// ReSharper disable once EmptyGeneralCatchClause
				catch
				{
				}
			}
		}
		#endregion

		#region File
		/// <summary>
		/// Creates temp file and return disposable handle.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="dirPath"/> is null.</exception>
		public static TempFile GetFile([NotNull] string dirPath, [CanBeNull] string fileName = null)
		{
			if (dirPath == null) throw new ArgumentNullException(nameof(dirPath));
			if (fileName == null)
				fileName = new Guid() + ".tmp";
			var filePath = Path.Combine(dirPath, fileName);
			File.Create(filePath).Close();
			return new TempFile(filePath);
		}

		/// <summary>
		/// Creates temp file and return disposable handle.
		/// </summary>
		public static TempFile GetFile()
		{
			var dirPath = Path.GetTempPath();
			var fileName = new Guid() + ".tmp";
			var filePath = Path.Combine(dirPath, fileName);
			File.Create(filePath).Close();
			return new TempFile(filePath);
		}

		/// <summary>
		/// Temp file.
		/// </summary>
		[PublicAPI]
		public struct TempFile : IDisposable
		{
			/// <summary>
			/// Initialize instance.
			/// </summary>
			public TempFile(string path)
			{
				Path = path;
			}

			/// <summary>
			/// Path to file.
			/// </summary>
			public string Path { get; }
			public void Dispose()
			{
				try
				{
					File.Delete(Path);
				}
				// ReSharper disable once EmptyGeneralCatchClause
				catch {}
			}
		}
		#endregion

		#region Stream
		/// <summary>
		/// Creates stream and returns disposable handler.
		/// </summary>
		public static TempStream GetStream(FileAccess fileAccess = FileAccess.ReadWrite)
		{
			var path = Path.Combine(Path.GetTempPath(), new Guid() + ".tmp");
			var stream = new FileStream(path, FileMode.CreateNew, fileAccess);
			return new TempStream(path, stream);
		}

		/// <summary>
		/// Temp stream handle.
		/// </summary>
		[PublicAPI]
		public struct TempStream : IDisposable
		{
			/// <summary>
			/// Path to stream file.
			/// </summary>
			public string Path { get; }

			/// <summary>
			/// Initialize instance.
			/// </summary>
			public TempStream(string path, FileStream stream)
			{
				Path = path;
				Stream = stream;
			}

			/// <summary>
			/// Temp stream.
			/// </summary>
			public FileStream Stream { get; }

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			public void Dispose()
			{
				try
				{
					Stream.Dispose();
					File.Delete(Path);
				}
					// ReSharper disable once EmptyGeneralCatchClause
				catch {}
			}
		}
		#endregion
	}
}