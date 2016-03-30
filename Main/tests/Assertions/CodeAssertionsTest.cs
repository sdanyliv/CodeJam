using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;

using JetBrains.Annotations;

using NUnit.Framework;

namespace CodeJam.Assertions
{
	[TestFixture(Category = "Assertions")]
	[SuppressMessage("ReSharper", "NotResolvedInText")]
	[SuppressMessage("ReSharper", "PassStringInterpolation")]
	[SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
	public class CodeAssertionsTest
	{
		private bool? _breakOnException;
		private CultureInfo _prevCulture;

		[OneTimeSetUp]
		[UsedImplicitly]
		public void SetUp()
		{
			_breakOnException = CodeExceptions.BreakOnException;
			_prevCulture = Thread.CurrentThread.CurrentUICulture;
			CodeExceptions.BreakOnException = false;
			Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
		}

		[OneTimeTearDown]
		[UsedImplicitly]
		public void TearDown()
		{
			Code.NotNull(_breakOnException, nameof(_breakOnException));
			Code.NotNull(_prevCulture, nameof(_prevCulture));
			CodeExceptions.BreakOnException = _breakOnException.GetValueOrDefault();
			Thread.CurrentThread.CurrentUICulture = _prevCulture;
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

			Assert.DoesNotThrow(() => Code.AssertState(true, "someUniqueMessage"));
		}

		[Test]
		public void TestDisposedIf()
		{
			// anything disposable
			var checkingObject = Enumerable.Empty<string>().GetEnumerator();
			var checkingObjectName = checkingObject.GetType().FullName;

			var ex = Assert.Throws<ObjectDisposedException>(
				() => Code.DisposedIf(true, checkingObject));
			Assert.That(ex.Message, Does.Contain(checkingObjectName));

			ex = Assert.Throws<ObjectDisposedException>(
				() => Code.DisposedIf(true, checkingObject, "someUniqueMessage"));
			Assert.That(ex.Message, Does.Contain(checkingObjectName));
			Assert.That(ex.Message, Does.Contain("someUniqueMessage"));

			ex = Assert.Throws<ObjectDisposedException>(
				() => Code.DisposedIf(true, checkingObject, "someUniqueMessage {0}", "someUniqueFormatArg"));
			Assert.That(ex.Message, Does.Contain(checkingObjectName));
			Assert.That(ex.Message, Does.Contain("someUniqueMessage someUniqueFormatArg"));

			ex = Assert.Throws<ObjectDisposedException>(
				() => Code.DisposedIf(true));
			Assert.That(ex.Message, Does.Contain(GetType().FullName));

			Assert.DoesNotThrow(() => Code.DisposedIf(false, checkingObject, "someUniqueMessage"));
		}

		[Test]
		public void TestDisposedIfNull()
		{
			// anything disposable
			var checkingObject = Enumerable.Empty<string>().GetEnumerator();
			var checkingObjectName = checkingObject.GetType().FullName;
			object nullValue = null;
			object notNullValue = "";

			var ex = Assert.Throws<ObjectDisposedException>(
				() => Code.DisposedIfNull(nullValue, checkingObject));
			Assert.That(ex.Message, Does.Contain(checkingObjectName));

			ex = Assert.Throws<ObjectDisposedException>(
				() => Code.DisposedIfNull(nullValue, checkingObject, "someUniqueMessage"));
			Assert.That(ex.Message, Does.Contain(checkingObjectName));
			Assert.That(ex.Message, Does.Contain("someUniqueMessage"));

			ex = Assert.Throws<ObjectDisposedException>(
				() => Code.DisposedIfNull(nullValue, checkingObject, "someUniqueMessage {0}", "someUniqueFormatArg"));
			Assert.That(ex.Message, Does.Contain(checkingObjectName));
			Assert.That(ex.Message, Does.Contain("someUniqueMessage someUniqueFormatArg"));

			ex = Assert.Throws<ObjectDisposedException>(
				() => Code.DisposedIfNull(nullValue));
			Assert.That(ex.Message, Does.Contain(GetType().FullName));

			Assert.DoesNotThrow(() => Code.DisposedIfNull(notNullValue, checkingObject, "someUniqueMessage"));
		}
	}
}