﻿using System;
using System.IO;
using System.Linq;

using CodeJam.Strings;

using NUnit.Framework;

namespace CodeJam.TableData
{
	// TODO: replace to dump comparision
	[TestFixture]
	public class TableDataTest
	{
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
			CsvFormat
				.CreateParser()
				.Parse(src)
				.Select(l => l.ToString())
				.Join("; ");

		[TestCase("", ExpectedResult = "")]
		[TestCase("a", ExpectedResult = "(1) a")]
		[TestCase("a,b", ExpectedResult = "(1) a, b")]
		[TestCase("a,b\r\nc,d", ExpectedResult = "(1) a, b; (2) c, d")]
		[TestCase("a,b\r\nc,d\r\ne,f", ExpectedResult = "(1) a, b; (2) c, d; (3) e, f")]
		[TestCase("a,b\r\n\r\nc,d\r\n   \r\ne,f", ExpectedResult = "(1) a, b; (3) c, d; (5) e, f")]
		[TestCase(
			"abc ,def, ghi\r\n o_p_r,stu,vwx\r\n\"123\", \" 4 5 6 \",\"78 9\"",
			ExpectedResult = "(1) abc , def,  ghi; (2)  o_p_r, stu, vwx; (3) \"123\",  \" 4 5 6 \", \"78 9\"")]
		public string ParseCsvNoEscape(string src) =>
			CsvFormat
				.CreateParser(false)
				.Parse(src)
				.Select(l => l.ToString())
				.Join("; ");

		[TestCase("", ExpectedResult = "")]
		[TestCase("a", ExpectedResult = "  a")]
		[TestCase("a,b", ExpectedResult = "  a, b")]
		[TestCase("a,b\r\nc,d", ExpectedResult = "  a, b\r\n  c, d")]
		[TestCase("a,b\r\ncc,dd", ExpectedResult = "  a, b\r\n  cc, dd")]
		[TestCase("a,b\r\ncc,dd\r\ne,f", ExpectedResult = "  a, b\r\n  cc, dd\r\n   e,  f")]
		public string PrintCsv(string source)
		{
			var data = CsvFormat.CreateParser(true).Parse(source);
			var result = new StringWriter();
			CsvFormat
				.CreateFormatter()
				.Print(result, data.Select(l => l.Values), "  ");
			return result.ToString();
		}

		[TestCase("a", new[] {1}, ExpectedResult = "(1) a")]
		[TestCase("ab", new[] { 1 }, ExpectedResult = "(1) a")]
		[TestCase("ab", new[] { 2 }, ExpectedResult = "(1) ab")]
		[TestCase("ab\r\ncd", new[] { 2 }, ExpectedResult = "(1) ab; (2) cd")]
		[TestCase("ab\r\ncd", new[] { 1, 1 }, ExpectedResult = "(1) a, b; (2) c, d")]
		[TestCase(" abc", new[] { 2, 2 }, ExpectedResult = "(1) a, bc")]
		public string ParseFixedWidth(string source, int[] widths) =>
			FixedWidthFormat
				.CreateParser(widths)
				.Parse(source)
				.Select(l => l.ToString())
				.Join("; ");
	}
}