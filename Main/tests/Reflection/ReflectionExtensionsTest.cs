using System.IO;

using NUnit.Framework;

namespace CodeJam.Reflection
{
	[TestFixture]
	public class ReflectionExtensionsTest
	{
		[Test]
		public void GetAsmPath() =>
			Assert.AreEqual("CodeJam-Tests.DLL", Path.GetFileName(GetType().Assembly.GetAssemblyPath()));
	}
}