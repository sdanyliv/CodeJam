using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace CodeJam.Threading
{
	/// <summary>
	/// Dictionary with lazy values initialization.
	/// </summary>
	/// <remarks>
	/// Thread safe.
	/// </remarks>
	[PublicAPI]
	public class LazyDictionary<TKey, TValue>
	{
		private readonly Func<TKey, TValue> _valueFactory;
		private readonly IEqualityComparer<TKey> _comparer;
		private readonly ConcurrentDictionary<TKey, TValue> _map;

		/// <summary>
		/// Initiaize instance.
		/// </summary>
		/// <param name="valueFactory">Function to create value on demand.</param>
		/// <param name="comparer">Key comparer.</param>
		public LazyDictionary([NotNull] Func<TKey, TValue> valueFactory, IEqualityComparer<TKey> comparer)
		{
			if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));
			_valueFactory = valueFactory;
			_comparer = comparer;
			_map = new ConcurrentDictionary<TKey, TValue>(comparer);
		}

		/// <summary>
		/// Initiaize instance.
		/// </summary>
		/// <param name="valueFactory">Function to create value on demand.</param>
		public LazyDictionary([NotNull] Func<TKey, TValue> valueFactory)
			: this(valueFactory, EqualityComparer<TKey>.Default)
		{ }

		/// <summary>
		/// Returns existing value associated to a key, or create new.
		/// </summary>
		public TValue Get(TKey key) => _map.GetOrAdd(key, _valueFactory);

		/// <summary>
		/// Clears all created values
		/// </summary>
		public void Clear() => _map.Clear();
	}
}