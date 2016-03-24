using System;

using JetBrains.Annotations;

namespace CodeJam.Collections
{
	/// <summary>
	/// Dictionary with lazy values initialization.
	/// </summary>
	/// <typeparam name="TKey">Type of key</typeparam>
	/// <typeparam name="TValue">Type of value</typeparam>
	[PublicAPI]
	public interface ILazyDictionary<in TKey, out TValue>
	{
		/// <summary>
		/// Clears all created values
		/// </summary>
		void Clear();

		/// <summary>
		/// Returns existing value associated to a key, or creates new.
		/// </summary>
		TValue Get(TKey key);
	}
}