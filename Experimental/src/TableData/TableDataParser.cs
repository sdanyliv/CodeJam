using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using CodeJam.Collections;

using JetBrains.Annotations;

namespace CodeJam.TableData
{
	/// <summary>
	/// 
	/// </summary>
	[PublicAPI]
	public static class TableDataParser
	{
		/// <summary>
		/// Reads single line from table data and parses it.
		/// </summary>
		/// <param name="reader"><see cref="TextReader"/> to read data from</param>
		/// <param name="lineNum">current number of line</param>
		/// <returns>
		/// Null, if end of file reached, string[0] if line contains no valued, or array of values.
		/// </returns>
		[CanBeNull]
		public delegate string[] Parser([NotNull] TextReader reader, ref int lineNum);

		/// <summary>
		/// Parses table data.
		/// </summary>
		/// <param name="parser">Instance of specific parser.</param>
		/// <param name="text">Text to parse</param>
		/// <returns>Enumeration of <see cref="DataLine"/> contained parsed data.</returns>
		[NotNull]
		public static IEnumerable<DataLine> Parse([NotNull] this Parser parser, [NotNull] string text)
		{
			if (text == null) throw new ArgumentNullException(nameof(text));
			return Parse(parser, new StringReader(text));
		}

		/// <summary>
		/// Parses table data.
		/// </summary>
		/// <param name="reader">Text to parse</param>
		/// <param name="parser">Instance of specific parser.</param>
		/// <returns>Enumeration of <see cref="DataLine"/> contained parsed data.</returns>
		[NotNull]
		public static IEnumerable<DataLine> Parse([NotNull] this Parser parser, [NotNull] TextReader reader)
		{
			if (parser == null) throw new ArgumentNullException(nameof(parser));
			if (reader == null) throw new ArgumentNullException(nameof(reader));

			var lineNum = 1;
			while (true)
			{
				var lastLineNum = lineNum;
				var values = parser(reader, ref lineNum);
				if (values == null)
					yield break;
				if (values.Length > 0) // Skip empty lines
					yield return new DataLine(lastLineNum, values);
			}
		}

		/// <summary>
		/// Creates RFC4180 compliant CSV parser.
		/// </summary>
		/// <param name="allowEscaping">If true, allows values escaping.</param>
		/// <returns>Parser to use with <see cref="Parse(TableDataParser.Parser,string)"/></returns>
		public static Parser CreateCsvParser(bool allowEscaping = true) =>
			allowEscaping ? ParseCsv : (Parser)ParseCsvNoEscaping;

		[CanBeNull]
		private static string[] ParseCsvNoEscaping(TextReader reader, ref int lineNum)
		{
			var line = reader.ReadLine();
			if (line == null)
				return null;
			lineNum++;
			var parts = line.Split(',');
			// Special case - whitespace lines are ignored
			if (parts.Length == 1 && parts[0].IsNullOrWhiteSpace())
				return Array<string>.Empty;
			return parts;
		}

		[CanBeNull]
		private static string[] ParseCsv(TextReader reader, ref int lineNum)
		{
			var curChar = CharReader.Create(reader);
			if (curChar.IsEof)
				return null; // EOF reached
			if (curChar.IsEol)
			{
				lineNum++;
				return Array<string>.Empty;
			}

			var result = new List<string>();
			StringBuilder curField = null;
			var state = ParserState.ExpectField;
			while (true)
			{
				var skip = false;

				while (!skip)
					switch (state)
					{
						case ParserState.ExpectField:
							if (curChar.IsEof || curChar.IsEol)
							{
								if (result.Count > 0) // Special case - empty string not treated as single empty value
									result.Add("");
								return result.ToArray();
							}

							if (curChar.IsComma)
							{
								result.Add("");
								state = ParserState.AfterField;
								break;
							}

							skip = true;
							if (curChar.IsWhitespace)
								break;

							curField = new StringBuilder();

							if (curChar.IsDoubleQuota)
							{
								state = ParserState.QuotedField;
								break;
							}

							state = ParserState.Field;
							curField.Append(curChar.Char);
							break;

						case ParserState.Field:
							Debug.Assert(curField != null, "curField != null");
							if (curChar.IsEof || curChar.IsEol || curChar.IsComma)
							{
								result.Add(curField.ToString().Trim());
								state = ParserState.AfterField;
								break;
							}

							skip = true;
							curField.Append(curChar.Char);
							break;

						case ParserState.QuotedField:
							Debug.Assert(curField != null, "curField != null");
							if (curChar.IsEof)
								throw new FormatException("Unexpected EOF");

							skip = true;
							if (curChar.IsEol)
							{
								curField.Append("\r\n");
								break;
							}
							if (curChar.IsDoubleQuota)
							{
								var peek = curChar.Peek();
								if (peek.IsDoubleQuota) // Escaped '"'
								{
									curField.Append('"');
									curChar = curChar.Next();
								}
								else
								{
									result.Add(curField.ToString());
									state = ParserState.AfterField;
								}
								break;
							}
							curField.Append(curChar.Char);
							break;

						case ParserState.AfterField:
							if (curChar.IsEof || curChar.IsEol)
								return result.ToArray();
							skip = true;
							if (curChar.IsWhitespace)
								continue;
							if (curChar.IsComma)
							{
								state = ParserState.ExpectField;
								break;
							}
							throw new FormatException($"Unexpected char '{curChar.Char}' at line {lineNum}");

						default:
							throw new ArgumentOutOfRangeException();
					}

				curChar = curChar.Next();
				if (curChar.IsEol)
					lineNum++;
			}
		}

		#region CharReader struct
		private struct CharReader
		{
			private const int s_Eof = -1;
			private const int s_Eol = -2;

			private readonly TextReader m_Reader;
			private readonly int m_Code;

			private CharReader(TextReader reader, int code)
			{
				m_Reader = reader;
				m_Code = code;
			}

			public char Char => (char)m_Code;

			public bool IsEof => m_Code == s_Eof;

			public bool IsEol => m_Code == s_Eol;

			public bool IsWhitespace => char.IsWhiteSpace(Char);

			public bool IsComma => m_Code == ',';

			public bool IsDoubleQuota => m_Code == '"';

			private static int Read(TextReader reader)
			{
				var code = reader.Read();
				if (code == '\r' || code == '\n')
				{
					if (code == '\r' && reader.Peek() == '\n')
						reader.Read();
					return s_Eol;
				}
				return code;
			}

			public static CharReader Create(TextReader reader) => new CharReader(reader, Read(reader));

			public CharReader Next() => Create(m_Reader);

			public CharReader Peek() => new CharReader(m_Reader, m_Reader.Peek());
		}
		#endregion

		#region ParserState enum
		private enum ParserState
		{
			ExpectField,
			Field,
			QuotedField,
			AfterField
		}
		#endregion
	}
}