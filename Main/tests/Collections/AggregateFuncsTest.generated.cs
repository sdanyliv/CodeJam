using System;
using System.Linq;

using NUnit.Framework;

namespace CodeJam
{
	[TestFixture]
	partial class AggregateFuncsTest
	{
		#region MinItem

		#region Int32

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Int32 Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Int32 Single", ExpectedResult = 1)]
		public Int32  MinItemInt32(int[] source)
			=> source.Select(v => new Item<Int32>(v)). MinItem(i => i.Value).Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Int32 Nullable Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Int32 Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Int32 Nullable Empty",  ExpectedResult = null)]
		public Int32?  MinItemInt32Nullable(int[] source)
			=> source.Select(v => new Item<Int32?>(v)). MinItem(i => i.Value)?.Value;

		#endregion

		#region Int64

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Int64 Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Int64 Single", ExpectedResult = 1)]
		public Int64  MinItemInt64(int[] source)
			=> source.Select(v => new Item<Int64>(v)). MinItem(i => i.Value).Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Int64 Nullable Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Int64 Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Int64 Nullable Empty",  ExpectedResult = null)]
		public Int64?  MinItemInt64Nullable(int[] source)
			=> source.Select(v => new Item<Int64?>(v)). MinItem(i => i.Value)?.Value;

		#endregion

		#region Single

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Single Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Single Single", ExpectedResult = 1)]
		public Single  MinItemSingle(int[] source)
			=> source.Select(v => new Item<Single>(v)). MinItem(i => i.Value).Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Single Nullable Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Single Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Single Nullable Empty",  ExpectedResult = null)]
		public Single?  MinItemSingleNullable(int[] source)
			=> source.Select(v => new Item<Single?>(v)). MinItem(i => i.Value)?.Value;

		#endregion

		#region Double

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Double Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Double Single", ExpectedResult = 1)]
		public Double  MinItemDouble(int[] source)
			=> source.Select(v => new Item<Double>(v)). MinItem(i => i.Value).Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Double Nullable Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Double Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Double Nullable Empty",  ExpectedResult = null)]
		public Double?  MinItemDoubleNullable(int[] source)
			=> source.Select(v => new Item<Double?>(v)). MinItem(i => i.Value)?.Value;

		#endregion

		#region Decimal

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Decimal Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Decimal Single", ExpectedResult = 1)]
		public Decimal  MinItemDecimal(int[] source)
			=> source.Select(v => new Item<Decimal>(v)). MinItem(i => i.Value).Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MinItem Decimal Nullable Array",  ExpectedResult = 0)]
		[TestCase(new[] {1},             TestName = "MinItem Decimal Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MinItem Decimal Nullable Empty",  ExpectedResult = null)]
		public Decimal?  MinItemDecimalNullable(int[] source)
			=> source.Select(v => new Item<Decimal?>(v)). MinItem(i => i.Value)?.Value;

		#endregion

		#endregion

		#region MaxItem

		#region Int32

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Int32 Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Int32 Single", ExpectedResult = 1)]
		public Int32  MaxItemInt32(int[] source)
			=> source.Select(v => new Item<Int32>(v)). MaxItem(i => i.Value).Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Int32 Nullable Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Int32 Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Int32 Nullable Empty",  ExpectedResult = null)]
		public Int32?  MaxItemInt32Nullable(int[] source)
			=> source.Select(v => new Item<Int32?>(v)). MaxItem(i => i.Value)?.Value;

		#endregion

		#region Int64

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Int64 Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Int64 Single", ExpectedResult = 1)]
		public Int64  MaxItemInt64(int[] source)
			=> source.Select(v => new Item<Int64>(v)). MaxItem(i => i.Value).Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Int64 Nullable Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Int64 Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Int64 Nullable Empty",  ExpectedResult = null)]
		public Int64?  MaxItemInt64Nullable(int[] source)
			=> source.Select(v => new Item<Int64?>(v)). MaxItem(i => i.Value)?.Value;

		#endregion

		#region Single

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Single Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Single Single", ExpectedResult = 1)]
		public Single  MaxItemSingle(int[] source)
			=> source.Select(v => new Item<Single>(v)). MaxItem(i => i.Value).Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Single Nullable Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Single Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Single Nullable Empty",  ExpectedResult = null)]
		public Single?  MaxItemSingleNullable(int[] source)
			=> source.Select(v => new Item<Single?>(v)). MaxItem(i => i.Value)?.Value;

		#endregion

		#region Double

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Double Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Double Single", ExpectedResult = 1)]
		public Double  MaxItemDouble(int[] source)
			=> source.Select(v => new Item<Double>(v)). MaxItem(i => i.Value).Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Double Nullable Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Double Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Double Nullable Empty",  ExpectedResult = null)]
		public Double?  MaxItemDoubleNullable(int[] source)
			=> source.Select(v => new Item<Double?>(v)). MaxItem(i => i.Value)?.Value;

		#endregion

		#region Decimal

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Decimal Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Decimal Single", ExpectedResult = 1)]
		public Decimal  MaxItemDecimal(int[] source)
			=> source.Select(v => new Item<Decimal>(v)). MaxItem(i => i.Value).Value;

		[TestCase(new[] {3, 1, 0, 4, 6}, TestName = "MaxItem Decimal Nullable Array",  ExpectedResult = 6)]
		[TestCase(new[] {1},             TestName = "MaxItem Decimal Nullable Single", ExpectedResult = 1)]
		[TestCase(new int[0],            TestName = "MaxItem Decimal Nullable Empty",  ExpectedResult = null)]
		public Decimal?  MaxItemDecimalNullable(int[] source)
			=> source.Select(v => new Item<Decimal?>(v)). MaxItem(i => i.Value)?.Value;

		#endregion

		#endregion

	}
}