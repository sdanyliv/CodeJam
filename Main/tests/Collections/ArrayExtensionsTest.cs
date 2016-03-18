using System;

using NUnit.Framework;

namespace CodeJam
{
	[TestFixture]
	public class ArrayExtensionsTest
	{
		[TestCase(new[] { 1,2,3,4 }, TestName = "Not Empty", ExpectedResult = new[] { 0,0,0,0 })]
		[TestCase(new int[0],        TestName = "Empty",     ExpectedResult = new int[0])]
		public static int[] Clear(int[] input)
		{
			input.Clear();
			return input;
		}
	}
}