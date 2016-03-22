using System;
using System.Collections.Generic;
using System.Text;

namespace CodeJam.ObjectPools
{
	public static class PooledObject
	{
		#region factory

		public static PooledObject<StringBuilder> Create(ObjectPool<StringBuilder> pool)
		{
			return new PooledObject<StringBuilder>(pool, Allocator, Releaser);
		}

		public static PooledObject<Stack<TItem>> Create<TItem>(ObjectPool<Stack<TItem>> pool)
		{
			return new PooledObject<Stack<TItem>>(pool, Allocator, Releaser);
		}

		public static PooledObject<Queue<TItem>> Create<TItem>(ObjectPool<Queue<TItem>> pool)
		{
			return new PooledObject<Queue<TItem>>(pool, Allocator, Releaser);
		}

		public static PooledObject<HashSet<TItem>> Create<TItem>(ObjectPool<HashSet<TItem>> pool)
		{
			return new PooledObject<HashSet<TItem>>(pool, Allocator, Releaser);
		}

		public static PooledObject<Dictionary<TKey, TValue>> Create<TKey, TValue>(ObjectPool<Dictionary<TKey, TValue>> pool)
		{
			return new PooledObject<Dictionary<TKey, TValue>>(pool, Allocator, Releaser);
		}

		public static PooledObject<List<TItem>> Create<TItem>(ObjectPool<List<TItem>> pool)
		{
			return new PooledObject<List<TItem>>(pool, Allocator, Releaser);
		}

		#endregion

		#region allocators and releasers

		private static StringBuilder Allocator(ObjectPool<StringBuilder> pool)
		{
			return pool.AllocateAndClear();
		}

		private static void Releaser(ObjectPool<StringBuilder> pool, StringBuilder sb)
		{
			pool.ClearAndFree(sb);
		}

		private static Stack<TItem> Allocator<TItem>(ObjectPool<Stack<TItem>> pool)
		{
			return pool.AllocateAndClear();
		}

		private static void Releaser<TItem>(ObjectPool<Stack<TItem>> pool, Stack<TItem> obj)
		{
			pool.ClearAndFree(obj);
		}

		private static Queue<TItem> Allocator<TItem>(ObjectPool<Queue<TItem>> pool)
		{
			return pool.AllocateAndClear();
		}

		private static void Releaser<TItem>(ObjectPool<Queue<TItem>> pool, Queue<TItem> obj)
		{
			pool.ClearAndFree(obj);
		}

		private static HashSet<TItem> Allocator<TItem>(ObjectPool<HashSet<TItem>> pool)
		{
			return pool.AllocateAndClear();
		}

		private static void Releaser<TItem>(ObjectPool<HashSet<TItem>> pool, HashSet<TItem> obj)
		{
			pool.ClearAndFree(obj);
		}

		private static Dictionary<TKey, TValue> Allocator<TKey, TValue>(ObjectPool<Dictionary<TKey, TValue>> pool)
		{
			return pool.AllocateAndClear();
		}

		private static void Releaser<TKey, TValue>(ObjectPool<Dictionary<TKey, TValue>> pool, Dictionary<TKey, TValue> obj)
		{
			pool.ClearAndFree(obj);
		}

		private static List<TItem> Allocator<TItem>(ObjectPool<List<TItem>> pool)
		{
			return pool.AllocateAndClear();
		}

		private static void Releaser<TItem>(ObjectPool<List<TItem>> pool, List<TItem> obj)
		{
			pool.ClearAndFree(obj);
		}

		#endregion
	}
}
