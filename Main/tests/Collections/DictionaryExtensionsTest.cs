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

		[Test]
		public static void AddOrUpdate()
		{
			var dic = new Dictionary<int, string>();

			var v1 = dic.AddOrUpdate(1, "1", (k, v) => v + "1");
			Assert.AreEqual(1, dic.Count, "#A01");
			Assert.AreEqual("1", dic[1], "#A02");
			Assert.AreEqual("1", v1, "#A03");

			var v2 = dic.AddOrUpdate(1, "2", (k, v) => v + "1");
			Assert.AreEqual(1, dic.Count, "#A04");
			Assert.AreEqual("11", dic[1], "#A05");
			Assert.AreEqual("11", v2, "#A06");
		}

		[Test]
		public static void AddOrUpdateLazy()
		{
			var dic = new Dictionary<int, string>();
			var addCalls = 0;
			var updateCalls = 0;

			var v1 = dic.AddOrUpdate(1, k => { addCalls++; return "1"; }, (k, v) => { updateCalls++; return v + "1"; });
			Assert.AreEqual(1, dic.Count, "#A01");
			Assert.AreEqual("1", dic[1], "#A02");
			Assert.AreEqual("1", v1, "#A03");
			Assert.AreEqual(1, addCalls, "#A07");
			Assert.AreEqual(0, updateCalls, "#A08");

			var v2 = dic.AddOrUpdate(1, k => { addCalls++; return "2"; }, (k, v) => { updateCalls++; return v + "1"; });
			Assert.AreEqual(1, dic.Count, "#A04");
			Assert.AreEqual("11", dic[1], "#A05");
			Assert.AreEqual("11", v2, "#A06");
			Assert.AreEqual(1, addCalls, "#A09");
			Assert.AreEqual(1, updateCalls, "#A10");
		}
	}
}