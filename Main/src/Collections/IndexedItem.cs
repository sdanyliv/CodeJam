using System;

namespace CodeJam.Collections 
{
	/// <summary>
	/// Represents an element associated with its index in a sequence.
	/// </summary>
	public struct IndexedItem<T>
	{
		/// <summary>
		/// Gets the value of the element.
		/// </summary>
		/// <returns>
		/// The value of the element.
		/// </returns>
		public T Item { get; }

		/// <summary>
		/// Gets the index of the element in a sequence.
		/// </summary>
		/// <returns>
		/// The index of the element in a sequence.
		/// </returns>
		public int Index { get; }

		/// <summary>
		/// Determines if the value is first in a sequence.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance is first; otherwise, <c>false</c>.
		/// </returns>
		public bool IsFirst { get; }

		/// <summary>
		/// Determines if the value is last in a sequence.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance is last; otherwise, <c>false</c>.
		/// </returns>
		public bool IsLast { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="IndexedItem{T}"/>.
		/// </summary>
		/// <param name="item">The value of the element.</param>
		/// <param name="index">The index of the element in a sequence.</param>
		/// <param name="isFirst">A value indicating whether this instance is first.</param>
		/// <param name="isLast">A value indicating whether this instance is last.</param>
		public IndexedItem(T item, int index, bool isFirst, bool isLast) : this()
		{
			Index = index;
			Item = item;
			IsFirst = isFirst;
			IsLast = isLast;
		}
	}
}
