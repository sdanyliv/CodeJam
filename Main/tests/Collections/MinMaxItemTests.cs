using System.Linq;

using NUnit.Framework;

namespace CodeJam
{
	[TestFixture]
	public partial class MinMaxItemTests
	{
		#region Min
		[TestCase(new[] {3, 1, 0, 4, 6}, ExpectedResult = "0")]
		[TestCase(new[] {1}, ExpectedResult = "1")]
		[TestCase(new int[0], ExpectedResult = null)]
		public string MinItemString(int[] source) =>
			Collections.EnumerableExtensions.MinItem(source.Select(v => new Item<string>(v.ToString())), i => i.Value)?.Value;
		#endregion

		#region Max
		[TestCase(new[] { 3, 1, 8, 0, 6 }, ExpectedResult = "8")]
		[TestCase(new[] { 1 }, ExpectedResult = "1")]
		[TestCase(new int[0], ExpectedResult = null)]
		public string MaxItemString(int[] source) =>
			Collections.EnumerableExtensions.MaxItem(source.Select(v => new Item<string>(v.ToString())), i => i.Value)?.Value;
		#endregion

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