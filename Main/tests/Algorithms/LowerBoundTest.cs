using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CodeJam
{
	[TestFixture]
	public class LowerBoundTest
	{
		[Test]
		public void Test01NegativeFrom()
		{
			var list = new List<int> { 0 };
			const int from = -1;
			Assert.That(() => list.LowerBound(10, from, list.Count, (x, y) => x - y)
				, Throws.InstanceOf(typeof(ArgumentOutOfRangeException)));
		}

		[Test]
		public void Test02NegativeTo()
		{
			var list = new List<int> { 0 };
			const int to = -1;
			Assert.That(() => list.LowerBound(10, 0, to, (x, y) => x - y)
				, Throws.InstanceOf(typeof(ArgumentOutOfRangeException)));
		}


		[Test]
		public void Test03ToExceedsCount()
		{
			var list = new List<int> { 0 };
			var to = list.Count + 1;
			Assert.That(() => list.LowerBound(10, 0, to, (x, y) => x - y)
				, Throws.InstanceOf(typeof(ArgumentOutOfRangeException)));
		}

		[Test]
		public void Test04BadFromToOrder()
		{
			var list = new List<int> { 0, 1, 2 };
			const int from = 2;
			const int to = 0;
			Assert.That(() => list.LowerBound(10, from, to, (x, y) => x - y), Throws.ArgumentException);
		}

		[Test]
		public void Test05EmptyRanges()
		{
			var list = new List<int>();
			Assert.That(list.LowerBound(1, 0, 0, (x, y) => x - y), Is.EqualTo(0));
			Assert.That(list.LowerBound(11, (x, y) => x - y), Is.EqualTo(0));
			Assert.That(list.LowerBound(4, 0), Is.EqualTo(0));
			Assert.That(list.LowerBound(14), Is.EqualTo(0));

			list = new List<int> { 1, 5, 12, 35, 123 };
			Assert.That(list.LowerBound(10, 0, 0, (x, y) => x - y), Is.EqualTo(0));
			Assert.That(list.LowerBound(11, 2, 2, (x, y) => x - y), Is.EqualTo(2));
			Assert.That(list.LowerBound(30, list.Count, list.Count, (x, y) => x - y), Is.EqualTo(list.Count));
			Assert.That(list.LowerBound(25, list.Count), Is.EqualTo(list.Count));
		}

		[Test]
		public void Test06Main()
		{
			var list = new List<int> { 1, 5, 12, 12, 123, 512, 512, 14534 };
			Assert.That(list.LowerBound(15, 0, 3, (x, y) => x - y), Is.EqualTo(3));
			Assert.That(list.LowerBound(15, 0), Is.EqualTo(4));
			Assert.That(list.LowerBound(50), Is.EqualTo(4));

			Assert.That(list.LowerBound(5, 1, 4, (x, y) => x - y), Is.EqualTo(1));
			Assert.That(list.LowerBound(5, 0), Is.EqualTo(1));
			Assert.That(list.LowerBound(5), Is.EqualTo(1));

			Assert.That(list.LowerBound(30000), Is.EqualTo(list.Count));
			Assert.That(list.LowerBound(30000, 0, 4, (x, y) => x - y), Is.EqualTo(4));

			Assert.That(list.LowerBound(-1), Is.EqualTo(0));
			Assert.That(list.LowerBound(-1, 4), Is.EqualTo(4));

			Assert.That(list.LowerBound(42), Is.EqualTo(4));
			Assert.That(list.LowerBound(42, 6), Is.EqualTo(6));

			Assert.That(list.LowerBound(1002), Is.EqualTo(7));
			Assert.That(list.LowerBound(1002, 3), Is.EqualTo(7));

			Assert.That(list.LowerBound(12), Is.EqualTo(2));
			Assert.That(list.LowerBound(12, 3), Is.EqualTo(3));

			Assert.That(list.LowerBound(3), Is.EqualTo(1));

			Assert.That(list.LowerBound(14534), Is.EqualTo(list.Count - 1));
		}
	}
}
