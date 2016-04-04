using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.NUnit;

using JetBrains.Annotations;

using NUnit.Framework;

namespace CodeJam
{
	/// <summary>
	/// Estimates average cost of calls
	/// </summary>
	[TestFixture(Category = BenchmarkConstants.BenchmarkCategory)]
	[Config(typeof(FastRunConfig))]
	[SuppressMessage("ReSharper", "AccessToModifiedClosure")]
	[SuppressMessage("ReSharper", "ClassCanBeSealed.Local")]
	[SuppressMessage("ReSharper", "ConvertMethodToExpressionBody")]
	[SuppressMessage("ReSharper", "ConvertToConstant.Local")]
	[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local")]
	[SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
	[SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
	[SuppressMessage("ReSharper", "UnusedTypeParameter")]
	[PublicAPI]
	public class CallCostsBenchmark
	{
		[Test]
		// WAITINGFOR: https://github.com/PerfDotNet/BenchmarkDotNet/issues/126.
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkCallCosts()
		{
			CompetitionBenchmarkRunner.Run(this, 0.9, 50.0);
		}

		#region CompetitionMethods
		private interface ICompareCalls
		{
			int CallInterface(int a);
			int CallInterface<T>(int a);
		}

		private interface ICompareCalls<T>
		{
			T CallInterface(T a);
		}

		private class CompareCalls: ICompareCalls<int>, ICompareCalls
		{
			public static int Call(int a)
			{
				return a + 1;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			public static int CallNoInline(int a)
			{
				return a + 1;
			}

			public static int Call<T>(int a)
			{
				return a + 1;
			}

			public int CallInst(int a)
			{
				return a + 1;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			public int CallInstNoInline(int a)
			{
				return a + 1;
			}

			public int CallInst<T>(int a)
			{
				return a + 1;
			}

			public virtual int CallVirtual(int a)
			{
				return a + 1;
			}

			public virtual int CallInterface(int a)
			{
				return a + 1;
			}

			public virtual int CallInterface<T>(int a)
			{
				return a + 1;
			}
		}

		private class CompareCallsDerived : CompareCalls
		{
			public override int CallVirtual(int a)
			{
				return a + 1;
			}

			public override int CallInterface(int a)
			{
				return a + 1;
			}

			public override int CallInterface<T>(int a)
			{
				return a + 1;
			}
		}
		#endregion

		private const int Count = 100 * 1000;

		[Benchmark(Baseline = true)]
		public int Test00Raw()
		{
			int a = 0;
			for (int i = 0; i < Count; i++)
			{
				a = a + 1;
			}

			return Count;
		}

		[CompetitionBenchmark(0.9, 1.15)]
		public int Test01Call()
		{
			int a = 0;
			for (int i = 0; i < Count; i++)
			{
				a = CompareCalls.Call(a);
			}

			return Count;
		}

		[CompetitionBenchmark(0.9, 1.15)]
		public int Test02GenericCall()
		{
			int a = 0;
			for (int i = 0; i < Count; i++)
			{
				a = CompareCalls.Call<object>(a);
			}

			return Count;
		}

		[CompetitionBenchmark(0.9, 1.15)]
		public int Test03InstanceCall()
		{
			int a = 0;
			CompareCalls p = new CompareCalls();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInst(a);
			}

			return Count;
		}

		[CompetitionBenchmark(0.9, 1.15)]
		public int Test04InstanceGenericCall()
		{
			int a = 0;
			CompareCalls p = new CompareCalls();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInst<object>(a);
			}

			return Count;
		}

		[CompetitionBenchmark(5.5, 7.5)]
		public int Test05CallNoInline()
		{
			int a = 0;
			for (int i = 0; i < Count; i++)
			{
				a = CompareCalls.CallNoInline(a);
			}

			return Count;
		}

		[CompetitionBenchmark(5.5, 7.5)]
		public int Test06InstanceCallNoInline()
		{
			int a = 0;
			CompareCalls p = new CompareCalls();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInstNoInline(a);
			}

			return Count;
		}

		[CompetitionBenchmark(6.0, 8.0)]
		public int Test07InstanceVirtualCall()
		{
			int a = 0;
			CompareCalls p = new CompareCalls();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallVirtual(a);
			}

			return Count;
		}

		[CompetitionBenchmark(6.0, 8.0)]
		public int Test08DerivedVirtualCall()
		{
			int a = 0;
			CompareCallsDerived p = new CompareCallsDerived();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallVirtual(a);
			}

			return Count;
		}

		[CompetitionBenchmark(8.0, 10.0)]
		public int Test09InterfaceCall()
		{
			int a = 0;
			ICompareCalls p = new CompareCalls();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInterface(a);
			}

			return Count;
		}

		[CompetitionBenchmark(8.0, 10.0)]
		public int Test10DerivedInterfaceCall()
		{
			int a = 0;
			ICompareCalls p = new CompareCallsDerived();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInterface(a);
			}

			return Count;
		}

		[CompetitionBenchmark(8.0, 10.0)]
		public int Test11GenericInterfaceCall()
		{
			int a = 0;
			ICompareCalls<int> p = new CompareCalls();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInterface(a);
			}

			return Count;
		}

		[CompetitionBenchmark(8.0, 10.0)]
		public int Test12DerivedGenericInterfaceCall()
		{
			int a = 0;
			ICompareCalls<int> p = new CompareCallsDerived();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInterface(a);
			}

			return Count;
		}

		[CompetitionBenchmark(30.0, 47.5)]
		public int Test13InterfaceGenericCall()
		{
			int a = 0;
			ICompareCalls p = new CompareCalls();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInterface<object>(a);
			}

			return Count;
		}

		[CompetitionBenchmark(30.0, 47.5)]
		public int Test14DerivedInterfaceGenericCall()
		{
			int a = 0;
			ICompareCalls p = new CompareCallsDerived();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInterface<object>(a);
			}

			return Count;
		}

		[CompetitionBenchmark(8.0, 10.0)]
		public int Test15LambdaCached()
		{
			int a1 = 0;
			Func<int, int> x = a => a + 1;
			for (int i = 0; i < Count; i++)
			{
				a1 = x(a1);
			}

			return Count;
		}

		[CompetitionBenchmark(9.0, 12.5)]
		public int Test16LambdaNew()
		{
			int a1 = 0;
			for (int i = 0; i < Count; i++)
			{
				Func<int, int> x = a => a + 1;
				a1 = x(a1);
			}

			return Count;
		}

		[CompetitionBenchmark(9.0, 12.5)]
		public int Test17LambdaClosure()
		{
			int a1 = 0;
			int t;
			for (int i = 0; i < Count; i++)
			{
				t = 1;
				Func<int, int> x = a => a + t;
				a1 = x(a1);
			}

			return Count;
		}

		[CompetitionBenchmark(34.5, 49.5)]
		public int Test18LambdaClosureLocal()
		{
			int a1 = 0;
			for (int i = 0; i < Count; i++)
			{
				int t = 1;
				Func<int, int> x = a => a + t;
				a1 = x(a1);
			}

			return Count;
		}

		[CompetitionBenchmark(9.0, 12.5)]
		public int Test19FuncCached()
		{
			int a = 0;
			Func<int, int> x = CompareCalls.Call;
			for (int i = 0; i < Count; i++)
			{
				a = x(a);
			}

			return Count;
		}

		[CompetitionBenchmark(7.0, 9.5)]
		public int Test20FuncCachedInstance()
		{
			int a = 0;
			Func<int, int> x = new CompareCalls().CallInst;
			for (int i = 0; i < Count; i++)
			{
				a = x(a);
			}

			return Count;
		}

		[CompetitionBenchmark(26.0, 33.5)]
		public int Test21FuncNew()
		{
			int a = 0;
			for (int i = 0; i < Count; i++)
			{
				Func<int, int> x = CompareCalls.Call;
				a = x(a);
			}

			return Count;
		}
	}
}