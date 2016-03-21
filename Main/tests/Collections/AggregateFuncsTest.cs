using System.Linq;

using NUnit.Framework;

namespace CodeJam
{
	[TestFixture]
	public class AggregateFuncsTest
	{
		[TestCase(new[] {3, 1, 0, 4, 6}, ExpectedResult = 0)]
		[TestCase(new[] { 1 }, ExpectedResult = 1)]
		public int MinInt(int[] source) => source.Select(v => new Item<int>(v)).Min(i => i.Value).Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, ExpectedResult = 0)]
		[TestCase(new[] {1}, ExpectedResult = 1)]
		[TestCase(new int[0], ExpectedResult = null)]
		public int? MinNullableInt(int[] source) => source.Select(v => new Item<int?>(v)).Min(i => i.Value)?.Value;

		[TestCase(new[] { 3, 1, 0, 4, 6 }, ExpectedResult = 0)]
		[TestCase(new[] { 1 }, ExpectedResult = 1)]
		public long MinLong(int[] source) => source.Select(v => new Item<long>(v)).Min(i => i.Value).Value;

		[TestCase(new[] { 3, 1, 0, 4, 6 }, ExpectedResult = 0)]
		[TestCase(new[] { 1 }, ExpectedResult = 1)]
		[TestCase(new int[0], ExpectedResult = null)]
		public long? MinNullableLong(int[] source) => source.Select(v => new Item<long?>(v)).Min(i => i.Value)?.Value;

		[TestCase(new[] { 3, 1, 0, 4, 6 }, ExpectedResult = 0)]
		[TestCase(new[] { 1 }, ExpectedResult = 1)]
		public float MinFloat(int[] source) => source.Select(v => new Item<float>(v)).Min(i => i.Value).Value;

		[TestCase(new[] { 3, 1, 0, 4, 6 }, ExpectedResult = 0)]
		[TestCase(new[] { 1 }, ExpectedResult = 1)]
		[TestCase(new int[0], ExpectedResult = null)]
		public float? MinNullableFloat(int[] source) => source.Select(v => new Item<float?>(v)).Min(i => i.Value)?.Value;

		[TestCase(new[] { 3, 1, 0, 4, 6 }, ExpectedResult = 0)]
		[TestCase(new[] { 1 }, ExpectedResult = 1)]
		public double MinDouble(int[] source) => source.Select(v => new Item<double>(v)).Min(i => i.Value).Value;

		[TestCase(new[] { 3, 1, 0, 4, 6 }, ExpectedResult = 0)]
		[TestCase(new[] { 1 }, ExpectedResult = 1)]
		[TestCase(new int[0], ExpectedResult = null)]
		public double? MinNullableDouble(int[] source) => source.Select(v => new Item<double?>(v)).Min(i => i.Value)?.Value;

		[TestCase(new[] { 3, 1, 0, 4, 6 }, ExpectedResult = 0)]
		[TestCase(new[] { 1 }, ExpectedResult = 1)]
		public decimal MinDec(int[] source) => source.Select(v => new Item<decimal>(v)).Min(i => i.Value).Value;

		[TestCase(new[] { 3, 1, 0, 4, 6 }, ExpectedResult = 0)]
		[TestCase(new[] { 1 }, ExpectedResult = 1)]
		[TestCase(new int[0], ExpectedResult = null)]
		public decimal? MinNullableDec(int[] source) => source.Select(v => new Item<decimal?>(v)).Min(i => i.Value)?.Value;

		[TestCase(new[] { 3, 1, 0, 4, 6 }, ExpectedResult = "0")]
		[TestCase(new[] { 1 }, ExpectedResult = "1")]
		public string MinStr(int[] source) => source.Select(v => new Item<string>(v.ToString())).Min(i => i.Value).Value;

		#region Item class
		private class Item<T>
		{
			public Item(T value)
			{
				Value = value;
			}

			public T Value { get; }
		}
		#endregion
	}
}