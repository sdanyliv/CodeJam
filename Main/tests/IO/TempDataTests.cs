using System.IO;

using NUnit.Framework;

namespace CodeJam.IO
{
	[TestFixture]
	public class TempDataTests
	{
		[Test]
		public void Dir()
		{
			string dirPath;
			using (var dir = TempData.GetDir())
			{
				dirPath = dir.Path;
				Assert.IsTrue(Directory.Exists(dirPath), "#A01");
			}
			Assert.IsFalse(Directory.Exists(dirPath), "#A02");

			using (var dir = TempData.GetDir())
			{
				dirPath = dir.Path;
				File.WriteAllText(Path.Combine(dirPath, "test.tmp"), "O La La");
			}
			Assert.IsFalse(Directory.Exists(dirPath), "#A03");

			using (var dir = TempData.GetDir())
			{
				dirPath = dir.Path;
				var subDir = Path.Combine(dirPath, "Subdir");
				Directory.CreateDirectory(subDir);
				File.WriteAllText(Path.Combine(subDir, "test.tmp"), "O La La");
			}
			Assert.IsFalse(Directory.Exists(dirPath), "#A04");
		}

		[Test]
		public void TmpFile()
		{
			string path;
			using (var fl = TempData.GetFile())
			{
				path = fl.Path;
				Assert.IsTrue(File.Exists(path), "#A01");
			}
			Assert.IsFalse(File.Exists(path), "#A02");
		}

		[Test]
		public void Stream()
		{
			string path;
			using (var stream = TempData.GetStream())
			{
				new StreamWriter(stream.Stream).Write("O La La");
				path = stream.Path;
				Assert.IsTrue(File.Exists(path), "#A01");
			}
			Assert.IsFalse(File.Exists(path), "#A02");
		}
	}
}