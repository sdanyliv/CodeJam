using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace CodeJam.Threading
{
	[PublicAPI]
	public static class ParallelExtensions
	{
		public static void RunMultipleProviderConsumers<TSourse,TTarget>(
			this IEnumerable<TSourse> source,
			int providerCount,
			Func<TSourse,TTarget> providerFunc,
			int consumerCount,
			Action<TTarget> consumerAction,
			string processName = "ParallelProcess")
		{
			using (var providerQueue = new ParallelQueue(providerCount, processName + "_provider_"))
			using (var consumerQueue = new ParallelQueue(consumerCount, processName + "_consumer_"))
			{
				foreach (var item in source)
				{
					var pItem = item;

					providerQueue.EnqueueItem(() =>
					{
						var data = providerFunc(pItem);

						consumerQueue.EnqueueItem(() => consumerAction(data));
					});
				}

				providerQueue.WaitAll();
				consumerQueue.WaitAll();
			}
		}

		public static void RunMultipleProviderConsumers<TSourse,TTarget>(
			this IEnumerable<TSourse> source,
			Func<TSourse,TTarget> providerFunc,
			int consumerCount,
			Action<TTarget> consumerAction,
			string processName = "ParallelProcess")
		{
			RunMultipleProviderConsumers(source, Environment.ProcessorCount, providerFunc, consumerCount, consumerAction, processName);
		}

		public static void RunMultipleProviderConsumers<TSourse,TTarget>(
			this IEnumerable<TSourse> source,
			int providerCount,
			Func<TSourse,TTarget> providerFunc,
			Action<TTarget> consumerAction,
			string processName = "ParallelProcess")
		{
			RunMultipleProviderConsumers(source, providerCount, providerFunc, Environment.ProcessorCount, consumerAction, processName);
		}

		public static void RunMultipleProviderConsumers<TSourse,TTarget>(
			this IEnumerable<TSourse> source,
			Func<TSourse,TTarget> providerFunc,
			Action<TTarget> consumerAction,
			string processName = "ParallelProcess")
		{
			RunMultipleProviderConsumers(source, Environment.ProcessorCount, providerFunc, Environment.ProcessorCount, consumerAction, processName);
		}

		public static void RunInParallel(this IEnumerable<Action> source, int parallelCount, string processName = "ParallelProcess")
		{
			using (var queue = new ParallelQueue(parallelCount, processName + '_'))
			{
				foreach (var action in source)
				{
					var data = action;
					queue.EnqueueItem(data);
				}

				queue.WaitAll();
			}
		}

		public static void RunInParallel(this IEnumerable<Action> source, string processName = "ParallelProcess")
			=> RunInParallel(source, Environment.ProcessorCount, processName);

		public static void RunInParallel<T>(this IEnumerable<T> source, int parallelCount, Action<T> action, string processName = "ParallelProcess")
		{
			using (var queue = new ParallelQueue(parallelCount, processName + '_'))
			{
				foreach (var item in source)
				{
					var data = item;
					queue.EnqueueItem(() => action(data));
				}

				queue.WaitAll();
			}
		}

		public static void RunInParallel<T>(this IEnumerable<T> source, Action<T> action, string processName = "ParallelProcess")
			=> RunInParallel(source, Environment.ProcessorCount, action, processName);
	}
}
