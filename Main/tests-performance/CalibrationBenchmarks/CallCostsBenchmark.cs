using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using BenchmarkDotNet.NUnit;

using JetBrains.Annotations;

using NUnit.Framework;

using static CodeJam.AssemblyWideConfig;

namespace CodeJam
{
	/// <summary>
	/// Estimates average cost of calls
	/// </summary>
	[TestFixture(Category = BenchmarkConstants.BenchmarkCategory + ": Self-testing")]
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
		public void BenchmarkCallCosts() =>
			CompetitionBenchmarkRunner.Run(this, RunConfig);

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

		private class CompareCalls : ICompareCalls<int>, ICompareCalls
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

		[CompetitionBaseline]
		public int Test00Raw()
		{
			int a = 0;
			for (int i = 0; i < Count; i++)
			{
				a = a + 1;
			}

			return Count;
		}

		[CompetitionBenchmark(0.82, 1.21)]
		public int Test01Call()
		{
			int a = 0;
			for (int i = 0; i < Count; i++)
			{
				a = CompareCalls.Call(a);
			}

			return Count;
		}

		[CompetitionBenchmark(0.76, 1.15)]
		public int Test02GenericCall()
		{
			int a = 0;
			for (int i = 0; i < Count; i++)
			{
				a = CompareCalls.Call<object>(a);
			}

			return Count;
		}

		[CompetitionBenchmark(0.91, 1.18)]
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

		[CompetitionBenchmark(0.71, 1.27)]
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

		[CompetitionBenchmark(4.39, 6.80)]
		public int Test05CallNoInline()
		{
			int a = 0;
			for (int i = 0; i < Count; i++)
			{
				a = CompareCalls.CallNoInline(a);
			}

			return Count;
		}

		[CompetitionBenchmark(4.96, 6.91)]
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

		[CompetitionBenchmark(4.88, 7.27)]
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

		[CompetitionBenchmark(4.71, 7.23)]
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

		[CompetitionBenchmark(6.51, 10.07)]
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

		[CompetitionBenchmark(6.66, 10.12)]
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

		[CompetitionBenchmark(6.31, 9.65)]
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

		[CompetitionBenchmark(6.01, 10.01)]
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

		[CompetitionBenchmark(26.54, 42.01)]
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

		[CompetitionBenchmark(26.00, 44.43)]
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

		[CompetitionBenchmark(6.14, 9.70)]
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

		[CompetitionBenchmark(7.36, 11.68)]
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

		[CompetitionBenchmark(7.39, 12.04)]
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

		[CompetitionBenchmark(28.29, 44.75)]
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

		[CompetitionBenchmark(7.99, 11.92)]
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

		[CompetitionBenchmark(5.34, 8.39)]
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

		[CompetitionBenchmark(22.59, 31.95)]
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