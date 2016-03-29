using System;
using System.IO;

using NUnit.Framework;

namespace CodeJam.Reflection
{
	[TestFixture]
	public class AssemblyExtensionsTest
	{
		[Test]
		public void GetAsmPath() =>
			Assert.AreEqual("CodeJam-Tests.DLL", Path.GetFileName(GetType().Assembly.GetAssemblyPath()));
	}
}
