using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using JetBrains.Annotations;

namespace CodeJam.Ranges
{
	[PublicAPI]
	[DebuggerDisplay("{DisplayValue()}")]
	public class ImmutableRangeList<TValue> : IEnumerable<Range<TValue>>
		where TValue : IComparable<TValue>
	{
		public static readonly ImmutableRangeList<TValue> Empty = new ImmutableRangeList<TValue>();
		public static readonly ImmutableRangeList<TValue> Full = new ImmutableRangeList<TValue>(Range<TValue>.Full);
		internal readonly List<Range<TValue>> Values;
		private int? _hashCode;

		internal ImmutableRangeList(Range<TValue> range)
		{
			Values = new List<Range<TValue>> {range};
		}

		internal ImmutableRangeList()
		{
		}

		internal ImmutableRangeList(List<Range<TValue>> values)
		{
			Values = values;
		}

		public bool IsEmpty => Values == null;

		public bool IsFull =>
			Values != null
				&& Values.Count == 1
				&& Values[0].IsFull;

		public IEnumerator<Range<TValue>> GetEnumerator()
		{
			if (Values == null)
				return Enumerable.Empty<Range<TValue>>().GetEnumerator();
			return Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		protected bool Equals(ImmutableRangeList<TValue> other)
		{
			if (IsFull && other.IsFull)
				return true;
			if (IsEmpty && other.IsEmpty)
				return true;

			if (IsEmpty)
				return false;
			if (other.IsEmpty)
				return false;

			if (Values.Count != other.Values.Count)
				return false;

			for (var i = 0; i < Values.Count; i++)
			{
				if (!Equals(Values[i], other.Values[i]))
					return false;
			}

			return true;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((ImmutableRangeList<TValue>) obj);
		}

		public override int GetHashCode()
		{
			// ReSharper disable NonReadonlyMemberInGetHashCode
			if (!_hashCode.HasValue)
				_hashCode = CalculateHashCode();

			return _hashCode.Value;
			// ReSharper restore NonReadonlyMemberInGetHashCode
		}

		private int CalculateHashCode()
		{
			if (Values == null)
				return 0;
			var res = 0x2D2816FE;
			foreach (var t in Values)
				res = res*31 + t.GetHashCode();
			return res;
		}

		public string DisplayValue()
		{
			string result;

			if (IsEmpty)
			{
				result = string.Empty;
			}
			else if (IsFull)
			{
				result = "...";
			}
			else
			{
				result = string.Join(", ", Values.Select(v => v.DisplayValue()));
			}

			return "[" + result + "]";
		}

		public ImmutableRangeList<TValue> Add(Range<TValue> range)
		{
			if (range.IsEmpty)
				return this;

			if (Values == null)
				return new ImmutableRangeList<TValue>(range);

			return this.Concat(new[] {range}).ToRangeList();
		}

		public ImmutableRangeList<TValue> AddRange(IEnumerable<Range<TValue>> ranges)
		{
			if (ranges == null)
				return this;

			return Values == null ? ranges.ToRangeList() : Values.Concat(ranges).ToRangeList();
		}

		public ImmutableRangeList<TValue> RemoveRange(IEnumerable<Range<TValue>> ranges)
		{
			if (Values == null)
				return this;

			if (ranges == null)
				return this;

			var rangesList = ranges.ToList();

			var sourceList = Values;
			List<Range<TValue>> copyValues = null;

			foreach (var toRemove in rangesList)
			{
				var i = 0;
				do
				{
					var current = sourceList[i];
					if (current.Intersects(toRemove))
					{
						var removed = current.Exclude(toRemove);
						if (copyValues == null)
						{
							copyValues = Values.ToList();
							sourceList = copyValues;
						}
						sourceList.RemoveAt(i);
						var prevCount = sourceList.Count;
						sourceList.InsertRange(i, removed);
						i += sourceList.Count - prevCount;
					}
					else
					{
						++i;
					}
				} while (i < sourceList.Count);
			}

			if (copyValues == null)
				return this;

			return copyValues.ToRangeList();
		}

		public ImmutableRangeList<TValue> Invert()
		{
			if (IsFull)
				return Empty;
			if (IsEmpty)
				return Full;

			return Full.RemoveRange(Values);
		}

		public ImmutableRangeList<TValue> Intersect(IEnumerable<Range<TValue>> ranges)
		{
			if (Values == null || ranges == null)
				return this;

			var intersected = ranges
				.SelectMany(r => Values, (r, v) => r.Intersect(v))
				.SelectMany(outer => outer);

			return intersected.ToRangeList();
		}


		public bool ConatinsValue(TValue value)
		{
			if (IsEmpty)
				return false;
			if (IsFull)
				return true;

			var index = Values.BinarySearch(Range.Simple(value));
			return index >= 0;
		}

		#region Operators
		public static ImmutableRangeList<TValue> operator ~(ImmutableRangeList<TValue> list) => list.Invert();

		public static ImmutableRangeList<TValue> operator +(
			ImmutableRangeList<TValue> list1, ImmutableRangeList<TValue> list2) => list1.AddRange(list2);

		public static ImmutableRangeList<TValue> operator -(
			ImmutableRangeList<TValue> list1,
			ImmutableRangeList<TValue> list2) =>
				list1.RemoveRange(list2);

		public static ImmutableRangeList<TValue> operator &(
			ImmutableRangeList<TValue> list1,
			ImmutableRangeList<TValue> list2) =>
				list1.Intersect(list2);

		public static ImmutableRangeList<TValue> operator |(
			ImmutableRangeList<TValue> list1,
			ImmutableRangeList<TValue> list2) =>
				list1.AddRange(list2);

		public static ImmutableRangeList<TValue> operator !(ImmutableRangeList<TValue> list) => list.Invert();
		#endregion
	}
}