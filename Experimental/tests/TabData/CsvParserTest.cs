using System;
using System.IO;
using System.Linq;

using NUnit.Framework;

namespace CodeJam.TabData
{
	// TODO: replace to dump comparision
	[TestFixture]
	public class CsvParserTest
	{
		[Test]
		public void Parse()
		{
			const string csv = "abc ,def, ghi   \r\no_p_r, stu, vwx\r\n\"123\", \" 4 5 6 \", \"78 \r\n9\"";
			var result = CsvParser.Parse(new StringReader(csv)).ToArray();

			var expected = new[] { new[] { "abc", "def", "ghi" }, new[] { "o_p_r", "stu", "vwx" }, new[] { "123", " 4 5 6 ", "78 \r\n9" } };

			for (var line = 0; line < 3; line++)
				for (var i = 0; i < 3; i++)
					Assert.AreEqual(expected[line][i], result[line].Values[i], "#A01");
		}

		[Test]
		public void ParseNoEscaping()
		{
			const string csv = "abc ,def, ghi\r\n o_p_r,stu,vwx\r\n\"123\", \" 4 5 6 \",\"78 9\"";
			var result = CsvParser.Parse(new StringReader(csv), false).ToArray();

			var expected = new[]
			{
				new[] { "abc ", "def", " ghi" }, new[] { " o_p_r", "stu", "vwx" }, new[] { "\"123\"", " \" 4 5 6 \"", "\"78 9\"" }
			};

			for (var line = 0; line < 3; line++)
				for (var i = 0; i < 3; i++)
					Assert.AreEqual(expected[line][i], result[line].Values[i], "#A01");
		}

		[Test]
		public void Print()
		{
			var values =
				new[]
				{
					new []{"One", "Two", "Three"},
					new []{"Four ", " Five", " Six "},
					new []{"Se\"ven", "Eig,ht", "Ni\r\nne"}
				};
			var output = CsvPrinter.Print(values, "   ");
			Assert.AreEqual(
@"   One      , Two     , Three
   ""Four ""  , "" Five"" , "" Six ""
   ""Se""""ven"", ""Eig,ht"", ""Ni
ne""",
			output,
			"#A01");
		}

		[Test]
		public void NoValues()
		{
			const string csv = "a,b,c\r\nd,,f\r\n,h,";
			var lines = CsvParser.Parse(csv).ToArray();
			Assert.AreEqual(3, lines.Length, "#A01");
			Assert.AreEqual(3, lines[0].Values.Length, "#A02");
			Assert.AreEqual(3, lines[1].Values.Length, "#A03");
			Assert.AreEqual(3, lines[2].Values.Length, "#A04");

			var output = CsvPrinter.Print(lines.Select(l => l.Values).ToArray(), "   ");
			Assert.AreEqual(
@"   a, b, c
   d,  , f
    , h, ",
			output,
			"#A05");
		}

		[Test]
		public void SingleColumn()
		{
			const string csv = "a\r\nb\r\nc\r\n\r\n\"\"";
			var result = CsvParser.Parse(csv).ToArray();
			var expected = new[] { new[] { "a" }, new[] { "b" }, new[] { "c" }, new[] { "" } };

			Assert.AreEqual(4, result.Length, "#A01");
			for (var line = 0; line < result.Length; line++)
			{
				Assert.AreEqual(1, result[line].Values.Length);
				Assert.AreEqual(expected[line][0], result[line].Values[0], "#A02");
			}
		}

		[TestCase("", ExpectedResult = "")]
		[TestCase("a", ExpectedResult = "(1) a")]
		[TestCase("a,b", ExpectedResult = "(1) a, b")]
		[TestCase("a,b\r\nc,d", ExpectedResult = "(1) a, b; (2) c, d")]
		[TestCase("a,b\r\nc,d\r\ne,f", ExpectedResult = "(1) a, b; (2) c, d; (3) e, f")]
		[TestCase("a,b\r\n\r\nc,d\r\n   \r\ne,f", ExpectedResult = "(1) a, b; (3) c, d; (5) e, f")]
		[TestCase("a,\"\r\nb\"\r\nc,d", ExpectedResult = "(1) a, \r\nb; (3) c, d")]
		public string Dump(string src)
		{
			var parsed = CsvParser.Parse(src);
			return parsed.Select(l => l.ToString()).Join("; ");
		}
	}
}