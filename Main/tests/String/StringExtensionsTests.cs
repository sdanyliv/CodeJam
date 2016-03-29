using System.Globalization;

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

		[TestCase("",    StringOrigin.Begin, 1, ExpectedResult = "")]
		[TestCase("",    StringOrigin.End,   1, ExpectedResult = "")]
		[TestCase("abc", StringOrigin.Begin, 0, ExpectedResult = "")]
		[TestCase("abc", StringOrigin.End,   0, ExpectedResult = "")]
		[TestCase("abc", StringOrigin.Begin, 2, ExpectedResult = "ab")]
		[TestCase("abc", StringOrigin.End,   2, ExpectedResult = "bc")]
		[TestCase("abc", StringOrigin.Begin, 4, ExpectedResult = "abc")]
		[TestCase("abc", StringOrigin.End,   4, ExpectedResult = "abc")]
		public string SubstringOrg(string str, StringOrigin origin, int length) => str.Substring(origin, length);

		[TestCase("abc", null, ExpectedResult = "abc")]
		[TestCase("abc", "", ExpectedResult = "abc")]
		[TestCase("abc", "abcd", ExpectedResult = "abc")]
		[TestCase("abc", "ab", ExpectedResult = "c")]
		[TestCase("abc", "ac", ExpectedResult = "abc")]
		[TestCase("abc", "abc", ExpectedResult = "")]
		public string TrimPrefix(string str, string prefix) => str.TrimPrefix(prefix);

		[TestCase("abc", null, ExpectedResult = "abc")]
		[TestCase("abc", "", ExpectedResult = "abc")]
		[TestCase("abc", "abcd", ExpectedResult = "abc")]
		[TestCase("abc", "bc", ExpectedResult = "a")]
		[TestCase("abc", "ac", ExpectedResult = "abc")]
		[TestCase("abc", "abc", ExpectedResult = "")]
		public string TrimSuffix(string str, string suffix) => str.TrimSuffix(suffix);

		[TestCase(0, ExpectedResult = "0")]
		[TestCase(1, ExpectedResult = "1 bytes")]
		[TestCase(512, ExpectedResult = "512 bytes")]
		[TestCase(1023, ExpectedResult = "1 KB")]
		[TestCase(1024, ExpectedResult = "1 KB")]
		[TestCase(1025, ExpectedResult = "1 KB")]
		[TestCase(1536, ExpectedResult = "1.5 KB")]
		[TestCase(1590, ExpectedResult = "1.55 KB")]
		[TestCase(1048576, ExpectedResult = "1 MB")]
		[TestCase(1048576L << 10, ExpectedResult = "1 GB")]
		[TestCase(1048576L << 20, ExpectedResult = "1 TB")]
		[TestCase(1048576L << 30, ExpectedResult = "1 PB")]
		[TestCase(1048576L << 40, ExpectedResult = "1 EB")]
		public string ToByteSizeString(long value) => value.ToByteSizeString(CultureInfo.InvariantCulture);
	}
}