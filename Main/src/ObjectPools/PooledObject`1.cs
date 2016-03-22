// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.

using System;

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

		public T Object => _pooledObject;

		public PooledObject(ObjectPool<T> pool, Func<ObjectPool<T>, T> allocator, Action<ObjectPool<T>, T> releaser) : this()
		{
			_pool = pool;
			_pooledObject = allocator(pool);
			_releaser = releaser;
		}

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
