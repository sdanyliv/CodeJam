// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.

using System;

using JetBrains.Annotations;

namespace CodeJam.ObjectPools
{
	/// <summary>
	/// The RAII object to automatically release pooled object when its owning pool
	/// </summary>
	public struct PooledObject<T> : IDisposable where T : class
	{
		private readonly Action<ObjectPool<T>, T> _releaser;
		private readonly ObjectPool<T> _pool;
		private T _pooledObject;

		[NotNull]
		public T Object => _pooledObject;

		public PooledObject(ObjectPool<T> pool) : this()
		{
			_pool = pool;
			_pooledObject = pool.Allocate();
			_releaser = null;
		}

		public PooledObject([NotNull] ObjectPool<T> pool, [NotNull] Action<ObjectPool<T>, T> releaser)
		{
			_pool = pool;
			_pooledObject = pool.Allocate();
			_releaser = releaser;
		}

		public PooledObject([NotNull] ObjectPool<T> pool, [NotNull] Func<ObjectPool<T>, T> allocator, [NotNull] Action<ObjectPool<T>, T> releaser)
		{
			_pool = pool;
			_pooledObject = allocator(pool);
			_releaser = releaser;
		}

		/// <summary>
		/// Returns object to the pool.
		/// </summary>
		public void Dispose()
		{
			if (_pooledObject == null)
				return;

			if (_releaser != null)
			{
				_releaser(_pool, _pooledObject);
			}
			else
			{
				_pool.Free(_pooledObject);
			}

			_pooledObject = null;
		}
	}
}
