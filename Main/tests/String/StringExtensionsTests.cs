using NUnit.Framework;

namespace CodeJam
{
	[TestFixture]
	public class StringExtensionsTests
	{
		[TestCase((string)null, ExpectedResult = 0)]
		[TestCase("", ExpectedResult = 0)]
		[TestCase(" ", ExpectedResult = 1)]
		[TestCase("x", ExpectedResult = 1)]
		[TestCase("abc", ExpectedResult = 3)]
		public int Length(string source) => source.Length();

		[TestCase("", StringOrigin.Begin, 1, ExpectedResult = "")]
		[TestCase("", StringOrigin.End, 1, ExpectedResult = "")]
		[TestCase("abc", StringOrigin.Begin, 0, ExpectedResult = "")]
		[TestCase("abc", StringOrigin.End, 0, ExpectedResult = "")]
		[TestCase("abc", StringOrigin.Begin, 2, ExpectedResult = "ab")]
		[TestCase("abc", StringOrigin.End, 2, ExpectedResult = "bc")]
		[TestCase("abc", StringOrigin.Begin, 4, ExpectedResult = "abc")]
		[TestCase("abc", StringOrigin.End, 4, ExpectedResult = "abc")]
		public string SubstringOrg(string str, StringOrigin origin, int length) => str.Substring(origin, length);
	}
}