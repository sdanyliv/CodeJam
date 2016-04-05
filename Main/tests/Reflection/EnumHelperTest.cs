using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using NUnit.Framework;

namespace CodeJam.Reflection
{
	[TestFixture]
	public class EnumHelperTest
	{
		[Test]
		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		public void GetField() =>
			Assert.AreEqual(
				AttributeTargets.ReturnValue,
				EnumHelper.GetField(AttributeTargets.ReturnValue).GetValue(null));

		[Test]
		public void GetName() => Assert.AreEqual("ReturnValue", EnumHelper.GetName(AttributeTargets.ReturnValue));

		[Test]
		public void GetNames() =>
			Assert.AreEqual(
				"Assembly, Module, Class, Struct, Enum, Constructor, Method, Property, Field, Event, Interface, Parameter, " +
				"Delegate, ReturnValue, GenericParameter, All",
				EnumHelper.GetNames<AttributeTargets>().Join(", "));

		[Test]
		public void GetValues() =>
			Assert.AreEqual(
				"Assembly, Module, Class, Struct, Enum, Constructor, Method, Property, Field, Event, Interface, Parameter, " +
				"Delegate, ReturnValue, GenericParameter, All",
				EnumHelper.GetValues<AttributeTargets>().Join(", "));

		[Test]
		public void GetPairs() =>
			Assert.AreEqual(
				"Assembly, Module, Class, Struct, Enum, Constructor, Method, Property, Field, Event, Interface, Parameter, " +
				"Delegate, ReturnValue, GenericParameter, All",
				EnumHelper.GetPairs<AttributeTargets>().Select(kvp => kvp.Key).Join(", "));
	}
}