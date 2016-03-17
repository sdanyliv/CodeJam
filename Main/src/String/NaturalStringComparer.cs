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
		public static readonly NaturalStringComparer Comparer = new NaturalStringComparer();
		public static readonly Comparison<string> Comparision = Compare;

		private NaturalStringComparer() {}

		public static int Compare([CanBeNull] string x, [CanBeNull] string y)
		{
			if (x == null && y == null) return 0;
			if (x == null) return -1;
			if (y == null) return 1;

			int lx = x.Length, ly = y.Length;
			int nwx = lx, nwy = ly;

			for (int mx = 0, my = 0; mx < lx && my < ly; mx++, my++)
			{
				if (char.IsDigit(x[mx]) && char.IsDigit(y[my]))
				{
					long vx = 0, vy = 0;

					for (; mx < lx && char.IsDigit(x[mx]); mx++)
						vx = vx * 10 + x[mx] - '0';

					for (; my < ly && char.IsDigit(y[my]); my++)
						vy = vy * 10 + y[my] - '0';

					if (vx != vy)
						return vx > vy ? 1 : -1;
				}

				for (; mx < lx && char.IsWhiteSpace(x[mx]); mx++, nwx--) ;
				for (; my < ly && char.IsWhiteSpace(y[my]); my++, nwy--) ;

				if (mx < lx && my < ly && x[mx] != y[my])
					return x[mx] > y[my] ? 1 : -1;
			}

			return nwx - nwy;
		}

		int IComparer<string>.Compare(string x, string y)
		{
			return Compare(x, y);
		}
	}
}