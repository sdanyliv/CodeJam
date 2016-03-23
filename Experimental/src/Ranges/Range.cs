using System;
using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

namespace CodeJam.Ranges
{
	[Flags]
	public enum RangeOptions
	{
		None = 0,
		HasStart = 0x1,
		HasEnd = 0x2,
		IncludingStart = 0x4,
		IncludingEnd = 0x8,
		IsEmpty = 0x10
	}

	[PublicAPI]
	public static class Range
	{
		[DebuggerStepThrough]
		public static Range<TValue> Create<TValue>(TValue start, TValue end, bool includeStart, bool includeEnd)
			where TValue : IComparable<TValue> =>
				new Range<TValue>(
					start,
					end,
					RangeOptions.HasStart | RangeOptions.HasEnd
						| (includeStart ? RangeOptions.IncludingStart : RangeOptions.None)
						| (includeEnd ? RangeOptions.IncludingEnd : RangeOptions.None));

		[DebuggerStepThrough]
		public static Range<TValue> Create<TValue>(TValue start, TValue end, bool include = true)
			where TValue : IComparable<TValue> =>
				Create(start, end, include, include);

		[DebuggerStepThrough]
		public static Range<TValue> StartsWith<TValue>(TValue start, bool include = true)
			where TValue : IComparable<TValue> =>
				new Range<TValue>(
					start,
					default(TValue),
					RangeOptions.HasStart | (include ? RangeOptions.IncludingStart : RangeOptions.None));

		[DebuggerStepThrough]
		public static Range<TValue> Simple<TValue>(TValue value)
			where TValue : IComparable<TValue> => Create(value, value, true, true);

		[DebuggerStepThrough]
		public static Range<TValue> EndsWith<TValue>(TValue end, bool include = true)
			where TValue : IComparable<TValue> =>
				new Range<TValue>(
					default(TValue),
					end,
					RangeOptions.HasEnd | (include ? RangeOptions.IncludingEnd : RangeOptions.None));

		public static Range<TValue> Empty<TValue>() where TValue : IComparable<TValue> => Range<TValue>.Empty;

		public static Range<TValue> Full<TValue>() where TValue : IComparable<TValue> => Range<TValue>.Full;
	}

	[PublicAPI]
	[DebuggerDisplay("{DisplayValue()}")]
	public struct Range<TValue> : IComparable<Range<TValue>>
		where TValue : IComparable<TValue>
	{
		public static readonly Range<TValue> Empty = new Range<TValue>(default(TValue), default(TValue),
			RangeOptions.IsEmpty);

		public static readonly Range<TValue> Full = new Range<TValue>(default(TValue), default(TValue),
			RangeOptions.None);

		private readonly RangeOptions _options;

		public readonly TValue End;
		public readonly TValue Start;

		[DebuggerStepThrough]
		public Range(TValue start, TValue end, RangeOptions options)
		{
			if (options.HasFlag(RangeOptions.IsEmpty))
			{
				// clear other flags
				_options	= RangeOptions.IsEmpty;
				Start		= default(TValue);
				End			= default(TValue);
				return;
			}

			if (ReferenceEquals(start, null))
				options &= ~RangeOptions.HasStart;

			if (ReferenceEquals(end, null))
				options &= ~RangeOptions.HasEnd;

			if (!options.HasFlag(RangeOptions.HasStart))
			{
				start = default(TValue);
				options &= ~RangeOptions.IncludingStart;
			}

			if (!options.HasFlag(RangeOptions.HasEnd))
			{
				end = default(TValue);
				options &= ~RangeOptions.IncludingEnd;
			}

			_options = options;

			if (!options.HasFlag(RangeOptions.HasStart) || !options.HasFlag(RangeOptions.HasEnd))
			{
				Start = start;
				End = end;
				return;
			}

			Debug.Assert(start != null, "start != null");
			var compare = start.CompareTo(end);
			if (compare > 0)
			{
				throw new ArgumentException("'Start' must be less or equal to the 'End' parameter");
			}

			Start = start;
			End = end;

			if (compare == 0)
			{
				if (!(options.HasFlag(RangeOptions.IncludingStart)
				      || options.HasFlag(RangeOptions.IncludingEnd)))
				{
					// empty
					Start = default(TValue);
					End = default(TValue);
					_options = RangeOptions.IsEmpty;
				}
				else
				{
					// ensure that everything are included
					_options |= RangeOptions.IncludingStart | RangeOptions.IncludingEnd;
				}
			}
		}

		private Range(RangeValue start, RangeValue end)
			: this(start, end, start.Included, end.Included)
		{
		}

		private Range(RangeValue start, RangeValue end, bool icludeStart, bool icludeEnd)
			: this(start.Value, end.Value,
				(start.HasValue ? RangeOptions.HasStart : RangeOptions.None)
				| (end.HasValue ? RangeOptions.HasEnd : RangeOptions.None)
				| (icludeStart  ? RangeOptions.IncludingStart : RangeOptions.None)
				| (icludeEnd    ? RangeOptions.IncludingEnd : RangeOptions.None)
				)
		{
		}

		public bool HasStart => _options.HasFlag(RangeOptions.HasStart);

		public bool HasEnd => _options.HasFlag(RangeOptions.HasEnd);

		public bool IncludeStart => _options.HasFlag(RangeOptions.IncludingStart);

		public bool IncludeEnd => _options.HasFlag(RangeOptions.IncludingEnd);

		public bool IsEmpty => _options.HasFlag(RangeOptions.IsEmpty);

		public bool IsFull => _options == RangeOptions.None;

		private RangeValue StartValue => new RangeValue(Start, !HasStart ? ValueInfo.MinValue : IncludeStart ? ValueInfo.Included : ValueInfo.Excluded);

		private RangeValue EndValue => new RangeValue(End, !HasEnd ? ValueInfo.MaxValue : IncludeEnd ? ValueInfo.Included : ValueInfo.Excluded);

		public int CompareTo(Range<TValue> other)
		{
			var current = this;
			if (!current.HasStart)
			{
				if (!other.HasStart)
					return 0;
				else
					return -1;
			}

			if (!other.HasStart)
				return 1;

			var compare = current.Start.CompareTo(other.Start);
			if (compare == 0)
			{
				if (current.IncludeStart != other.IncludeStart)
				{
					if (current.IncludeStart)
						compare = -1;
					else
						compare = 1;
				}
				else
				{
					if (current.HasEnd && other.HasEnd)
					{
						compare = current.End.CompareTo(other.End);
						if (compare == 0)
						{
							if (current.IncludeEnd != other.IncludeEnd)
							{
								if (current.IncludeEnd)
									compare = 1;
								else
									compare = -1;
							}
						}
					}
				}
			}

			return compare;
		}

		public bool Equals(Range<TValue> other) =>
			_options == other._options
				&& EqualityComparer<TValue>.Default.Equals(End, other.End)
				&& EqualityComparer<TValue>.Default.Equals(Start, other.Start);

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Range<TValue> && Equals((Range<TValue>) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (int) _options;
				hashCode = (hashCode*397) ^ EqualityComparer<TValue>.Default.GetHashCode(End);
				hashCode = (hashCode*397) ^ EqualityComparer<TValue>.Default.GetHashCode(Start);
				return hashCode;
			}
		}

		public Range<TValue> ShiftStart(TValue newStart, bool? include = null) =>
			new Range<TValue>(
				newStart,
				End,
				((include ?? IncludeStart) ? RangeOptions.IncludingStart : RangeOptions.None)
					| (IncludeEnd ? RangeOptions.IncludingEnd : RangeOptions.None));

		public Range<TValue> ShiftEnd(TValue newEnd, bool? include = null) =>
			new Range<TValue>(
				Start,
				newEnd,
				(IncludeStart ? RangeOptions.IncludingStart : RangeOptions.None)
					| ((include ?? IncludeEnd) ? RangeOptions.IncludingEnd : RangeOptions.None));

		public Range<TValue> Union(Range<TValue> other)
		{
			var current = this;

			if (current.IsEmpty)
				return other;

			if (other.IsEmpty)
				return this;

			RangeValue currentStart	= current.StartValue;
			RangeValue currentEnd	= current.EndValue;

			RangeValue otherStart	= other.StartValue;
			RangeValue otherEnd		= other.EndValue;

			var overlap = currentStart <= otherEnd && otherStart <= currentEnd;

			if (!overlap)
				return Empty;

			RangeValue newStart;
			RangeValue newEnd;

			var compareStart = currentStart.CompareTo(otherStart);
			if (compareStart < 0)
				newStart = currentStart;
			else if (compareStart > 0)
				newStart = otherStart;
			else
				newStart = new RangeValue(currentStart.Value,
					!currentStart.HasValue
						? currentStart.ValueInfo
						: (currentStart.Included || otherStart.Included ? ValueInfo.Included : ValueInfo.Excluded));

			var compareEnd = currentEnd.CompareTo(otherEnd);
			if (compareEnd > 0)
				newEnd = currentEnd;
			else if (compareEnd < 0)
				newEnd = otherEnd;
			else
				newEnd = new RangeValue(currentEnd.Value,
					!currentEnd.HasValue
						? currentEnd.ValueInfo
						: (currentEnd.Included || otherEnd.Included ? ValueInfo.Included : ValueInfo.Excluded));

			return new Range<TValue>(newStart, newEnd);
		}

		public bool Contains(TValue value) => Contains(value, true);

		private bool Contains(TValue value, bool included)
		{
			if (IsEmpty)
				return false;

			var result = true;

			if (HasStart)
			{
				var compare = Start.CompareTo(value);
				result = compare < 0 || compare == 0 && IncludeStart && included;
			}

			if (result && HasEnd)
			{
				var compare = End.CompareTo(value);
				result = compare > 0 || compare == 0 && IncludeEnd && included;
			}

			return result;
		}

		public bool IsAdjastent(Range<TValue> other)
		{
			var current = this;
			if (current.HasEnd && other.HasStart)
				return (current.IncludeEnd || other.IncludeStart) && current.End.CompareTo(other.Start) == 0;

			if (other.HasEnd && current.HasStart)
				return (other.IncludeEnd || current.IncludeStart) && other.End.CompareTo(current.Start) == 0;
			return false;
		}

		public bool Overlaps(Range<TValue> other)
		{
			var current = this;

			if (current.IsEmpty || other.IsEmpty)
				return false;

			if (current.IsFull || other.IsFull)
				return true;

			if (!current.HasStart && !other.HasStart)
				return true;

			if (!current.HasEnd && !other.HasEnd)
				return true;

			var result = other.HasStart && current.Contains(other.Start, other.IncludeStart)
			             || other.HasEnd && current.Contains(other.End, other.IncludeEnd)
			             || current.HasStart && other.Contains(current.Start, current.IncludeStart)
			             || current.HasEnd && other.Contains(current.End, current.IncludeEnd);

			return result;
		}

		public IEnumerable<Range<TValue>> Exclude(Range<TValue> other)
		{
			var current = this;

			if (current.IsEmpty || other.IsFull)
			{
				yield return Empty;
				yield break;
			}

			if (other.IsEmpty)
			{
				yield return this;
				yield break;
			}

			if (current.IsFull)
			{
				foreach (var r in other.Invert())
				{
					yield return r;
				}
				yield break;
			}

			RangeValue currentStart	= current.StartValue;
			RangeValue currentEnd	= current.EndValue;

			RangeValue otherStart	= other.StartValue;
			RangeValue otherEnd		= other.EndValue;

			var overlap = currentStart <= otherEnd && otherStart <= currentEnd;

			if (!overlap)
				yield return current; // Nothing to exclude

			var compareStart = currentStart.CompareTo(otherStart);
			if (compareStart <= 0) // currentStart <= otherStart
			{
				// =====---C---------      //
				//      --------O--        //
				//      --------O----      //
				//      --------O--------- //

				if (compareStart == 0)
				{
					if (!otherStart.Included && currentStart.Included)
					{
						var singleValueRange = new Range<TValue>(currentStart, currentStart, true, true);
						yield return singleValueRange;
					}
				}
				else
				{
					var newRange = new Range<TValue>(currentStart, otherStart, currentStart.Included, !otherStart.Included);
					yield return newRange;
				}
			}

			var compareEnd = currentEnd.CompareTo(otherEnd);
			if (compareEnd >= 0) // currentEnd >= otherEnd
			{
				//    --------C----=====      //
				// --------O-------           //
				// --------O------------      //

				if (compareEnd == 0)
				{
					if (!otherEnd.Included && currentEnd.Included)
					{
						var singleValueRange = new Range<TValue>(currentEnd, currentEnd, true, true);
						yield return singleValueRange;
					}
				}
				else
				{
					var newRange = new Range<TValue>(otherEnd, currentEnd, !otherEnd.Included, currentEnd.Included);
					yield return newRange;
				}
			}
		}

		public Range<TValue> Intersect(Range<TValue> other)
		{
			var current = this;

			if (current.IsEmpty || other.IsEmpty)
			{
				return Empty;
			}

			if (other.IsFull)
			{
				return this;
			}

			if (current.IsFull)
			{
				return other;
			}

			RangeValue currentStart	= current.StartValue;
			RangeValue currentEnd	= current.EndValue;

			RangeValue otherStart	= other.StartValue;
			RangeValue otherEnd		= other.EndValue;

			var overlap = currentStart <= otherEnd && otherStart <= currentEnd;

			if (!overlap)
				return Empty;

			RangeValue newStart;
			RangeValue newEnd;

			var compareStart = currentStart.CompareTo(otherStart);
			if (compareStart > 0)
				newStart = currentStart;
			else if (compareStart < 0)
				newStart = otherStart;
			else
				newStart = new RangeValue(currentStart.Value,
					!currentStart.HasValue
						? currentStart.ValueInfo
						: (currentStart.Included && otherStart.Included ? ValueInfo.Included : ValueInfo.Excluded));

			var compareEnd = currentEnd.CompareTo(otherEnd);
			if (compareEnd < 0)
				newEnd = currentEnd;
			else if (compareEnd > 0)
				newEnd = otherEnd;
			else
				newEnd = new RangeValue(currentEnd.Value,
					!currentEnd.HasValue
						? currentEnd.ValueInfo
						: (currentEnd.Included && otherEnd.Included ? ValueInfo.Included : ValueInfo.Excluded));

			if (newStart.CompareTo(newEnd) == 0 && (!newStart.Included || !newEnd.Included))
			{
				return Empty;
			}

			return new Range<TValue>(newStart, newEnd);
		}

		public IEnumerable<Range<TValue>> Invert()
		{
			if (IsEmpty)
				yield return Full;
			else
			{
				if (IsFull)
					yield return Empty;
				else
				{
					if (HasStart)
					{
						yield return
							new Range<TValue>(default(TValue), Start,
								RangeOptions.HasEnd | (IncludeStart ? RangeOptions.None : RangeOptions.IncludingEnd));
					}

					if (HasEnd)
					{
						yield return
							new Range<TValue>(End, default(TValue),
								RangeOptions.HasStart | (IncludeEnd ? RangeOptions.None : RangeOptions.IncludingStart));
					}
				}
			}
		}

		public string DisplayValue()
		{
			if (IsEmpty)
				return string.Empty;
			if (IsFull)
				return "...";

			if (HasStart && HasEnd && Equals(Start, End))
			{
				var value = StartValue.ToString();
				if (!IncludeStart)
					value = "(" + value;
				if (!IncludeEnd)
					value = value + ")";

				return value;
			}

			var startValue = string.Empty;
			var endValue = string.Empty;

			if (HasStart)
			{
				startValue = Start.ToString();
				if (!IncludeStart)
					startValue = "(" + startValue + ")";
			}

			if (HasEnd)
			{
				endValue = End.ToString();
				if (!IncludeEnd)
					endValue = "(" + endValue + ")";
			}

			return startValue + ".." + endValue;
		}


		private enum ValueInfo
		{
			Excluded,
			Included,
			MinValue,
			MaxValue
		}

		[DebuggerDisplay("{ToString()}")]
		private struct RangeValue : IComparable<RangeValue>
		{
			public readonly TValue Value;
			private readonly ValueInfo _valueInfo;

			public ValueInfo ValueInfo => _valueInfo;

			public RangeValue(TValue value, ValueInfo valueInfo)
			{
				Value = value;
				_valueInfo = valueInfo;
			}

			public bool IsMin		=> _valueInfo == ValueInfo.MinValue;
			public bool IsMax		=> _valueInfo == ValueInfo.MaxValue;
			public bool Included	=> _valueInfo == ValueInfo.Included;
			public bool HasValue	=> _valueInfo == ValueInfo.Included || _valueInfo == ValueInfo.Excluded;

			public int CompareTo(RangeValue other)
			{
				var current = this;
				if (current.IsMin)
				{
					if (other.IsMin)
						return 0;
					return -1;
				}

				if (current.IsMax)
				{
					if (other.IsMax)
						return 0;
					return 1;
				}

				if (other.IsMin)
					return 1;

				if (other.IsMax)
					return -1;

				var compare = current.Value.CompareTo(other.Value);
				return compare;
			}

			public override string ToString()
			{
				switch (_valueInfo)
				{
					case ValueInfo.Excluded:
						return $"({Value})";
					case ValueInfo.Included:
						return $"{Value}";
					case ValueInfo.MinValue:
						return "min";
					case ValueInfo.MaxValue:
						return "max";
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			public static bool operator < (RangeValue value1, RangeValue value2)
			{
				var compare = value1.CompareTo(value2);
				return compare < 0;
			}

			public static bool operator >(RangeValue value1, RangeValue value2)
			{
				var compare = value1.CompareTo(value2);
				return compare > 0;
			}

			public static bool operator <=(RangeValue value1, RangeValue value2)
			{
				var compare = value1.CompareTo(value2);
				return compare <= 0;
			}

			public static bool operator >=(RangeValue value1, RangeValue value2)
			{
				var compare = value1.CompareTo(value2);
				return compare >= 0;
			}
		}
	}
}