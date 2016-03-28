using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using NUnit.Framework;

namespace CodeJam.Assertions
{
	[TestFixture]
	[SuppressMessage("ReSharper", "NotResolvedInText")]
	[SuppressMessage("ReSharper", "PassStringInterpolation")]
	public class CodeAssertionsTest
	{
		private bool? _breakOnException;

		[OneTimeSetUp]
		[UsedImplicitly]
		public void SetUp()
		{
			_breakOnException = CodeExceptions.BreakOnException;
			CodeExceptions.BreakOnException = false;
		}

		[OneTimeTearDown]
		[UsedImplicitly]
		public void TearDown()
		{
			Code.NotNull(_breakOnException, nameof(_breakOnException));
			CodeExceptions.BreakOnException = _breakOnException.GetValueOrDefault();
		}

		[Test]
		public void TestNotNull()
		{
			var ex = Assert.Throws<ArgumentNullException>(() => Code.NotNull<object>(null, "someUniqueArgName"));
			Assert.That(ex.Message.Contains("someUniqueArgName"));

			Assert.DoesNotThrow(() => Code.NotNull<object>("Hello!", "someUniqueArgName"));
		}

		[Test]
		public void TestNotNullNorEmpty()
		{
			Assert.Throws<ArgumentException>(() => Code.NotNullNorEmpty(null, "someUniqueArgName"));
			var ex = Assert.Throws<ArgumentException>(() => Code.NotNullNorEmpty("", "someUniqueArgName"));
			Assert.That(ex.Message.Contains("someUniqueArgName"));

			Assert.DoesNotThrow(() => Code.NotNullNorEmpty("Hello!", "someUniqueArgName"));
		}

		[Test]
		public void TestAssertArgument()
		{
			var ex = Assert.Throws<ArgumentException>(
				() => Code.AssertArgument(false, "someUniqueArgName", "someUniqueMessage {0}", "someUniqueFormatArg"));
			Assert.That(ex.Message.Contains("someUniqueArgName"));
			Assert.That(ex.Message.Contains("someUniqueMessage"));
			Assert.That(ex.Message.Contains("someUniqueFormatArg"));

			Assert.DoesNotThrow(() => Code.AssertArgument(true, "someUniqueArgName", "someUniqueMessage"));
		}

		[Test]
		public void TestAssertState()
		{
			var ex = Assert.Throws<InvalidOperationException>(
				() => Code.AssertState(false, "someUniqueMessage"));
			Assert.AreEqual(ex.Message, "someUniqueMessage");

			ex = Assert.Throws<InvalidOperationException>(
				() => Code.AssertState(false, "someUniqueMessage {0}", "someUniqueFormatArg"));
			Assert.AreEqual(ex.Message, "someUniqueMessage someUniqueFormatArg");

			Assert.DoesNotThrow(() => Code.AssertArgument(true, "someUniqueArgName", "someUniqueMessage"));
		}
	}
}