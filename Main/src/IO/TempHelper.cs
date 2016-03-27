using System;
using System.IO;

using JetBrains.Annotations;

namespace CodeJam.IO
{
	/// <summary>
	/// Methods to work with temporary data.
	/// </summary>
	[PublicAPI]
	public class TempHelper
	{
		/// <summary>
		/// Creates temp directory and returns <see cref="IDisposable"/> to free it.
		/// </summary>
		public static TempDir GetTempDir()
		{
			var dirName= new Guid() + ".tmp";
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
				catch { }
			}
		}
	}
}