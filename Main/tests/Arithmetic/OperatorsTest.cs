using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using NUnit.Framework;

using IntOp = CodeJam.Arithmetic.Operators<int>;
using NullableDoubleOp = CodeJam.Arithmetic.Operators<double?>;
using StringOp = CodeJam.Arithmetic.Operators<string>;

namespace CodeJam.Arithmetic
{
	[TestFixture(Category = "Arithmetic")]
	[TestFixture]
	[SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
	public class OperatorsTest
	{
		private class ClassNoComparable { }
		private class ClassComparable : IComparable
		{
			public int CompareTo(object obj) => 0;
		}
		private class ClassGenericComparable : IComparable<ClassGenericComparable>
		{
			public int CompareTo(ClassGenericComparable other) => 0;
		}

		[Test]
		public void Test00OperatorsSupported()
		{
			Debugger.Break();
			GetValue();
		}

		private static void GetValue()
		{
			Assert.IsNull(Operators<ClassNoComparable>.Compare);
			Assert.IsNotNull(Operators<ClassComparable>.Compare);
			Assert.IsNotNull(Operators<ClassGenericComparable>.Compare);
			Assert.IsNull(Operators<int[]>.Compare);
		}

		[Test]
		public void Test01IntOperators()
		{
			Assert.That(IntOp.AreEqual(1, 2), Is.EqualTo(false));
			Assert.That(IntOp.AreEqual(2, 2), Is.EqualTo(true));

			Assert.That(IntOp.AreNotEqual(1, 2), Is.EqualTo(true));
			Assert.That(IntOp.AreNotEqual(2, 2), Is.EqualTo(false));

			Assert.That(IntOp.Compare(1, 2), Is.LessThan(0));
			Assert.That(IntOp.Compare(2, 2), Is.EqualTo(0));
			Assert.That(IntOp.Compare(2, 1), Is.GreaterThan(0));

			Assert.That(IntOp.GreaterThanOrEqual(1, 2), Is.EqualTo(false));
			Assert.That(IntOp.GreaterThanOrEqual(2, 2), Is.EqualTo(true));
			Assert.That(IntOp.GreaterThanOrEqual(2, 1), Is.EqualTo(true));

			Assert.That(IntOp.GreaterThan(1, 2), Is.EqualTo(false));
			Assert.That(IntOp.GreaterThan(2, 2), Is.EqualTo(false));
			Assert.That(IntOp.GreaterThan(2, 1), Is.EqualTo(true));

			Assert.That(IntOp.LessThanOrEqual(1, 2), Is.EqualTo(true));
			Assert.That(IntOp.LessThanOrEqual(2, 2), Is.EqualTo(true));
			Assert.That(IntOp.LessThanOrEqual(2, 1), Is.EqualTo(false));

			Assert.That(IntOp.LessThan(1, 2), Is.EqualTo(true));
			Assert.That(IntOp.LessThan(2, 2), Is.EqualTo(false));
			Assert.That(IntOp.LessThan(2, 1), Is.EqualTo(false));

		}
		[Test]
		public void Test02NullableDoubleOperators()
		{
			Assert.That(NullableDoubleOp.AreEqual(1, 2), Is.EqualTo(false));
			Assert.That(NullableDoubleOp.AreEqual(2, 2), Is.EqualTo(true));

			Assert.That(NullableDoubleOp.AreNotEqual(1, 2), Is.EqualTo(true));
			Assert.That(NullableDoubleOp.AreNotEqual(2, 2), Is.EqualTo(false));

			Assert.That(NullableDoubleOp.Compare(1, 2), Is.LessThan(0));
			Assert.That(NullableDoubleOp.Compare(2, 2), Is.EqualTo(0));
			Assert.That(NullableDoubleOp.Compare(2, 1), Is.GreaterThan(0));

			Assert.That(NullableDoubleOp.GreaterThanOrEqual(1, 2), Is.EqualTo(false));
			Assert.That(NullableDoubleOp.GreaterThanOrEqual(2, 2), Is.EqualTo(true));
			Assert.That(NullableDoubleOp.GreaterThanOrEqual(2, 1), Is.EqualTo(true));

			Assert.That(NullableDoubleOp.GreaterThan(1, 2), Is.EqualTo(false));
			Assert.That(NullableDoubleOp.GreaterThan(2, 2), Is.EqualTo(false));
			Assert.That(NullableDoubleOp.GreaterThan(2, 1), Is.EqualTo(true));

			Assert.That(NullableDoubleOp.LessThanOrEqual(1, 2), Is.EqualTo(true));
			Assert.That(NullableDoubleOp.LessThanOrEqual(2, 2), Is.EqualTo(true));
			Assert.That(NullableDoubleOp.LessThanOrEqual(2, 1), Is.EqualTo(false));

			Assert.That(NullableDoubleOp.LessThan(1, 2), Is.EqualTo(true));
			Assert.That(NullableDoubleOp.LessThan(2, 2), Is.EqualTo(false));
			Assert.That(NullableDoubleOp.LessThan(2, 1), Is.EqualTo(false));
		}

		[Test]
		public void Test03StringOperators()
		{
			Assert.That(StringOp.AreEqual("1", "2"), Is.EqualTo(false));
			Assert.That(StringOp.AreEqual("2", "2"), Is.EqualTo(true));

			Assert.That(StringOp.AreNotEqual("1", "2"), Is.EqualTo(true));
			Assert.That(StringOp.AreNotEqual("2", "2"), Is.EqualTo(false));

			Assert.That(StringOp.Compare("1", "2"), Is.LessThan(0));
			Assert.That(StringOp.Compare("2", "2"), Is.EqualTo(0));
			Assert.That(StringOp.Compare("2", "1"), Is.GreaterThan(0));

			Assert.That(StringOp.GreaterThanOrEqual("1", "2"), Is.EqualTo(false));
			Assert.That(StringOp.GreaterThanOrEqual("2", "2"), Is.EqualTo(true));
			Assert.That(StringOp.GreaterThanOrEqual("2", "1"), Is.EqualTo(true));

			Assert.That(StringOp.GreaterThan("1", "2"), Is.EqualTo(false));
			Assert.That(StringOp.GreaterThan("2", "2"), Is.EqualTo(false));
			Assert.That(StringOp.GreaterThan("2", "1"), Is.EqualTo(true));

			Assert.That(StringOp.LessThanOrEqual("1", "2"), Is.EqualTo(true));
			Assert.That(StringOp.LessThanOrEqual("2", "2"), Is.EqualTo(true));
			Assert.That(StringOp.LessThanOrEqual("2", "1"), Is.EqualTo(false));

			Assert.That(StringOp.LessThan("1", "2"), Is.EqualTo(true));
			Assert.That(StringOp.LessThan("2", "2"), Is.EqualTo(false));
			Assert.That(StringOp.LessThan("2", "1"), Is.EqualTo(false));
		}
	}
}
