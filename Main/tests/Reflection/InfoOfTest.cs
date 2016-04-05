using System;
using System.Linq;

using JetBrains.Annotations;

using NUnit.Framework;

namespace CodeJam.Reflection
{
	[TestFixture]
	public class InfoOfTest
	{
		[Test]
		public void ExtarctingPropertyInfo1()
		{
			var expected = typeof (User).GetProperty(nameof (User.Name));

			var info1 = InfoOf.Property(() => (object)new User().Name);
			var info2 = InfoOf.Property(() => new User().Name);
			var info3 = InfoOf.Property<User>(u => u.Name);
			var info4 = InfoOf.Property<User, string>(u => u.Name);
			var info5 = InfoOf<User>.Property(u => u.Name);

			Assert.AreEqual(expected, info1, "#1");
			Assert.AreEqual(expected, info2, "#2");
			Assert.AreEqual(expected, info3, "#3");
			Assert.AreEqual(expected, info4, "#4");
			Assert.AreEqual(expected, info5, "#5");
		}

		[Test]
		public void ExtarctingPropertyInfo2()
		{
			var expected = typeof (User).GetProperty(nameof (User.Age));

			var info1 = InfoOf.Property(() => (object)new User().Age);
			var info2 = InfoOf.Property(() => new User().Age);
			var info3 = InfoOf.Property<User>(u => u.Age);
			var info4 = InfoOf.Property<User, int>(u => u.Age);
			var info5 = InfoOf<User>.Property(u => u.Age);

			Assert.AreEqual(expected, info1, "#1");
			Assert.AreEqual(expected, info2, "#2");
			Assert.AreEqual(expected, info3, "#3");
			Assert.AreEqual(expected, info4, "#4");
			Assert.AreEqual(expected, info5, "#5");
		}

		[Test]
		public void ExtractingFieldInfo()
		{
			var expected = typeof (User).GetField(nameof (User.LastName));

			var info1 = InfoOf.Field(() => (object)new User().LastName);
			var info2 = InfoOf.Field(() => new User().LastName);
			var info3 = InfoOf.Field<User>(u => u.LastName);
			var info4 = InfoOf.Field<User, string>(u => u.LastName);
			var info5 = InfoOf<User>.Field(u => u.LastName);

			Assert.AreEqual(expected, info1, "#1");
			Assert.AreEqual(expected, info2, "#2");
			Assert.AreEqual(expected, info3, "#3");
			Assert.AreEqual(expected, info4, "#4");
			Assert.AreEqual(expected, info5, "#5");
		}

		[Test]
		public void ExtarctingCtor()
		{
			var expected = typeof (User).GetConstructors().First(c => c.GetParameters().Length != 0);
			var ctor1 = InfoOf.Constructor(() => (object)new User("", ""));
			var ctor2 = InfoOf.Constructor(() => new User("", ""));
			var ctor3 = InfoOf<User>.Constructor(() => new User("", ""));

			Assert.AreEqual(expected, ctor1, "#1");
			Assert.AreEqual(expected, ctor2, "#2");
			Assert.AreEqual(expected, ctor3, "#3");
		}

		[Test]
		public void ExtarctingMethod()
		{
			var expected = typeof (User).GetMethod("Debug");
			var method1 = InfoOf.Method(() => new User().Debug());
			var method2 = InfoOf.Method<User>(u => u.Debug());
			var method3 = InfoOf<User>.Method(u => u.Debug());

			Assert.AreEqual(expected, method1, "#1");
			Assert.AreEqual(expected, method2, "#2");
			Assert.AreEqual(expected, method3, "#3");
		}

		[Test]
		public void ExtarctingPropertyGetMethod()
		{
			var expected = typeof (User).GetProperty("Name").GetMethod;
			var method1 = InfoOf.Method(() => (object)new User().Name);
			var method2 = InfoOf.Method(() => new User().Name);
			var method3 = InfoOf.Method<User>(u => u.Name);
			var method4 = InfoOf.Method<User, string>(u => u.Name);
			var method5 = InfoOf<User>.Method(u => u.Name);

			Assert.AreEqual(expected, method1, "#1");
			Assert.AreEqual(expected, method2, "#2");
			Assert.AreEqual(expected, method3, "#3");
			Assert.AreEqual(expected, method4, "#4");
			Assert.AreEqual(expected, method5, "#5");
		}

		#region Inner types
		[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
		public class User
		{
			public string Name { get; set; }
			public string LastName;
			public int Age { get; set; }

			public User()
			{
			}

			public User(string name, string lastName)
			{
				Name = name;
				LastName = lastName;
			}

			public void Debug()
			{
			}
		}
		#endregion
	}
}
