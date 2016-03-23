using System;
using System.Linq;

using NUnit.Framework;

namespace CodeJam.Reflection
{
	[TestFixture]
	public class InfoOfTest
	{
		[Test]
		public void ExtarctingPropertyInfo()
		{
			var expected = typeof (User).GetProperty(nameof(User.Name));

			var info1 = InfoOf.Property(() => new User().Name);
			var info2 = InfoOf<User>.Property(u => u.Name);

			Assert.AreEqual(expected, info1, "#1");
			Assert.AreEqual(expected, info2, "#2");
		}

		[Test]
		public void ExtractingFieldInfo()
		{
			var expected = typeof (User).GetField(nameof(User.LastName));

			var info1 = InfoOf.Field(() => new User().LastName);
			var info2 = InfoOf<User>.Field(u => u.LastName);

			Assert.AreEqual(expected, info1, "#1");
			Assert.AreEqual(expected, info2, "#2");
		}

		[Test]
		public void ExtarctingCtor()
		{
			var expected = typeof (User).GetConstructors().First(c => c.GetParameters().Length != 0);
			var ctor1 = InfoOf.Constructor(() => new User("", ""));
			var ctor2 = InfoOf<User>.Constructor(() => new User("", ""));

			Assert.AreEqual(expected, ctor1, "#1");
			Assert.AreEqual(expected, ctor2, "#2");
		}

		[Test]
		public void ExtarctingMethod()
		{
			var expected = typeof (User).GetMethod("Debug");
			var method1 = InfoOf.Method(() => new User().Debug());
			var method2 = InfoOf<User>.Method(u => u.Debug());

			Assert.AreEqual(expected, method1, "#1");
			Assert.AreEqual(expected, method2, "#2");
		}

		#region Inner types
		private class User
		{
			public string Name { get; }
			public readonly string LastName;

			public User()
			{
			}

			public User(string name, string lastName)
			{
				Name = name;
				LastName = lastName;
			}

			// ReSharper disable once MemberCanBeMadeStatic.Local
			public void Debug()
			{}
		}

		#endregion
	}
}
