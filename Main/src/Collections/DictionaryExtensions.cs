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
		#region GetValueOrDefault
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
			return GetValueOrDefault((IDictionary<TKey, TValue>) dictionary, key);
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
		#endregion

		#region GetOrAdd, AddOrUpdate
		/// <summary>
		/// Returns value by <paramref name="key"/>, or adds specified <paramref name="value"/> to dictionary.
		/// </summary>
		public static TValue GetOrAdd<TKey, TValue>(
			[NotNull] this IDictionary<TKey, TValue> dictionary,
			TKey key,
			TValue value)
		{
			if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
			TValue result;
			if (dictionary.TryGetValue(key, out result))
				return result;
			dictionary.Add(key, value);
			return value;
		}

		/// <summary>
		/// Returns value by <paramref name="key"/>, or adds value created by <paramref name="valueFactory"/> to dictionary.
		/// </summary>
		public static TValue GetOrAdd<TKey, TValue>(
			[NotNull] this IDictionary<TKey, TValue> dictionary,
			TKey key,
			Func<TKey, TValue> valueFactory)
		{
			if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
			TValue result;
			if (dictionary.TryGetValue(key, out result))
				return result;
			var value = valueFactory(key);
			dictionary.Add(key, value);
			return value;
		}
		#endregion
	}
}