using System.IO;

using NUnit.Framework;

namespace CodeJam.IO
{
	[TestFixture]
	public class TempHelperTests
	{
		[Test]
		public void TempDir()
		{
			string dirPath;
			using (var dir = TempHelper.GetTempDir())
			{
				dirPath = dir.Path;
				Assert.IsTrue(Directory.Exists(dirPath), "#A01");
			}
			Assert.IsFalse(Directory.Exists(dirPath), "#A02");

			using (var dir = TempHelper.GetTempDir())
			{
				dirPath = dir.Path;
				File.WriteAllText(Path.Combine(dirPath, "test.tmp"), "O La La");
			}
			Assert.IsFalse(Directory.Exists(dirPath), "#A03");

			using (var dir = TempHelper.GetTempDir())
			{
				dirPath = dir.Path;
				var subDir = Path.Combine(dirPath, "Subdir");
				Directory.CreateDirectory(subDir);
				File.WriteAllText(Path.Combine(subDir, "test.tmp"), "O La La");
			}
			Assert.IsFalse(Directory.Exists(dirPath), "#A04");
		}
	}
}