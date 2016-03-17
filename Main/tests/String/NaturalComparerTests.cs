using System.Linq;

using NUnit.Framework;

namespace CodeJam
{
	[TestFixture]
	public class NaturalComparerTests
	{
		[Test]
		public void Sort()
		{
			var data =
				new[]
				{
					"      b3.txt",
					"10",
					"x10m.txt",
					"20",
					"2",
					"444444",
					"b10.txt",
					"a10b1.txt",
					"10.txt",
					"3.txt",
					"x2m.txt",
					"a1b1.txt",
					"a2b2.txt",
					null,
					"        ",
					"01",
					"a2b1.txt",
					"a2b11.txt",
					"     b4.txt    ",
					"b1.txt",
					"1.txt",
					"0000010.txt",
					"b2.txt",
					"1",
					"",
					" 15",
					"                         16"
				};

			var expected =
				new[]
				{
					null,
					"",
					"        ",
					"1",
					"01",
					"1.txt",
					"2",
					"3.txt",
					"0000010.txt",
					"10",
					"10.txt",
					" 15",
					"                         16",
					"20",
					"444444",
					"a1b1.txt",
					"a2b1.txt",
					"a2b2.txt",
					"a2b11.txt",
					"a10b1.txt",
					"b1.txt",
					"b2.txt",
					"      b3.txt",
					"     b4.txt    ",
					"b10.txt",
					"x2m.txt",
					"x10m.txt",
				};

			var comparerSort = data.ToList();
			comparerSort.Sort(NaturalStringComparer.Comparer);
			Assert.IsTrue(comparerSort.Zip(expected, (x, y) => x == y).All(s => s), "#A01");

			var comparisionSort = data.ToList();
			comparisionSort.Sort(NaturalStringComparer.Comparision);
			Assert.IsTrue(comparisionSort.Zip(expected, (x, y) => x == y).All(s => s), "#A02");
		}
	}
}