using System;
using System.Collections.Generic;
using System.Text;

using JetBrains.Annotations;

namespace CodeJam.ObjectPools
{
	/// <summary>
	/// The pooled object factory.
	/// </summary>
	public static class PooledObject
	{
		[Pure]
		public static PooledObject<StringBuilder> Create([NotNull] ObjectPool<StringBuilder> pool) =>
			new PooledObject<StringBuilder>(pool, p => p.AllocateAndClear(), (p, sb) => p.ClearAndFree(sb));

		[Pure]
		public static PooledObject<Stack<TItem>> Create<TItem>([NotNull] ObjectPool<Stack<TItem>> pool) =>
			new PooledObject<Stack<TItem>>(pool, p => p.AllocateAndClear(), (p, obj) => p.ClearAndFree(obj));

		[Pure]
		public static PooledObject<Queue<TItem>> Create<TItem>([NotNull] ObjectPool<Queue<TItem>> pool) =>
			new PooledObject<Queue<TItem>>(pool, p => p.AllocateAndClear(), (p, obj) => p.ClearAndFree(obj));

		[Pure]
		public static PooledObject<HashSet<TItem>> Create<TItem>([NotNull] ObjectPool<HashSet<TItem>> pool) =>
			new PooledObject<HashSet<TItem>>(pool, p => p.AllocateAndClear(), (p, obj) => p.ClearAndFree(obj));

		[Pure]
		public static PooledObject<Dictionary<TKey, TValue>> Create<TKey, TValue>([NotNull] ObjectPool<Dictionary<TKey, TValue>> pool) =>
			new PooledObject<Dictionary<TKey, TValue>>(pool, p => p.AllocateAndClear(), (p, obj) => p.ClearAndFree(obj));

		[Pure]
		public static PooledObject<List<TItem>> Create<TItem>([NotNull] ObjectPool<List<TItem>> pool) =>
			new PooledObject<List<TItem>>(pool, p => p.AllocateAndClear(), (p, obj) => p.ClearAndFree(obj));
	}
}
