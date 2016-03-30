﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using NUnit.Framework;

namespace CodeJam.Reflection
{
	[TestFixture]
	public class ReflectionExtensionsTest
	{
		[TestCase(typeof (List<int>),     typeof (IList<int>),       ExpectedResult = true)]
		[TestCase(typeof (List<int>),     typeof (IList),            ExpectedResult = true)]
		[TestCase(typeof (List<int>),     typeof (IList<>),          ExpectedResult = true)]
		[TestCase(typeof (List<int>),     typeof (IEnumerable<int>), ExpectedResult = true)]
		[TestCase(typeof (List<int>),     typeof (IEnumerable),      ExpectedResult = true)]
		[TestCase(typeof (List<int>),     typeof (IEnumerable<>),    ExpectedResult = true)]
		[TestCase(typeof (IList<int>),    typeof (IEnumerable<>),    ExpectedResult = true)]
		[TestCase(typeof (IList<>),       typeof (IEnumerable<>),    ExpectedResult = true)]
		[TestCase(typeof (IEnumerable<>), typeof (IEnumerable),      ExpectedResult = true)]
		[TestCase(typeof (List<int>),     typeof (List<int>),        ExpectedResult = false)]
		[TestCase(typeof (IList<>),       typeof (ISet<>),           ExpectedResult = false)]
		public bool IsSubClassTest(Type type, Type check) => type.IsSubClass(check);

		[TestCase(typeof (int),                       ExpectedResult = true)]
		[TestCase(typeof (List<int>),                 ExpectedResult = true)]
		[TestCase(typeof (List<>),                    ExpectedResult = false)]
		[TestCase(typeof (int[]),                     ExpectedResult = false)]
		[TestCase(typeof (IList),                     ExpectedResult = false)]
		[TestCase(typeof (ReflectionExtensions),      ExpectedResult = false)]
		[TestCase(typeof (KeyedCollection<int, int>), ExpectedResult = false)]
		public bool IsInstantiableTypeTest(Type type) => type.IsInstantiable();

		[TestCase(typeof (int),                       ExpectedResult = false)]
		[TestCase(typeof (int?),                      ExpectedResult = true)]
		[TestCase(typeof (string),                    ExpectedResult = false)]
		[TestCase(typeof (double),                    ExpectedResult = false)]
		[TestCase(typeof (double?),                   ExpectedResult = true)]
		public bool IsIsNullableTypeTest(Type type) => type.IsNullable();
	}
}
