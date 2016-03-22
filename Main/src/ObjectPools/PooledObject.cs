using System;
using System.Collections.Generic;
using System.Text;

namespace CodeJam.ObjectPools
{
	public static class PooledObject
	{
		public static PooledObject<StringBuilder> Create(ObjectPool<StringBuilder> pool) =>
			new PooledObject<StringBuilder>(pool, p => p.AllocateAndClear(), (p, sb) => p.ClearAndFree(sb));

		public static PooledObject<Stack<TItem>> Create<TItem>(ObjectPool<Stack<TItem>> pool) =>
			new PooledObject<Stack<TItem>>(pool, p => p.AllocateAndClear(), (p, obj) => p.ClearAndFree(obj));

		public static PooledObject<Queue<TItem>> Create<TItem>(ObjectPool<Queue<TItem>> pool) =>
			new PooledObject<Queue<TItem>>(pool, p => p.AllocateAndClear(), (p, obj) => p.ClearAndFree(obj));

		public static PooledObject<HashSet<TItem>> Create<TItem>(ObjectPool<HashSet<TItem>> pool) =>
			new PooledObject<HashSet<TItem>>(pool, p => p.AllocateAndClear(), (p, obj) => p.ClearAndFree(obj));

		public static PooledObject<Dictionary<TKey, TValue>> Create<TKey, TValue>(ObjectPool<Dictionary<TKey, TValue>> pool) =>
			new PooledObject<Dictionary<TKey, TValue>>(pool, p => p.AllocateAndClear(), (p, obj) => p.ClearAndFree(obj));

		public static PooledObject<List<TItem>> Create<TItem>(ObjectPool<List<TItem>> pool) =>
			new PooledObject<List<TItem>>(pool, p => p.AllocateAndClear(), (p, obj) => p.ClearAndFree(obj));
	}
}
