// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Text;

using JetBrains.Annotations;

namespace CodeJam.ObjectPools
{
	public static class SharedPoolExtensions
	{
		private const int Threshold = 512;

		[Pure]
		public static PooledObject<StringBuilder> GetPooledObject([NotNull] this ObjectPool<StringBuilder> pool) =>
			new PooledObject<StringBuilder>(pool, p => p.AllocateAndClear(), (p, sb) => p.ClearAndFree(sb));

		[Pure]
		public static PooledObject<Stack<TItem>> GetPooledObject<TItem>([NotNull] this ObjectPool<Stack<TItem>> pool) =>
			new PooledObject<Stack<TItem>>(pool, p => p.AllocateAndClear(), (p, obj) => p.ClearAndFree(obj));

		[Pure]
		public static PooledObject<Queue<TItem>> GetPooledObject<TItem>([NotNull] this ObjectPool<Queue<TItem>> pool) =>
			new PooledObject<Queue<TItem>>(pool, p => p.AllocateAndClear(), (p, obj) => p.ClearAndFree(obj));

		[Pure]
		public static PooledObject<HashSet<TItem>> GetPooledObject<TItem>([NotNull] this ObjectPool<HashSet<TItem>> pool) =>
			new PooledObject<HashSet<TItem>>(pool, p => p.AllocateAndClear(), (p, obj) => p.ClearAndFree(obj));

		[Pure]
		public static PooledObject<Dictionary<TKey, TValue>> GetPooledObject<TKey, TValue>([NotNull] this ObjectPool<Dictionary<TKey, TValue>> pool) =>
			new PooledObject<Dictionary<TKey, TValue>>(pool, p => p.AllocateAndClear(), (p, obj) => p.ClearAndFree(obj));

		[Pure]
		public static PooledObject<List<TItem>> GetPooledObject<TItem>([NotNull] this ObjectPool<List<TItem>> pool) =>
			new PooledObject<List<TItem>>(pool, p => p.AllocateAndClear(), (p, obj) => p.ClearAndFree(obj));

		[Pure]
		public static PooledObject<T> GetPooledObject<T>([NotNull] this ObjectPool<T> pool) where T : class =>
			new PooledObject<T>(pool, p => p.Allocate(), (p, o) => p.Free(o));

		[NotNull, Pure]
		public static StringBuilder AllocateAndClear([NotNull] this ObjectPool<StringBuilder> pool)
		{
			var sb = pool.Allocate();
			sb.Clear();

			return sb;
		}

		[NotNull, Pure]
		public static Stack<T> AllocateAndClear<T>([NotNull] this ObjectPool<Stack<T>> pool)
		{
			var set = pool.Allocate();
			set.Clear();

			return set;
		}

		[NotNull, Pure]
		public static Queue<T> AllocateAndClear<T>([NotNull] this ObjectPool<Queue<T>> pool)
		{
			var set = pool.Allocate();
			set.Clear();

			return set;
		}

		[NotNull, Pure]
		public static HashSet<T> AllocateAndClear<T>([NotNull] this ObjectPool<HashSet<T>> pool)
		{
			var set = pool.Allocate();
			set.Clear();

			return set;
		}

		[NotNull, Pure]
		public static Dictionary<TKey, TValue> AllocateAndClear<TKey, TValue>([NotNull] this ObjectPool<Dictionary<TKey, TValue>> pool)
		{
			var map = pool.Allocate();
			map.Clear();

			return map;
		}

		[NotNull, Pure]
		public static List<T> AllocateAndClear<T>([NotNull] this ObjectPool<List<T>> pool)
		{
			var list = pool.Allocate();
			list.Clear();

			return list;
		}

		public static void ClearAndFree([NotNull] this ObjectPool<StringBuilder> pool, StringBuilder sb)
		{
			if (sb == null)
				return;

			sb.Clear();

			if (sb.Capacity > Threshold)
				sb.Capacity = Threshold;

			pool.Free(sb);
		}

		public static void ClearAndFree<T>([NotNull] this ObjectPool<HashSet<T>> pool, HashSet<T> set)
		{
			if (set == null)
				return;

			var count = set.Count;
			set.Clear();

			if (count > Threshold)
				set.TrimExcess();

			pool.Free(set);
		}

		public static void ClearAndFree<T>([NotNull] this ObjectPool<Stack<T>> pool, Stack<T> set)
		{
			if (set == null)
				return;

			var count = set.Count;
			set.Clear();

			if (count > Threshold)
				set.TrimExcess();

			pool.Free(set);
		}

		public static void ClearAndFree<T>([NotNull] this ObjectPool<Queue<T>> pool, Queue<T> set)
		{
			if (set == null)
				return;

			var count = set.Count;
			set.Clear();

			if (count > Threshold)
				set.TrimExcess();

			pool.Free(set);
		}

		public static void ClearAndFree<TKey, TValue>([NotNull] this ObjectPool<Dictionary<TKey, TValue>> pool, Dictionary<TKey, TValue> map)
		{
			if (map == null)
				return;

			// if map grew too big, don't put it back to pool
			if (map.Count > Threshold)
			{
				pool.ForgetTrackedObject(map);
				return;
			}

			map.Clear();
			pool.Free(map);
		}

		public static void ClearAndFree<T>([NotNull] this ObjectPool<List<T>> pool, List<T> list)
		{
			if (list == null)
				return;

			list.Clear();

			if (list.Capacity > Threshold)
				list.Capacity = Threshold;

			pool.Free(list);
		}
	}
}
