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
		public static Disposable.AnonymousDisposable GetTempDir()
		{
			var dirName= new Guid() + ".tmp";
			var dirPath = Path.Combine(Path.GetTempPath(), dirName);
			Directory.CreateDirectory(dirPath);
			return
				Disposable.Create(
					() =>
					{
						try
						{
							Directory.Delete(dirPath, true);
						}
						// ReSharper disable once EmptyGeneralCatchClause
						catch {}
					});
		}
	}
}