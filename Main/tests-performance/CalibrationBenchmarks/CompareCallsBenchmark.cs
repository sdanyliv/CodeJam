using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using BenchmarkDotNet.Attributes;

using NUnit.Framework;

using BenchmarkDotNet.NUnit;

namespace CodeJam.Tests.Performance
{
	[Config(typeof(FastRunConfig))]
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	[SuppressMessage("ReSharper", "UnusedTypeParameter")]
	[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local")]
	[SuppressMessage("ReSharper", "ConvertToConstant.Local")]
	[SuppressMessage("ReSharper", "ClassCanBeSealed.Local")]
	[SuppressMessage("ReSharper", "AccessToModifiedClosure")]
	[SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
	public class CompareCallsBenchmark : CompareCallsBenchmark.ICompareCalls, CompareCallsBenchmark.ICompareCalls<int>
	{
		[Test]
		[Explicit(
			"Autorun disabled as it takes too long to run. If needed, select the test in the Test Explorer and run it manually")]
		// WAITINGFOR: https://github.com/PerfDotNet/BenchmarkDotNet/issues/126 . Remove [Explicit] as fixed.
		public static void CompareCalls()
		{
			CompetitionBenchmarkRunner.Run<CompareCallsBenchmark>(0.9, 50.0);
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

		private class CompareCallsDerived : CompareCallsBenchmark
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

		private static int Call(int a)
		{
			return a + 1;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static int CallNoInline(int a)
		{
			return a + 1;
		}

		private static int Call<T>(int a)
		{
			return a + 1;
		}

		private int CallInst(int a)
		{
			return a + 1;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private int CallInstNoInline(int a)
		{
			return a + 1;
		}

		private int CallInst<T>(int a)
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
		#endregion

		private const int Count = 1000 * 1000;

		[Benchmark(Baseline = true)]
		public int M_00_Raw()
		{
			int a = 0;
			for (int i = 0; i < Count; i++)
			{
				a = a + 1;
			}

			return Count;
		}

		[Benchmark]
		public int M_01_Call()
		{
			int a = 0;
			for (int i = 0; i < Count; i++)
			{
				a = Call(a);
			}

			return Count;
		}

		[Benchmark]
		public int M_02_GenericCall()
		{
			int a = 0;
			for (int i = 0; i < Count; i++)
			{
				a = Call<object>(a);
			}

			return Count;
		}

		[Benchmark]
		public int M_03_InstanceCall()
		{
			int a = 0;
			CompareCallsBenchmark p = new CompareCallsBenchmark();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInst(a);
			}

			return Count;
		}

		[Benchmark]
		public int M_04_InstanceGenericCall()
		{
			int a = 0;
			CompareCallsBenchmark p = new CompareCallsBenchmark();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInst<object>(a);
			}

			return Count;
		}

		[Benchmark]
		public int M_05_CallNoInline()
		{
			int a = 0;
			for (int i = 0; i < Count; i++)
			{
				a = CallNoInline(a);
			}

			return Count;
		}

		[Benchmark]
		public int M_06_InstanceCallNoInline()
		{
			int a = 0;
			CompareCallsBenchmark p = new CompareCallsBenchmark();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInstNoInline(a);
			}

			return Count;
		}

		[Benchmark]
		public int M_07_InstanceVirtualCall()
		{
			int a = 0;
			CompareCallsBenchmark p = new CompareCallsBenchmark();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallVirtual(a);
			}

			return Count;
		}

		[Benchmark]
		public int M_08_DerivedVirtualCall()
		{
			int a = 0;
			CompareCallsDerived p = new CompareCallsDerived();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallVirtual(a);
			}

			return Count;
		}

		[Benchmark]
		public int M_09_InterfaceCall()
		{
			int a = 0;
			ICompareCalls p = new CompareCallsBenchmark();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInterface(a);
			}

			return Count;
		}

		[Benchmark]
		public int M_10_DerivedInterfaceCall()
		{
			int a = 0;
			ICompareCalls p = new CompareCallsDerived();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInterface(a);
			}

			return Count;
		}

		[Benchmark]
		public int M_11_GenericInterfaceCall()
		{
			int a = 0;
			ICompareCalls<int> p = new CompareCallsBenchmark();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInterface(a);
			}

			return Count;
		}

		[Benchmark]
		public int M_12_DerivedGenericInterfaceCall()
		{
			int a = 0;
			ICompareCalls<int> p = new CompareCallsDerived();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInterface(a);
			}

			return Count;
		}

		[Benchmark]
		public int M_13_InterfaceGenericCall()
		{
			int a = 0;
			ICompareCalls p = new CompareCallsBenchmark();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInterface<object>(a);
			}

			return Count;
		}

		[Benchmark]
		public int M_14_DerivedInterfaceGenericCall()
		{
			int a = 0;
			ICompareCalls p = new CompareCallsDerived();
			for (int i = 0; i < Count; i++)
			{
				a = p.CallInterface<object>(a);
			}

			return Count;
		}

		[Benchmark]
		public int M_15_LambdaCached()
		{
			int a1 = 0;
			Func<int, int> x = a => a + 1;
			for (int i = 0; i < Count; i++)
			{
				a1 = x(a1);
			}

			return Count;
		}

		[Benchmark]
		public int M_16_LambdaNew()
		{
			int a1 = 0;
			for (int i = 0; i < Count; i++)
			{
				Func<int, int> x = a => a + 1;
				a1 = x(a1);
			}

			return Count;
		}

		[Benchmark]
		public int M_17_LambdaClosure()
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

		[Benchmark]
		public int M_18_LambdaClosureLocal()
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

		[Benchmark]
		public int M_19_FuncCached()
		{
			int a = 0;
			Func<int, int> x = Call;
			for (int i = 0; i < Count; i++)
			{
				a = x(a);
			}

			return Count;
		}

		[Benchmark]
		public int M_20_FuncCachedInstance()
		{
			int a = 0;
			Func<int, int> x = new CompareCallsBenchmark().CallInst;
			for (int i = 0; i < Count; i++)
			{
				a = x(a);
			}

			return Count;
		}

		[Benchmark]
		public int M_21_FuncNew()
		{
			int a = 0;
			for (int i = 0; i < Count; i++)
			{
				Func<int, int> x = Call;
				a = x(a);
			}

			return Count;
		}
	}
}