using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace CodeJam.Threading
{
	/// <summary>
	/// Parallel extensions.
	/// </summary>
	[PublicAPI]
	public static class ParallelExtensions
	{
		/// <summary>
		/// Implements Provider-Consumer pattern.
		/// </summary>
		/// <typeparam name="TSourse"></typeparam>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="source">Incoming data.</param>
		/// <param name="providerCount">Number of provider threads.</param>
		/// <param name="providerFunc">Provider function</param>
		/// <param name="consumerCount">Number of consumer threads.</param>
		/// <param name="consumerAction">Consumer action.</param>
		/// <param name="processName">Process name pattern.</param>
		public static void RunInParallel<TSourse,TTarget>(
			[NotNull] this IEnumerable<TSourse> source,
			int providerCount,
			[NotNull] Func<TSourse,TTarget> providerFunc,
			int consumerCount,
			[NotNull] Action<TTarget> consumerAction,
			string processName = "ParallelProcess")
		{
			if (source         == null) throw new ArgumentNullException(nameof(source));
			if (providerFunc   == null) throw new ArgumentNullException(nameof(providerFunc));
			if (consumerAction == null) throw new ArgumentNullException(nameof(consumerAction));

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

		/// <summary>
		/// Implements Provider-Consumer pattern.
		/// </summary>
		/// <typeparam name="TSourse"></typeparam>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="source">Incoming data.</param>
		/// <param name="providerFunc">Provider function</param>
		/// <param name="consumerCount">Number of consumer threads.</param>
		/// <param name="consumerAction">Consumer action.</param>
		/// <param name="processName">Process name pattern.</param>
		public static void RunInParallel<TSourse,TTarget>(
			[NotNull] this IEnumerable<TSourse> source,
			[NotNull] Func<TSourse,TTarget> providerFunc,
			int consumerCount,
			[NotNull] Action<TTarget> consumerAction,
			string processName = "ParallelProcess")
		{
			RunInParallel(source, Environment.ProcessorCount, providerFunc, consumerCount, consumerAction, processName);
		}

		/// <summary>
		/// Implements Provider-Consumer pattern.
		/// </summary>
		/// <typeparam name="TSourse"></typeparam>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="source">Incoming data.</param>
		/// <param name="providerCount">Number of provider threads.</param>
		/// <param name="providerFunc">Provider function</param>
		/// <param name="consumerAction">Consumer action.</param>
		/// <param name="processName">Process name pattern.</param>
		public static void RunInParallel<TSourse,TTarget>(
			[NotNull] this IEnumerable<TSourse> source,
			int providerCount,
			[NotNull] Func<TSourse,TTarget> providerFunc,
			[NotNull] Action<TTarget> consumerAction,
			string processName = "ParallelProcess")
		{
			RunInParallel(source, providerCount, providerFunc, Environment.ProcessorCount, consumerAction, processName);
		}

		/// <summary>
		/// Implements Provider-Consumer pattern.
		/// </summary>
		/// <typeparam name="TSourse"></typeparam>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="source">Incoming data.</param>
		/// <param name="providerFunc">Provider function</param>
		/// <param name="consumerAction">Consumer action.</param>
		/// <param name="processName">Process name pattern.</param>
		public static void RunInParallel<TSourse,TTarget>(
			[NotNull] this IEnumerable<TSourse> source,
			[NotNull] Func<TSourse,TTarget> providerFunc,
			[NotNull] Action<TTarget> consumerAction,
			string processName = "ParallelProcess")
		{
			RunInParallel(source, Environment.ProcessorCount / 2, providerFunc, Environment.ProcessorCount / 2, consumerAction, processName);
		}

		/// <summary>
		/// Runs in parallel provided source of actions.
		/// </summary>
		/// <param name="source">Actions to run.</param>
		/// <param name="parallelCount">number of threads to use.</param>
		/// <param name="processName">Process name pattern.</param>
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

		/// <summary>
		/// Runs in parallel provided source of actions.
		/// </summary>
		/// <param name="source">Actions to run.</param>
		/// <param name="processName">Process name pattern.</param>
		public static void RunInParallel(this IEnumerable<Action> source, string processName = "ParallelProcess")
			=> RunInParallel(source, Environment.ProcessorCount, processName);

		/// <summary>
		/// Runs in parallel actions for provided data source.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">Source to run.</param>
		/// <param name="parallelCount">number of threads to use.</param>
		/// <param name="action">Action to run.</param>
		/// <param name="processName">Process name.</param>
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


		/// <summary>
		/// Runs in parallel actions for provided data source.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">Source to run.</param>
		/// <param name="action">Action to run.</param>
		/// <param name="processName">Process name.</param>
		public static void RunInParallel<T>(this IEnumerable<T> source, Action<T> action, string processName = "ParallelProcess")
			=> RunInParallel(source, Environment.ProcessorCount, action, processName);
	}
}
