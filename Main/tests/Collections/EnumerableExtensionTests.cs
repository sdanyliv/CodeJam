using NUnit.Framework;

namespace CodeJam
{
	[TestFixture]
	public class EnumerableExtensionTests
	{
		[Test]
		public void Concat()
		{
			Assert.AreEqual("1, 2, 3", new[] {"1", "2"}.Concat("3").Join(", "), "#A01");
			Assert.AreEqual("3", new string[0].Concat("3").Join(", "), "#A02");
			Assert.AreEqual("1, 2, 3, 4", new[] { "1", "2" }.Concat("3", "4").Join(", "), "#A03");
		}

		[Test]
		public void Prepend()
		{
			Assert.AreEqual("0, 1, 2", new[] { "1", "2" }.Prepend("0").Join(", "), "#A01");
			Assert.AreEqual("0", new string[0].Prepend("0").Join(", "), "#A02");
			Assert.AreEqual("-1, 0, 1, 2", new[] { "1", "2" }.Prepend("-1", "0").Join(", "), "#A03");
		}
	}
}