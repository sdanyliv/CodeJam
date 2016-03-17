using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// Extensions for <see cref="IDictionary{TKey,TValue}"/>
	/// </summary>
	[PublicAPI]
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Returns value associated with <paramref name="key"/>, or default(TValue) if key does not exists in
		/// <paramref name="dictionary"/>
		/// </summary>
		public static TValue GetValueOrDefault<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			if (dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));
			TValue result;
			return
				dictionary.TryGetValue(key, out result)
					? result
					: default(TValue);
		}

		/// <summary>
		/// Returns value associated with <paramref name="key"/>, or default(TValue) if key does not exists in
		/// <paramref name="dictionary"/>
		/// </summary>
		public static TValue GetValueOrDefault<TKey, TValue>([NotNull] this Dictionary<TKey, TValue> dictionary, TKey key)
		{
			// Resolve ambiguity between IDictionary and IReadOnlyDictionary in System.Dictionary class.
			return GetValueOrDefault((IDictionary<TKey, TValue>)dictionary, key);
		}

		/// <summary>
		/// Returns value associated with <paramref name="key"/>, or default(TValue) if key does not exists in
		/// <paramref name="dictionary"/>
		/// </summary>
		public static TValue GetValueOrDefault<TKey, TValue>(
			[NotNull] this IReadOnlyDictionary<TKey, TValue> dictionary,
			TKey key)
		{
			if (dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));
			TValue result;
			return
				dictionary.TryGetValue(key, out result)
					? result
					: default(TValue);
		}
	}
}