// Based on the C version by Martin Pool.

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// String comparisons using a "natural order" algorithm.
	/// </summary>
	[PublicAPI]
	public class NaturalStringComparer : IComparer<string>
	{
		[NotNull] public static readonly NaturalStringComparer Comparer = new NaturalStringComparer();
		[NotNull] public static readonly Comparison<string> Comparision = Compare;

		/// <summary>End of line character</summary>
		private const char Eol = char.MinValue;

		private NaturalStringComparer() { }

		/// <summary>Compares numerical strings starting from non-zeroes</summary>
		private static int CompareNumerical(string a, string b, ref int ia, ref int ib)
		{
			var bias = 0;

			// The longest run of digits wins. That aside, the greatest
			// value wins, but we can't know that it will until we've scanned
			// both numbers to know that they have the same magnitude, so we
			// remember it in BIAS.
			for (; ; ia++, ib++)
			{
				var ca = CharAt(a, ia);
				var cb = CharAt(b, ib);

				var caIsDigit = ca.IsDigit();
				var cbIsDigit = cb.IsDigit();
				if (!caIsDigit)
				{
					if (cbIsDigit) // number in a is shorter than number in b
					{
						return -1;
					}
					return bias; // numbers have the same length
				}
				if (!cbIsDigit)
				{
					return +1; // number in b is shorter than number in a
				}

				if (bias != 0)
				{
					continue;
				}

				if (ca < cb)
				{
					bias = -1;
				}
				else if (ca > cb)
				{
					bias = +1;
				}
			}
		}

		public static int Compare([CanBeNull] string a, [CanBeNull] string b)
		{
			if (a == null && b == null) return 0;
			if (a == null) return -1;
			if (b == null) return 1;

			var ia = 0;
			var ib = 0;

			for (;;)
			{
				// only count the number of zeroes leading the last number compared
				var leadingZeroesCountA = SkipLeadingZeroesAndWhitespaces(a, ref ia);
				var leadingZeroesCountB = SkipLeadingZeroesAndWhitespaces(b, ref ib);

				var ca = CharAt(a, ia);
				var cb = CharAt(b, ib);

				// process run of digits
				if (ca.IsDigit() && cb.IsDigit())
				{
					var result = CompareNumerical(a, b, ref ia, ref ib);
					if (result != 0)
					{
						return result;
					}
					ca = CharAt(a, ia);
					cb = CharAt(b, ib);
				}

				if (ca < cb)
					return -1;
				if (ca > cb)
					return +1;

				if (ca == Eol && cb == Eol)
				{
					// The strings are equal. Perhaps the caller
					// will want to call strcmp to break the tie.

					// TODO: WTF??? I guess we should return 0 here!
					return leadingZeroesCountA - leadingZeroesCountB;
				}

				++ia;
				++ib;
			}
		}

		private static int SkipLeadingZeroesAndWhitespaces(string s, ref int i)
		{
			var leadingZeroes = 0;
			for (; ; ++i)
			{
				var c = CharAt(s, i);
				if (c.IsWhiteSpace())
				{
					// TODO: WTF???
					// "000 0001" and "0001" are equal according to this logic
					// Is it correct?
					leadingZeroes = 0;
					continue;
				}
				if (c != '0')
				{
					return leadingZeroes;
				}
				++leadingZeroes;
			}
		}

		private static char CharAt(string s, int i)
		{
			return i >= s.Length ? Eol : s[i];
		}

		int IComparer<string>.Compare(string x, string y)
		{
			return Compare(x, y);
		}
	}
}