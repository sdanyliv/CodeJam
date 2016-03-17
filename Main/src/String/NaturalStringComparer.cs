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

		private NaturalStringComparer(){}

		private static int CompareRight(string a, string b)
		{
			var bias = 0;
			var ia = 0;
			var ib = 0;

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
				if (!caIsDigit && !cbIsDigit)
					return bias;
				if (!caIsDigit)
					return -1;
				if (!cbIsDigit)
					return +1;
				if (ca < cb)
				{
					if (bias == 0)
						bias = -1;
				}
				else if (ca > cb)
				{
					if (bias == 0)
						bias = +1;
				}
				else if (ca == 0 && cb == 0)
					return bias;
			}
		}

		public static int Compare([CanBeNull] string a, [CanBeNull] string b)
		{
			if (a == null && b == null) return 0;
			if (a == null) return -1;
			if (b == null) return 1;

			int ia = 0, ib = 0;

			while (true)
			{
				// only count the number of zeroes leading the last number compared
				var nzb = 0;
				var nza = 0;

				var ca = CharAt(a, ia);
				var cb = CharAt(b, ib);

				// skip over leading spaces or zeros
				while (ca.IsWhiteSpace() || ca == '0')
				{
					if (ca == '0')
						nza++;
					else
					// only count consecutive zeroes
						nza = 0;

					ca = CharAt(a, ++ia);
				}

				while (cb.IsWhiteSpace() || cb == '0')
				{
					if (cb == '0')
						nzb++;
					else
					// only count consecutive zeroes
						nzb = 0;

					cb = CharAt(b, ++ib);
				}

				// process run of digits
				if (ca.IsDigit() && cb.IsDigit())
				{
					int result;
					if ((result = CompareRight(a.Substring(ia), b.Substring(ib))) != 0)
						return result;
				}

				if (ca == 0 && cb == 0)
					// The strings compare the same. Perhaps the caller
					// will want to call strcmp to break the tie.
					return nza - nzb;

				if (ca < cb)
					return -1;
				if (ca > cb)
					return +1;

				++ia;
				++ib;
			}
		}

		private static char CharAt(string s, int i)
		{
			return i >= s.Length ? char.MinValue : s[i];
		}

		int IComparer<string>.Compare(string x, string y)
		{
			return Compare(x, y);
		}
	}
}