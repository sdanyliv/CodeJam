using System.Collections.Generic;

using NUnit.Framework;

namespace CodeJam
{
	[TestFixture]
	public class DictionaryExtensionsTest
	{
		[Test]
		public static void GetOrAdd()
		{
			var dic = new Dictionary<int, string>();

			var v1 = dic.GetOrAdd(1, "1");
			Assert.AreEqual(1, dic.Count, "#A01");
			Assert.AreEqual("1", dic[1], "#A02");
			Assert.AreEqual("1", v1, "#A03");

			var v2 = dic.GetOrAdd(1, "2");
			Assert.AreEqual(1, dic.Count, "#A04");
			Assert.AreEqual("1", dic[1], "#A05");
			Assert.AreEqual("1", v2, "#A06");
		}

		[Test]
		public static void GetOrAddLazy()
		{
			var dic = new Dictionary<int, string>();
			var calls = 0;

			var v1 = dic.GetOrAdd(1, k => { calls++; return "1"; });
			Assert.AreEqual(1, dic.Count, "#A01");
			Assert.AreEqual("1", dic[1], "#A02");
			Assert.AreEqual("1", v1, "#A03");
			Assert.AreEqual(1, calls, "#A07");

			var v2 = dic.GetOrAdd(1, k => { calls++; return "2"; });
			Assert.AreEqual(1, dic.Count, "#A04");
			Assert.AreEqual("1", dic[1], "#A05");
			Assert.AreEqual("1", v2, "#A06");
			Assert.AreEqual(1, calls, "#A08");
		}
	}
}