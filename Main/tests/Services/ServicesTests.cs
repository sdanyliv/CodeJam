using System;

using NUnit.Framework;

namespace CodeJam.Services
{
	[TestFixture]
	public class ServicesTests
	{
		[Test]
		public void ServiceContainer()
		{
			var container = new ServiceContainer();

			Assert.IsNull(container.GetService<ISampleSvc>(), "#A01");
			var sampleSvc = new SampleSvc();

			using (container.Publish<ISampleSvc>(sampleSvc))
			{
				Assert.AreEqual(sampleSvc, container.GetService<ISampleSvc>(), "#A02");
				Assert.Throws<ArgumentException>(() => container.Publish<ISampleSvc>(sampleSvc));
			}
			Assert.IsNull(container.GetService<ISampleSvc>(), "#A03");

			var calls = 0;
			using (
				container.Publish<ISampleSvc>(
					// ReSharper disable ImplicitlyCapturedClosure
					sp =>
					// ReSharper restore ImplicitlyCapturedClosure
					{
						calls++;
						return sampleSvc;
					}))
			{
				Assert.AreEqual(0, calls, "#A04");
				Assert.AreEqual(sampleSvc, container.GetService<ISampleSvc>(), "#A05");
				Assert.AreEqual(1, calls, "#A06");
			}
			Assert.IsNull(container.GetService<ISampleSvc>(), "#A07");
		}

		private interface ISampleSvc { }

		private class SampleSvc : ISampleSvc { }
	}
}