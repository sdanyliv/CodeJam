using System;
using System.Linq;
using System.Threading;

using CodeJam.Threading;

using NUnit.Framework;

namespace CodeJam.Parallel
{
	[TestFixture]
	public class ParallelQueueTest
	{
		static int _providerCount;
		static int _consumerCount;

		[ThreadStatic] static bool _providerInit;
		[ThreadStatic] static bool _consumerInit;

		[Test]
		public void MultipleConsumerProviderTest()
		{
			Enumerable.Range(1, 100).RunInParallel(
				5, i =>
				{
					if (!_providerInit)
					{
						_providerInit = true;
						Interlocked.Increment(ref _providerCount);
					}

					return i.ToString();
				},
				2, s =>
				{
					if (!_consumerInit)
					{
						_consumerInit = true;
						Interlocked.Increment(ref _consumerCount);
					}

					Console.WriteLine(s);
				});

			Assert.That(_providerCount, Is.EqualTo(5));
			Assert.That(_consumerCount, Is.EqualTo(2));
		}

		static int _actionCount;

		[ThreadStatic] static bool _actionInit;

		[Test]
		public void RunInParallelTest()
		{
			_actionCount = 0;

			Enumerable.Range(1, 50)
				.Select(n => (Action)(() =>
				{
					if (!_actionInit)
					{
						_actionInit = true;
						Interlocked.Increment(ref _actionCount);
					}

					Console.WriteLine(n);
				}))
				.RunInParallel(5);

			Assert.That(_actionCount, Is.EqualTo(5));
		}

		[Test]
		public void RunInParallelActionTest()
		{
			_actionCount = 0;

			new Action[]
			{
				() => Console.WriteLine(1),
				() => Console.WriteLine("2"),
				() => Console.WriteLine("33"),
				() => Console.WriteLine(44),
				() => Console.WriteLine(5),
				() => Console.WriteLine("6"),
				() => Console.WriteLine("7"),
				() => Console.WriteLine("8"),
			}
			.RunInParallel(
				5, a =>
				{
					if (!_actionInit)
					{
						_actionInit = true;
						Interlocked.Increment(ref _actionCount);
					}

					a();
				});

			Assert.That(_actionCount, Is.EqualTo(5));
		}

		[Test]
		public void RunInParallelFuncTest()
		{
			_actionCount = 0;

			Enumerable.Range(1, 50).RunInParallel(
				5, n =>
				{
					if (!_actionInit)
					{
						_actionInit = true;
						Interlocked.Increment(ref _actionCount);
					}

					Console.WriteLine(n);
				});

			Assert.That(_actionCount, Is.EqualTo(5));
		}
	}
}
