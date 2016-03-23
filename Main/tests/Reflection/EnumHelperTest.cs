using System;

using NUnit.Framework;

namespace CodeJam.Reflection
{
	[TestFixture]
	public class EnumHelperTest
	{
		[Test]
		public void GetName() => Assert.AreEqual("ReturnValue", EnumHelper.GetName(AttributeTargets.ReturnValue));

		[Test]
		public void GetValues() =>
			Assert.AreEqual(
				"Assembly, Module, Class, Struct, Enum, Constructor, Method, Property, Field, Event, Interface, Parameter," +
				" Delegate, ReturnValue, GenericParameter, All",
				EnumHelper.GetValues<AttributeTargets>().Join(", "));
	}
}