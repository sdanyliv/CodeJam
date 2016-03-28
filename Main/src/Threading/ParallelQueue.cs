using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using JetBrains.Annotations;

namespace CodeJam.Threading
{
	class ParallelQueue : IDisposable
	{
		readonly BlockingCollection<Action> _queue = new BlockingCollection<Action>();
		readonly Thread[]                   _workers;
		readonly List<Exception>            _exceptions = new List<Exception>();

		public ParallelQueue(int workerCount, string name = null)
		{
			_workers = new Thread[Math.Max(1, workerCount)];

			for (var i = 0; i < workerCount; i++)
				(_workers[i] = new Thread(Work) { Name = name + i }).Start();
		}

		bool _isFinished;

		readonly object _syncFinished = new object();

		public void WaitAll()
		{
			if (_isFinished)
				return;

			lock (_syncFinished)
			{
				if (_isFinished)
					return;

				_isFinished = true;
			}

			foreach (var _ in _workers)
				_queue.Add(null);

			foreach (var worker in _workers)
				worker.Join();

			_queue.CompleteAdding();

			if (_exceptions.Count > 0)
				throw new AggregateException(_exceptions[0].Message, _exceptions);
		}

		public void EnqueueItem([NotNull] Action item)
		{
			if (item == null) throw new ArgumentNullException(nameof(item));

			_queue.Add(item);
		}

		void Work()
		{
			foreach (var action in _queue.GetConsumingEnumerable())
			{
				if (action == null || _exceptions.Count != 0)
					return;

				try
				{
					action();
				}
				catch (Exception ex)
				{
					_exceptions.Add(ex);
				}
			}
		}

		public void Dispose()
		{
			WaitAll();
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
				_queue?.Dispose();
		}
	}
}
