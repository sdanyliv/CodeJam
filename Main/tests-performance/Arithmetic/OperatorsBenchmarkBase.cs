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
		protected int ValueARepeats { get; set; } = 5;
		protected int ValueAOffset { get; set; } = 0;

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
				ValuesA[i] = GetValueA(i % ValueARepeats + ValueAOffset);
				ValuesB[i] = GetValueB(i + 1);
			}
		}
	}

	public abstract class IntOperatorsBenchmark<TStorage> : OperatorsBenchmarkBase<int, TStorage>
	{
		protected override int GetValueA(int i) => i;
		protected override int GetValueB(int i) => i;
	}

	public abstract class NullableIntOperatorsBenchmark<TStorage> : OperatorsBenchmarkBase<int?, TStorage>
	{
		protected override int? GetValueA(int i) => i == 0 ? null : (int?)i;

		protected override int? GetValueB(int i) => i;
	}

	public abstract class NullableDoubleOperatorsBenchmark<TStorage> : OperatorsBenchmarkBase<double?, TStorage>
	{
		protected override double? GetValueA(int i) => i == 0 ? null : (int?)i;

		protected override double? GetValueB(int i) => i;
	}

	public abstract class NullableDateTimeOperatorsBenchmark<TStorage> : OperatorsBenchmarkBase<DateTime?, TStorage>
	{
		protected override DateTime? GetValueA(int i) =>
			i == 0 ? (DateTime?)null : DateTime.UtcNow.AddDays(i);

		protected override DateTime? GetValueB(int i) => DateTime.UtcNow;
	}

	public abstract class StringOperatorsBenchmark<TStorage> : OperatorsBenchmarkBase<string, TStorage>
	{
		protected StringOperatorsBenchmark()
		{
			Count /= 5;
		}

		protected override string GetValueA(int i) => i == 0 ? null : i.ToString();

		protected override string GetValueB(int i) => i.ToString();
	}
}