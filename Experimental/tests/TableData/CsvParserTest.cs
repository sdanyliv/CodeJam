using System;
using System.Linq;

using NUnit.Framework;

namespace CodeJam.TableData
{
	// TODO: replace to dump comparision
	[TestFixture]
	public class CsvParserTest
	{
		[Test]
		public void ParseNoEscaping()
		{
			const string csv = "abc ,def, ghi\r\n o_p_r,stu,vwx\r\n\"123\", \" 4 5 6 \",\"78 9\"";
			var result = TableDataParser.CreateCsvParser(false).Parse(csv).ToArray();

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

		[TestCase("", ExpectedResult = "")]
		[TestCase("a", ExpectedResult = "(1) a")]
		[TestCase("a,b", ExpectedResult = "(1) a, b")]
		[TestCase("a,b\r\nc,d", ExpectedResult = "(1) a, b; (2) c, d")]
		[TestCase("a,b\r\nc,d\r\ne,f", ExpectedResult = "(1) a, b; (2) c, d; (3) e, f")]
		[TestCase("a,b\r\n\r\nc,d\r\n   \r\ne,f", ExpectedResult = "(1) a, b; (3) c, d; (5) e, f")]
		[TestCase("a,\"\r\nb\"\r\nc,d", ExpectedResult = "(1) a, \r\nb; (3) c, d")]
		[TestCase(
			"abc, def, ghi   \r\no_p_r, stu, vwx\r\n\"123\", \" 4 5 6 \", \"78 \r\n9\"",
			ExpectedResult = "(1) abc, def, ghi; (2) o_p_r, stu, vwx; (3) 123,  4 5 6 , 78 \r\n9")]
		[TestCase("a\r\nb\r\nc\r\n\r\n\"\"", ExpectedResult = "(1) a; (2) b; (3) c; (5) ")]
		public string ParseCsv(string src) =>
			TableDataParser
				.CreateCsvParser()
				.Parse(src)
				.Select(l => l.ToString())
				.Join("; ");

		[TestCase("", ExpectedResult = "")]
		[TestCase("a", ExpectedResult = "(1) a")]
		[TestCase("a,b", ExpectedResult = "(1) a, b")]
		[TestCase("a,b\r\nc,d", ExpectedResult = "(1) a, b; (2) c, d")]
		[TestCase("a,b\r\nc,d\r\ne,f", ExpectedResult = "(1) a, b; (2) c, d; (3) e, f")]
		[TestCase("a,b\r\n\r\nc,d\r\n   \r\ne,f", ExpectedResult = "(1) a, b; (3) c, d; (5) e, f")]
		public string ParseCsvNoEscape(string src) =>
			TableDataParser
				.CreateCsvParser(false)
				.Parse(src)
				.Select(l => l.ToString())
				.Join("; ");
	}
}