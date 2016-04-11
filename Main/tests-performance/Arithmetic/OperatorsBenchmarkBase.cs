using System;

using BenchmarkDotNet.Attributes;

using JetBrains.Annotations;

namespace CodeJam.Arithmetic
{
	/// <summary>
	/// Base class for all operator test cases;
	/// </summary>
	public abstract class OperatorsBenchmarkBase<T, TStorage>
	{
		protected int Count { get; set; } = 1000 * 1000;
		protected int ValueBRepeats { get; set; } = 5;

		protected T[] ValuesA;
		protected T[] ValuesB;
		protected TStorage Storage;

		protected abstract T GetValueA(int i);
		protected abstract T GetValueB(int i);

		[Setup]
		[UsedImplicitly]
		public void Setup()
		{
			var count = Count;
			ValuesA = new T[count];
			ValuesB = new T[count];
			for (var i = 0; i < count; i++)
			{
				ValuesA[i] = GetValueA(i);
				ValuesB[i] = GetValueB(i);
			}
		}
	}

	public abstract class IntOperatorsBenchmark<TStorage> : OperatorsBenchmarkBase<int, TStorage>
	{
		protected override int GetValueA(int i) => i;
		protected override int GetValueB(int i) => i % ValueBRepeats;
	}

	public abstract class NullableIntOperatorsBenchmark<TStorage> : OperatorsBenchmarkBase<int?, TStorage>
	{
		protected override int? GetValueA(int i) => i;

		protected override int? GetValueB(int i)
		{
			int? b = i % ValueBRepeats;
			if (b == 0)
				b = null;
			return b;
		}
	}

	public abstract class NullableDateTimeOperatorsBenchmark<TStorage> : OperatorsBenchmarkBase<DateTime?, TStorage>
	{
		protected override DateTime? GetValueA(int i) => DateTime.UtcNow;

		protected override DateTime? GetValueB(int i)
		{
			var i2 = i % ValueBRepeats;
			return i2 == 0 ? (DateTime?)null : DateTime.UtcNow.AddDays(i2);
		}
	}

	public abstract class StringOperatorsBenchmark<TStorage> : OperatorsBenchmarkBase<string, TStorage>
	{
		protected StringOperatorsBenchmark()
		{
			Count /= 5;
		}

		protected override string GetValueA(int i) => i.ToString();

		protected override string GetValueB(int i)
		{
			var i2 = i % ValueBRepeats;
			return i2 == 0 ? null : i2.ToString();
		}
	}
}