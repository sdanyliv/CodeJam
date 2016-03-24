using System;

using NUnit.Framework;

namespace CodeJam.Threading
{
	[TestFixture]
	public class ThreadingExtensionsTests
	{
		[Test]
		public void Memoize()
		{
			var calls = 0;
			Func<string, int> func = str => { calls++; return str.Length; };
			var memFunc = func.Memoize();
			Assert.AreEqual(7, memFunc("O La La"), "#A01");
			Assert.AreEqual(1, calls, "#A02");
			Assert.AreEqual(7, memFunc("O La La"), "#A03");
			Assert.AreEqual(1, calls, "#A04");
			Assert.AreEqual(9, memFunc("O La La 2"), "#A05");
			Assert.AreEqual(2, calls, "#A06");
		}
	}
}