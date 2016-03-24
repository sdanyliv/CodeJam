using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace CodeJam
{
	[TestFixture]
	public class EqualRangeTest
	{
		[Test]
		public void Test01NegativeFrom()
		{
			var list = new List<int> { 0 };
			const int from = -1;
			Assert.That(() => list.EqualRange(10, from, list.Count, (x, y) => x - y)
				, Throws.InstanceOf(typeof(ArgumentOutOfRangeException)));
		}

		[Test]
		public void Test02NegativeTo()
		{
			var list = new List<int> { 0 };
			const int to = -1;
			Assert.That(() => list.EqualRange(10, 0, to, (x, y) => x - y)
				, Throws.InstanceOf(typeof(ArgumentOutOfRangeException)));
		}

		[Test]
		public void Test03ToExceedsCount()
		{
			var list = new List<int> { 0 };
			var to = list.Count + 1;
			Assert.That(() => list.EqualRange(10, 0, to, (x, y) => x - y)
				, Throws.InstanceOf(typeof(ArgumentOutOfRangeException)));
		}

		[Test]
		public void Test04BadFromToOrder()
		{
			var list = new List<int> { 0, 1, 2 };
			const int from = 2;
			const int to = 1;
			Assert.That(() => list.EqualRange(10, from, to, (x, y) => x - y), Throws.ArgumentException);
		}

		[Test]
		[TestCase(new int[0], 1, 0, 0)]
		[TestCase(new[] { 1, 5, 12, 35, 123 }, 5, 0, 0)]
		[TestCase(new[] { 1, 5, 12, 35, 123 }, 21, 3, 3)]
		[TestCase(new[] { 1, 5, 12, 35, 123 }, 101, 5, 5)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 15, 0, 3)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 5, 1, 4)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 30000, 0, 4)]
		public void Test05WithAllParams(int[] data, int value, int from, int to)
		{
			var list = (IList<int>)data;
			var expected = Tuple.Create(
				list.LowerBound(value, from, to, (x, y) => x - y), list.UpperBound(value, from, to, (x, y) => x - y));
			Assert.That(list.EqualRange(value, from, to, (x, y) => x - y), Is.EqualTo(expected));
		}

		[Test]
		[TestCase(new int[0], 11)]
		public void Test06WithComparer(int[] data, int value)
		{
			var list = (IList<int>)data;
			var expected = Tuple.Create(
				list.LowerBound(value, (x, y) => x - y), list.UpperBound(value, (x, y) => x - y));
			Assert.That(list.EqualRange(value, (x, y) => x - y), Is.EqualTo(expected));
		}

		[Test]
		[TestCase(new int[0], 4, 0)]
		[TestCase(new[] { 1, 5, 12, 35, 123 }, 15, 5)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 15, 0)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 5, 0)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, -1, 4)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 1, 3)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 42, 6)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 1002, 3)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 12, 1)]
		public void Test07WithFrom(int[] data, int value, int from)
		{
			var list = (IList<int>)data;
			var expected = Tuple.Create(
				list.LowerBound(value, from), list.UpperBound(value, from));
			Assert.That(list.EqualRange(value, from), Is.EqualTo(expected));
		}

		[Test]
		[TestCase(new int[0], 14)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 50)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 5)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 30000)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, -1)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 1)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 42)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 1002)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 12)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 3)]
		[TestCase(new[] { 1, 5, 12, 12, 123, 512, 512, 14534 }, 14534)]
		public void Test08WithoutParams(int[] data, int value)
		{
			var list = (IList<int>)data;
			var expected = Tuple.Create(
				list.LowerBound(value), list.UpperBound(value));
			Assert.That(list.EqualRange(value), Is.EqualTo(expected));
		}

		[Test]
		public void Test09Randomized()
		{
			var list = new List<int>();
			var rnd = new Random();
			const int maxValue = 10000;
			for (var i = 0; i < maxValue; ++i)
			{
				var repeats = rnd.Next(1, 11);
				list.AddRange(Enumerable.Repeat(i, repeats));
			}
			for (var j = -10; j < maxValue + 10; ++j)
			{
				var expected = Tuple.Create(
					list.LowerBound(j), list.UpperBound(j));
				Assert.That(list.EqualRange(j), Is.EqualTo(expected));
			}
		}
	}
}
