﻿using System.Collections.Generic;

using CodeJam.Collections;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// Hash code helper methods.
	/// </summary>
	[PublicAPI]
	public static class HashCode
	{
		/// <summary>
		/// Combines hash codes.
		/// </summary>
		/// <param name="h1">Hash code 1</param>
		/// <param name="h2">Hash code 2</param>
		/// <returns>Combined hash code</returns>
		[Pure]
		public static int Combine(int h1, int h2) => unchecked (((h1 << 5) + h1) ^ h2);

		/// <summary>
		/// Combines hash codes.
		/// </summary>
		/// <param name="h1">Hash code 1</param>
		/// <param name="h2">Hash code 2</param>
		/// <param name="h3">Hash code 3</param>
		/// <returns>Combined hash code</returns>
		[Pure]
		public static int Combine(int h1, int h2, int h3) => Combine(Combine(h1, h2), h3);

		/// <summary>
		/// Combines hash codes.
		/// </summary>
		/// <param name="h1">Hash code 1</param>
		/// <param name="h2">Hash code 2</param>
		/// <param name="h3">Hash code 3</param>
		/// <param name="h4">Hash code 4</param>
		/// <returns>Combined hash code</returns>
		[Pure]
		public static int Combine(int h1, int h2, int h3, int h4) => Combine(Combine(h1, h2), Combine(h3, h4));

		/// <summary>
		/// Combines hash codes.
		/// </summary>
		/// <param name="h1">Hash code 1</param>
		/// <param name="h2">Hash code 2</param>
		/// <param name="h3">Hash code 3</param>
		/// <param name="h4">Hash code 4</param>
		/// <param name="h5">Hash code 5</param>
		/// <returns>Combined hash code</returns>
		[Pure]
		public static int Combine(int h1, int h2, int h3, int h4, int h5) => Combine(Combine(h1, h2, h3, h4), h5);

		/// <summary>
		/// Combines hash codes.
		/// </summary>
		/// <param name="h1">Hash code 1</param>
		/// <param name="h2">Hash code 2</param>
		/// <param name="h3">Hash code 3</param>
		/// <param name="h4">Hash code 4</param>
		/// <param name="h5">Hash code 5</param>
		/// <param name="h6">Hash code 6</param>
		/// <returns>Combined hash code</returns>
		[Pure]
		public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6) =>
			Combine(Combine(h1, h2, h3, h4), Combine(h5, h6));

		/// <summary>
		/// Combines hash codes.
		/// </summary>
		/// <param name="h1">Hash code 1</param>
		/// <param name="h2">Hash code 2</param>
		/// <param name="h3">Hash code 3</param>
		/// <param name="h4">Hash code 4</param>
		/// <param name="h5">Hash code 5</param>
		/// <param name="h6">Hash code 6</param>
		/// <param name="h7">Hash code 7</param>
		/// <returns>Combined hash code</returns>
		[Pure]
		public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7) =>
			Combine(Combine(h1, h2, h3, h4), Combine(h5, h6, h7));

		/// <summary>
		/// Combines hash codes.
		/// </summary>
		/// <param name="h1">Hash code 1</param>
		/// <param name="h2">Hash code 2</param>
		/// <param name="h3">Hash code 3</param>
		/// <param name="h4">Hash code 4</param>
		/// <param name="h5">Hash code 5</param>
		/// <param name="h6">Hash code 6</param>
		/// <param name="h7">Hash code 7</param>
		/// <param name="h8">Hash code 8</param>
		/// <returns>Combined hash code</returns>
		[Pure]
		public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8) => Combine(Combine(h1, h2, h3, h4), Combine(h5, h6, h7, h8));

		/// <summary>
		/// Combines hash codes.
		/// </summary>
		/// <param name="values">The collection to combine hash codes.</param>
		/// <returns>
		/// Combined hash code.
		/// </returns>
		[Pure]
		public static int CombineValues<T>([CanBeNull] T[] values)
		{
			if (values.IsNullOrEmpty())
				return 0;

			var hashCode = 0;
			foreach (var value in values)
				if (value != null)
					hashCode = Combine(value.GetHashCode(), hashCode);

			return hashCode;
		}

		/// <summary>
		/// Combines hash codes.
		/// </summary>
		/// <param name="values">The sequence to combine hash codes.</param>
		/// <returns>
		/// Combined hash code.
		/// </returns>
		[Pure]
		public static int CombineValues<T>([CanBeNull, InstantHandle] IEnumerable<T> values)
		{
			if (values == null)
				return 0;

			var hashCode = 0;

			var list = values as IList<T>;
			if (list != null)
			{
				for (int i = 0, count = list.Count; i < count; i++)
				{
					var value = list[i];
					if (value != null)
						hashCode = Combine(value.GetHashCode(), hashCode);
				}
			}
			else
			{
				foreach (var value in values)
					if (value != null)
						hashCode = Combine(value.GetHashCode(), hashCode);
			}

			return hashCode;
		}
	}
}
