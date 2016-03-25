using System;
using System.Diagnostics.CodeAnalysis;

using NUnit.Framework;

namespace CodeJam.Assertions
{
	[TestFixture]
	[SuppressMessage("ReSharper", "NotResolvedInText")]
	[SuppressMessage("ReSharper", "PassStringInterpolation")]
	public class CodeAssertionsTest
	{
		private bool? m_BreakOnException;

		[OneTimeSetUp]
		public void SetUp()
		{
			m_BreakOnException = CodeExceptions.BreakOnException;
			CodeExceptions.BreakOnException = false;
		}

		[OneTimeTearDown]
		public void TearDown()
		{
			Code.NotNull(m_BreakOnException, nameof(m_BreakOnException));
			CodeExceptions.BreakOnException = m_BreakOnException.GetValueOrDefault();
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