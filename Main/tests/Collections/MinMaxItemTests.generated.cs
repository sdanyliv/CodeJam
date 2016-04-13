using System;
using System.Linq;

using CodeJam.Collections;

using NUnit.Framework;

namespace CodeJam
{
	[TestFixture]
	partial class MinMaxItemTests
	{
		#region MinItem

		#region Int32

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Int32 Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Int32 Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Int32 Empty", ExpectedResult = null)]
		public Int32? MinItemInt32(int[] source)
			=> source.Select(v => new Item<Int32>((Int32)v)). MinItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Int32 Nullable Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Int32 Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Int32 Nullable Empty",  ExpectedResult = null)]
		public Int32?  MinItemInt32Nullable(int[] source)
			=> source.Select(v => new Item<Int32?>((Int32)v)). MinItem(i => i.Value)?.Value;

		#endregion

		#region Int64

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Int64 Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Int64 Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Int64 Empty", ExpectedResult = null)]
		public Int64? MinItemInt64(int[] source)
			=> source.Select(v => new Item<Int64>((Int64)v)). MinItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Int64 Nullable Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Int64 Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Int64 Nullable Empty",  ExpectedResult = null)]
		public Int64?  MinItemInt64Nullable(int[] source)
			=> source.Select(v => new Item<Int64?>((Int64)v)). MinItem(i => i.Value)?.Value;

		#endregion

		#region Single

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Single Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Single Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Single Empty", ExpectedResult = null)]
		public Single? MinItemSingle(int[] source)
			=> source.Select(v => new Item<Single>((Single)v)). MinItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Single Nullable Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Single Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Single Nullable Empty",  ExpectedResult = null)]
		public Single?  MinItemSingleNullable(int[] source)
			=> source.Select(v => new Item<Single?>((Single)v)). MinItem(i => i.Value)?.Value;

		#endregion

		#region Double

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Double Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Double Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Double Empty", ExpectedResult = null)]
		public Double? MinItemDouble(int[] source)
			=> source.Select(v => new Item<Double>((Double)v)). MinItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Double Nullable Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Double Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Double Nullable Empty",  ExpectedResult = null)]
		public Double?  MinItemDoubleNullable(int[] source)
			=> source.Select(v => new Item<Double?>((Double)v)). MinItem(i => i.Value)?.Value;

		#endregion

		#region Decimal

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Decimal Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Decimal Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Decimal Empty", ExpectedResult = null)]
		public Decimal? MinItemDecimal(int[] source)
			=> source.Select(v => new Item<Decimal>((Decimal)v)). MinItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Decimal Nullable Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Decimal Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Decimal Nullable Empty",  ExpectedResult = null)]
		public Decimal?  MinItemDecimalNullable(int[] source)
			=> source.Select(v => new Item<Decimal?>((Decimal)v)). MinItem(i => i.Value)?.Value;

		#endregion

		#region Int16

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Int16 Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Int16 Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Int16 Empty", ExpectedResult = null)]
		public Int16? MinItemInt16(int[] source)
			=> source.Select(v => new Item<Int16>((Int16)v)). MinItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Int16 Nullable Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Int16 Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Int16 Nullable Empty",  ExpectedResult = null)]
		public Int16?  MinItemInt16Nullable(int[] source)
			=> source.Select(v => new Item<Int16?>((Int16)v)). MinItem(i => i.Value)?.Value;

		#endregion

		#region SByte

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem SByte Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem SByte Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem SByte Empty", ExpectedResult = null)]
		public SByte? MinItemSByte(int[] source)
			=> source.Select(v => new Item<SByte>((SByte)v)). MinItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem SByte Nullable Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem SByte Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem SByte Nullable Empty",  ExpectedResult = null)]
		public SByte?  MinItemSByteNullable(int[] source)
			=> source.Select(v => new Item<SByte?>((SByte)v)). MinItem(i => i.Value)?.Value;

		#endregion

		#region UInt32

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem UInt32 Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem UInt32 Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem UInt32 Empty", ExpectedResult = null)]
		public UInt32? MinItemUInt32(int[] source)
			=> source.Select(v => new Item<UInt32>((UInt32)v)). MinItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem UInt32 Nullable Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem UInt32 Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem UInt32 Nullable Empty",  ExpectedResult = null)]
		public UInt32?  MinItemUInt32Nullable(int[] source)
			=> source.Select(v => new Item<UInt32?>((UInt32)v)). MinItem(i => i.Value)?.Value;

		#endregion

		#region UInt64

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem UInt64 Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem UInt64 Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem UInt64 Empty", ExpectedResult = null)]
		public UInt64? MinItemUInt64(int[] source)
			=> source.Select(v => new Item<UInt64>((UInt64)v)). MinItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem UInt64 Nullable Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem UInt64 Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem UInt64 Nullable Empty",  ExpectedResult = null)]
		public UInt64?  MinItemUInt64Nullable(int[] source)
			=> source.Select(v => new Item<UInt64?>((UInt64)v)). MinItem(i => i.Value)?.Value;

		#endregion

		#region UInt16

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem UInt16 Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem UInt16 Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem UInt16 Empty", ExpectedResult = null)]
		public UInt16? MinItemUInt16(int[] source)
			=> source.Select(v => new Item<UInt16>((UInt16)v)). MinItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem UInt16 Nullable Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem UInt16 Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem UInt16 Nullable Empty",  ExpectedResult = null)]
		public UInt16?  MinItemUInt16Nullable(int[] source)
			=> source.Select(v => new Item<UInt16?>((UInt16)v)). MinItem(i => i.Value)?.Value;

		#endregion

		#region Byte

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Byte Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Byte Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Byte Empty", ExpectedResult = null)]
		public Byte? MinItemByte(int[] source)
			=> source.Select(v => new Item<Byte>((Byte)v)). MinItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Byte Nullable Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Byte Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Byte Nullable Empty",  ExpectedResult = null)]
		public Byte?  MinItemByteNullable(int[] source)
			=> source.Select(v => new Item<Byte?>((Byte)v)). MinItem(i => i.Value)?.Value;

		#endregion

		#endregion

		#region MaxItem

		#region Int32

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Int32 Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Int32 Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Int32 Empty", ExpectedResult = null)]
		public Int32? MaxItemInt32(int[] source)
			=> source.Select(v => new Item<Int32>((Int32)v)). MaxItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Int32 Nullable Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Int32 Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Int32 Nullable Empty",  ExpectedResult = null)]
		public Int32?  MaxItemInt32Nullable(int[] source)
			=> source.Select(v => new Item<Int32?>((Int32)v)). MaxItem(i => i.Value)?.Value;

		#endregion

		#region Int64

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Int64 Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Int64 Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Int64 Empty", ExpectedResult = null)]
		public Int64? MaxItemInt64(int[] source)
			=> source.Select(v => new Item<Int64>((Int64)v)). MaxItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Int64 Nullable Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Int64 Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Int64 Nullable Empty",  ExpectedResult = null)]
		public Int64?  MaxItemInt64Nullable(int[] source)
			=> source.Select(v => new Item<Int64?>((Int64)v)). MaxItem(i => i.Value)?.Value;

		#endregion

		#region Single

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Single Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Single Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Single Empty", ExpectedResult = null)]
		public Single? MaxItemSingle(int[] source)
			=> source.Select(v => new Item<Single>((Single)v)). MaxItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Single Nullable Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Single Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Single Nullable Empty",  ExpectedResult = null)]
		public Single?  MaxItemSingleNullable(int[] source)
			=> source.Select(v => new Item<Single?>((Single)v)). MaxItem(i => i.Value)?.Value;

		#endregion

		#region Double

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Double Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Double Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Double Empty", ExpectedResult = null)]
		public Double? MaxItemDouble(int[] source)
			=> source.Select(v => new Item<Double>((Double)v)). MaxItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Double Nullable Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Double Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Double Nullable Empty",  ExpectedResult = null)]
		public Double?  MaxItemDoubleNullable(int[] source)
			=> source.Select(v => new Item<Double?>((Double)v)). MaxItem(i => i.Value)?.Value;

		#endregion

		#region Decimal

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Decimal Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Decimal Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Decimal Empty", ExpectedResult = null)]
		public Decimal? MaxItemDecimal(int[] source)
			=> source.Select(v => new Item<Decimal>((Decimal)v)). MaxItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Decimal Nullable Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Decimal Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Decimal Nullable Empty",  ExpectedResult = null)]
		public Decimal?  MaxItemDecimalNullable(int[] source)
			=> source.Select(v => new Item<Decimal?>((Decimal)v)). MaxItem(i => i.Value)?.Value;

		#endregion

		#region Int16

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Int16 Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Int16 Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Int16 Empty", ExpectedResult = null)]
		public Int16? MaxItemInt16(int[] source)
			=> source.Select(v => new Item<Int16>((Int16)v)). MaxItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Int16 Nullable Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Int16 Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Int16 Nullable Empty",  ExpectedResult = null)]
		public Int16?  MaxItemInt16Nullable(int[] source)
			=> source.Select(v => new Item<Int16?>((Int16)v)). MaxItem(i => i.Value)?.Value;

		#endregion

		#region SByte

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem SByte Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem SByte Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem SByte Empty", ExpectedResult = null)]
		public SByte? MaxItemSByte(int[] source)
			=> source.Select(v => new Item<SByte>((SByte)v)). MaxItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem SByte Nullable Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem SByte Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem SByte Nullable Empty",  ExpectedResult = null)]
		public SByte?  MaxItemSByteNullable(int[] source)
			=> source.Select(v => new Item<SByte?>((SByte)v)). MaxItem(i => i.Value)?.Value;

		#endregion

		#region UInt32

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem UInt32 Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem UInt32 Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem UInt32 Empty", ExpectedResult = null)]
		public UInt32? MaxItemUInt32(int[] source)
			=> source.Select(v => new Item<UInt32>((UInt32)v)). MaxItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem UInt32 Nullable Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem UInt32 Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem UInt32 Nullable Empty",  ExpectedResult = null)]
		public UInt32?  MaxItemUInt32Nullable(int[] source)
			=> source.Select(v => new Item<UInt32?>((UInt32)v)). MaxItem(i => i.Value)?.Value;

		#endregion

		#region UInt64

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem UInt64 Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem UInt64 Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem UInt64 Empty", ExpectedResult = null)]
		public UInt64? MaxItemUInt64(int[] source)
			=> source.Select(v => new Item<UInt64>((UInt64)v)). MaxItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem UInt64 Nullable Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem UInt64 Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem UInt64 Nullable Empty",  ExpectedResult = null)]
		public UInt64?  MaxItemUInt64Nullable(int[] source)
			=> source.Select(v => new Item<UInt64?>((UInt64)v)). MaxItem(i => i.Value)?.Value;

		#endregion

		#region UInt16

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem UInt16 Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem UInt16 Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem UInt16 Empty", ExpectedResult = null)]
		public UInt16? MaxItemUInt16(int[] source)
			=> source.Select(v => new Item<UInt16>((UInt16)v)). MaxItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem UInt16 Nullable Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem UInt16 Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem UInt16 Nullable Empty",  ExpectedResult = null)]
		public UInt16?  MaxItemUInt16Nullable(int[] source)
			=> source.Select(v => new Item<UInt16?>((UInt16)v)). MaxItem(i => i.Value)?.Value;

		#endregion

		#region Byte

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Byte Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Byte Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Byte Empty", ExpectedResult = null)]
		public Byte? MaxItemByte(int[] source)
			=> source.Select(v => new Item<Byte>((Byte)v)). MaxItem(i => i.Value)?.Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Byte Nullable Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Byte Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Byte Nullable Empty",  ExpectedResult = null)]
		public Byte?  MaxItemByteNullable(int[] source)
			=> source.Select(v => new Item<Byte?>((Byte)v)). MaxItem(i => i.Value)?.Value;

		#endregion

		#endregion

	}
}