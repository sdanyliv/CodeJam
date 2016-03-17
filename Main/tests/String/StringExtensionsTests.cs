using NUnit.Framework;

namespace CodeJam
{
	[TestFixture]
	public class StringExtensionsTests
	{
		[Test]
		public void Length()
		{
			Assert.AreEqual(0, ((string)null).Length(), "#A01");
			Assert.AreEqual(0, "".Length(), "#A02");
			Assert.AreEqual(1, " ".Length(), "#A03");
			Assert.AreEqual(1, "x".Length(), "#A04");
		}
	}
}