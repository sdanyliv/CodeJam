using System;
using System.Collections.Generic;
using System.Linq;

using CodeJam.Collections;

using NUnit.Framework;

namespace CodeJam
{
	[TestFixture]
	public class EnumerableExtensionTests
	{
		[TestCase(new[] {"1", "2"}, "3", TestName = "Concat1 1", ExpectedResult = "1, 2, 3")]
		[TestCase(new string[0],    "3", TestName = "Concat1 2", ExpectedResult = "3")]
		public string Concat1(string[] input, string concat)
			=> input.Concat(concat).Join(", ");

		[TestCase(new[] {"1", "2"}, new string[0],      TestName = "Concat2 1", ExpectedResult = "1, 2")]
		[TestCase(new string[0],    new[] { "3", "5" }, TestName = "Concat2 2", ExpectedResult = "3, 5")]
		[TestCase(new[] {"1", "2"}, new[] { "3", "0" }, TestName = "Concat2 3", ExpectedResult = "1, 2, 3, 0")]
		public string Concat2(string[] input, string[] concats)
			=> input.Concat(concats).Join(", ");

		[TestCase(new[] {"1", "2"}, "0", TestName = "Prepend1 1", ExpectedResult = "0, 1, 2")]
		[TestCase(new string[0],    "0", TestName = "Prepend1 2", ExpectedResult = "0")]
		public string Prepend1(string[] input, string prepend)
			=> input.Prepend(prepend).Join(", ");

		[TestCase(new[] {"1", "2"}, new string[0],     TestName = "Prepend2 1", ExpectedResult = "1, 2")]
		[TestCase(new[] {"1", "2"}, new[] {"-1", "0"}, TestName = "Prepend2 2", ExpectedResult = "-1, 0, 1, 2")]
		public string Prepend(string[] input, string[] prepend)
			=> input.Prepend(prepend).Join(", ");

		[TestCase(arg: new[] {"a:b", "b:c", "c"}, ExpectedResult = "c, b, a")]
		[TestCase(arg: new[] { "a:c", "b:c", "c" }, ExpectedResult = "c, a, b")]
		[TestCase(arg: new[] { "a", "b", "c: a, b" }, ExpectedResult = "a, b, c")]
		[TestCase(arg: new[] { "a:c", "b:c", "c", "d:a, b" }, TestName = "Diamond", ExpectedResult = "c, a, b, d")]
		// TODO: add more cases
		public string TopoSort(string[] source)
		{
			// Prepare dependency structure
			Dictionary<string, string[]> deps;
			var items = GetDepStructure(source, out deps);

			// Perform sort
			return items.TopoSort(i => deps[i]).Join(", ");
		}

		[TestCase(arg: new[] { "a:b", "b:a"})]
		[TestCase(arg: new[] { "a:b", "b:c", "c:a" })]
		// TODO: add more cases
		public void TopoSortCycle(string[] source)
		{
			// Prepare dependency structure
			Dictionary<string, string[]> deps;
			var items = GetDepStructure(source, out deps);

			// Perform sort
			// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
			Assert.Throws<ArgumentException>(() => items.TopoSort(i => deps[i]).Join(", "));
		}

		private static IEnumerable<string> GetDepStructure(IEnumerable<string> source, out Dictionary<string, string[]> deps)
		{
			var items = new HashSet<string>();
			deps = new Dictionary<string, string[]>();
			foreach (var itemStr in source)
			{
				var itemParts = itemStr.Split(':');
				var item = itemParts[0].Trim();
				items.Add(item);
				deps.Add(
					item,
					itemParts.Length > 1
						? itemParts[1].Split(',').Select(s => s.Trim()).ToArray()
						: Array<string>.Empty);
			}
			return items;
		}
	}
}