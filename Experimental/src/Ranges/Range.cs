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
		public static Range<TValue> Create<TValue>(TValue start, TValue end, bool includeStart, bool includeEnd)
			where TValue : IComparable<TValue> =>
				new Range<TValue>(
					start,
					end,
					RangeOptions.HasStart | RangeOptions.HasEnd
						| (includeStart ? RangeOptions.IncludingStart : RangeOptions.None)
						| (includeEnd ? RangeOptions.IncludingEnd : RangeOptions.None));

		public static Range<TValue> Create<TValue>(TValue start, TValue end, bool include = true)
			where TValue : IComparable<TValue> =>
				new Range<TValue>(
					start,
					end,
					RangeOptions.HasStart | RangeOptions.HasEnd
						| (include ? RangeOptions.IncludingStart | RangeOptions.IncludingEnd : RangeOptions.None));

		public static Range<TValue> StartsWith<TValue>(TValue start, bool include = true)
			where TValue : IComparable<TValue> =>
				new Range<TValue>(
					start,
					default(TValue),
					RangeOptions.HasStart | (include ? RangeOptions.IncludingStart : RangeOptions.None));

		public static Range<TValue> Simple<TValue>(TValue value)
			where TValue : IComparable<TValue> => Create(value, value, true, true);

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

		public Range(TValue start, TValue end, RangeOptions options)
		{
			if (options.HasFlag(RangeOptions.IsEmpty))
			{
				// clear other flags
				options = RangeOptions.IsEmpty;
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

			if (options.HasFlag(RangeOptions.HasStart) && options.HasFlag(RangeOptions.HasEnd))
			{
				Debug.Assert(start != null, "start != null");
				var compare = start.CompareTo(end);
				if (compare > 0)
				{
					// clear including flags before split
					_options &= ~(RangeOptions.IncludingStart | RangeOptions.IncludingEnd);

					Start = end;
					End = start;
					if (options.HasFlag(RangeOptions.IncludingStart))
						_options |= RangeOptions.IncludingEnd;
					if (options.HasFlag(RangeOptions.IncludingEnd))
						_options |= RangeOptions.IncludingEnd;
				}
				else
				{
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
							// ensure that everyting included
							_options |= RangeOptions.IncludingStart | RangeOptions.IncludingEnd;
						}
					}
				}
			}
			else
			{
				Start = start;
				End = end;
			}
		}

		private Range(RangeValue start, RangeValue end)
			: this(start.Value, end.Value,
				(start.HasValue ? RangeOptions.HasStart : RangeOptions.None)
				| (end.HasValue ? RangeOptions.HasEnd : RangeOptions.None)
				| (start.Included ? RangeOptions.IncludingStart : RangeOptions.None)
				| (end.Included ? RangeOptions.IncludingEnd : RangeOptions.None)
				)
		{
		}

		public bool HasStart => _options.HasFlag(RangeOptions.HasStart);

		public bool HasEnd => _options.HasFlag(RangeOptions.HasEnd);

		public bool IncludeStart => _options.HasFlag(RangeOptions.IncludingStart);

		public bool IncludeEnd => _options.HasFlag(RangeOptions.IncludingEnd);

		public bool IsEmpty => _options.HasFlag(RangeOptions.IsEmpty);

		public bool IsFull => _options == RangeOptions.None;

		private RangeValue StartValue => new RangeValue(Start, HasStart, IncludeStart);

		private RangeValue EndValue => new RangeValue(End, HasEnd, IncludeEnd);

		public int CompareTo(Range<TValue> other)
		{
			var current = this;
			if (!current.HasStart)
				if (!other.HasStart)
					return 0;
				else
					return -1;

			if (!other.HasStart)
				return 1;

			var compare = current.Start.CompareTo(other.Start);
			if (compare == 0)
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

		public Range<TValue> SetStart(TValue newStart, bool? include = null) =>
			new Range<TValue>(
				newStart,
				End,
				((include ?? IncludeStart) ? RangeOptions.IncludingStart : RangeOptions.None)
					| (IncludeEnd ? RangeOptions.IncludingEnd : RangeOptions.None));

		public Range<TValue> SetEnd(TValue newEnd, bool? include = null) =>
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

			var newStart = RangeValue.MinValue(other.StartValue, current.StartValue, false);
			var newEnd = RangeValue.MaxValue(other.EndValue, current.EndValue, false);

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

		public bool Intersects(Range<TValue> other)
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

			// ?

			if (current.HasStart)
			{
				// cS---------------?
				// ?

				if (other.HasStart)
				{
					//    cS---------------?
					// oS------------------?
					//          oS---------?

					var compare = current.Start.CompareTo(other.Start);
					if (compare <= 0)
					{
						//    cS=====----------?
						//          oS---------?


						if (compare == 0)
						{
							//          cS--------------?
							//          oS---------?

							if (current.IncludeStart && !other.IncludeStart)
							{
								var newRange = new Range<TValue>(current.Start, current.Start,
									RangeOptions.HasStart | RangeOptions.HasEnd
									| RangeOptions.IncludingStart
									| RangeOptions.IncludingEnd
									);
								yield return newRange;
							}
						}

						else
						{
							var newRange = new Range<TValue>(current.Start, other.Start,
								RangeOptions.HasStart | RangeOptions.HasEnd
								| (current.IncludeStart ? RangeOptions.IncludingStart : RangeOptions.None)
								| (other.IncludeStart ? RangeOptions.None : RangeOptions.IncludingEnd) // ivert flag
								);

							if (!newRange.IsEmpty)
								yield return newRange;
						}
					}
				}
			}

			if (current.HasEnd)
			{
				// ?---------------cE
				// ?

				if (other.HasEnd)
				{
					//    ?----------------cE
					//          oS-----oE
					//          oS-------------oE

					var compare = other.End.CompareTo(current.End);
					if (compare <= 0)
					{
						//    ?-------------===cE
						//          oS-----oE

						if (compare == 0)
						{
							//    ?------------cE
							//          oS-----oE

							if (current.IncludeEnd && !other.IncludeEnd)
							{
								var newRange = new Range<TValue>(other.End, other.End,
									RangeOptions.HasStart | RangeOptions.HasEnd
									| RangeOptions.IncludingStart
									| RangeOptions.IncludingEnd
									);
								yield return newRange;
							}
						}
						else
						{
							var newRange = new Range<TValue>(other.End, current.End,
								RangeOptions.HasStart | RangeOptions.HasEnd
								| (other.IncludeEnd ? RangeOptions.None : RangeOptions.IncludingStart) // ivert flag
								| (current.IncludeEnd ? RangeOptions.IncludingEnd : RangeOptions.None)
								);

							if (!newRange.IsEmpty)
								yield return newRange;
						}
					}
				}
			}
			else
			{
				// cS---------------
				// ?
				if (other.HasEnd)
				{
					var newRange = new Range<TValue>(other.End, default(TValue),
						RangeOptions.HasStart
						| (other.IncludeEnd ? RangeOptions.None : RangeOptions.IncludingStart) // ivert flag
						);

					if (!newRange.IsEmpty)
						yield return newRange;
				}
			}
		}

		public IEnumerable<Range<TValue>> Intersect(Range<TValue> other)
		{
			var current = this;

			if (current.IsEmpty || other.IsEmpty)
			{
				yield return Empty;
				yield break;
			}

			if (other.IsFull)
			{
				yield return this;
				yield break;
			}

			if (current.IsFull)
			{
				yield return other;
				yield break;
			}


			if (!current.Intersects(other))
				yield break;

			RangeValue newStart;
			RangeValue newEnd;

			if (current.HasStart)
			{
				newStart =
					other.HasStart ? RangeValue.MaxValue(current.StartValue, other.StartValue, true) : current.StartValue;
			}
			else
			{
				newStart = other.StartValue;
			}

			if (current.HasEnd)
				newEnd =
					other.HasEnd
						? RangeValue.MinValue(current.EndValue, other.EndValue, true)
						: current.EndValue;
			else
				newEnd = other.EndValue;

			yield return new Range<TValue>(newStart, newEnd);
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

		private struct RangeValue
		{
			public readonly bool HasValue;
			public readonly bool Included;
			public readonly TValue Value;

			public RangeValue(TValue value, bool hasValue, bool included)
			{
				Value = value;
				HasValue = hasValue;
				Included = included;
			}

			public override string ToString()
			{
				if (!HasValue)
					return string.Empty;
				if (!Included)
					return $"({Value})";
				return Value.ToString();
			}

			public static RangeValue MinValue(RangeValue value1, RangeValue value2, bool isAnd)
			{
				if (!value1.HasValue)
					return value1;
				if (!value2.HasValue)
					return value2;

				var compare = value1.Value.CompareTo(value2.Value);
				if (compare > 0)
					return value2;

				var newIncluded = value1.Included;
				if (compare == 0)
				{
					if (isAnd)
						newIncluded = value2.Included && newIncluded;
					else
						newIncluded = value2.Included || newIncluded;
				}

				return new RangeValue(value1.Value, true, newIncluded);
			}

			public static RangeValue MaxValue(RangeValue value1, RangeValue value2, bool isAnd)
			{
				if (!value1.HasValue)
					return value1;
				if (!value2.HasValue)
					return value2;

				var compare = value1.Value.CompareTo(value2.Value);
				if (compare < 0)
					return value2;

				var newIncluded = value1.Included;
				if (compare == 0)
				{
					if (isAnd)
						newIncluded = value2.Included && newIncluded;
					else
						newIncluded = value2.Included || newIncluded;
				}

				return new RangeValue(value1.Value, true, newIncluded);
			}
		}
	}
}