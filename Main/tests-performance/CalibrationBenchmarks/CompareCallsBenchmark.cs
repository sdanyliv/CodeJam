using System;
using System.Runtime.CompilerServices;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.NUnit;

using NUnit.Framework;

namespace CodeJam.Tests.Performance
{
	[Config(typeof (FastRunConfig))]
	public class CompareCallsBenchmark : CompareCallsBenchmark.ICompareCalls, CompareCallsBenchmark.ICompareCalls<int>
	{
		[Test]
		[Explicit(
			"Autorun disabled as it takes too long to run. If needed, select the test in the Test Explorer and run it manually")]
		// WAITINGFOR: https://github.com/PerfDotNet/BenchmarkDotNet/issues/126 . Remove [Explicit] as fixed.
		public static void CompareCalls() => CompetitionBenchmarkRunner.Run<CompareCallsBenchmark>(0.9, 50.0);

		#region CompetitionMethods
		// ReSharper disable UnusedTypeParameter
		// ReSharper disable MemberCanBeMadeStatic.Local
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
			protected override int CallVirtual(int a) => a + 1;

			public override int CallInterface(int a) => a + 1;

			public override int CallInterface<T>(int a) => a + 1;
		}

		private static int Call(int a) => a + 1;

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static int CallNoInline(int a) => a + 1;

		
		private static int Call<T>(int a) => a + 1;

		
		private int CallInst(int a) => a + 1;

		[MethodImpl(MethodImplOptions.NoInlining)]
		private int CallInstNoInline(int a) => a + 1;

		private int CallInst<T>(int a) => a + 1;

		protected virtual int CallVirtual(int a) => a + 1;

		public virtual int CallInterface(int a) => a + 1;

		public virtual int CallInterface<T>(int a) => a + 1;
		// ReSharper restore UnusedTypeParameter
		// ReSharper restore MemberCanBeMadeStatic.Local
		#endregion

		private const int Count = 1000*1000;

		[Benchmark(Baseline = true)]
		// ReSharper disable UnusedMember.Global
		public int M_00_Raw()
		{
			var a = 0;
			for (var i = 0; i < Count; i++) a = a + 1;
			return Count;
		}

		[Benchmark]
		public int M_01_Call()
		{
			var a = 0;
			for (var i = 0; i < Count; i++) a = Call(a);
			return Count;
		}

		[Benchmark]
		public int M_02_GenericCall()
		{
			var a = 0;
			for (var i = 0; i < Count; i++) a = Call<object>(a);
			return Count;
		}

		[Benchmark]
		public int M_03_InstanceCall()
		{
			var a = 0;
			var p = new CompareCallsBenchmark();
			for (var i = 0; i < Count; i++) a = p.CallInst(a);
			return Count;
		}

		[Benchmark]
		public int M_04_InstanceGenericCall()
		{
			var a = 0;
			var p = new CompareCallsBenchmark();
			for (var i = 0; i < Count; i++) a = p.CallInst<object>(a);
			return Count;
		}

		[Benchmark]
		public int M_05_CallNoInline()
		{
			var a = 0;
			for (var i = 0; i < Count; i++) a = CallNoInline(a);
			return Count;
		}

		[Benchmark]
		public int M_06_InstanceCallNoInline()
		{
			var a = 0;
			var p = new CompareCallsBenchmark();
			for (var i = 0; i < Count; i++) a = p.CallInstNoInline(a);
			return Count;
		}

		[Benchmark]
		public int M_07_InstanceVirtualCall()
		{
			var a = 0;
			var p = new CompareCallsBenchmark();
			for (var i = 0; i < Count; i++) a = p.CallVirtual(a);
			return Count;
		}

		[Benchmark]
		public int M_08_DerivedVirtualCall()
		{
			var a = 0;
			var p = new CompareCallsDerived();
			for (var i = 0; i < Count; i++) a = p.CallVirtual(a);
			return Count;
		}

		[Benchmark]
		public int M_09_InterfaceCall()
		{
			var a = 0;
			ICompareCalls p = new CompareCallsBenchmark();
			for (var i = 0; i < Count; i++) a = p.CallInterface(a);
			return Count;
		}

		[Benchmark]
		public int M_10_DerivedInterfaceCall()
		{
			var a = 0;
			ICompareCalls p = new CompareCallsDerived();
			for (var i = 0; i < Count; i++) a = p.CallInterface(a);
			return Count;
		}

		[Benchmark]
		public int M_11_GenericInterfaceCall()
		{
			var a = 0;
			ICompareCalls<int> p = new CompareCallsBenchmark();
			for (var i = 0; i < Count; i++) a = p.CallInterface(a);
			return Count;
		}

		[Benchmark]
		public int M_12_DerivedGenericInterfaceCall()
		{
			var a = 0;
			ICompareCalls<int> p = new CompareCallsDerived();
			for (var i = 0; i < Count; i++) a = p.CallInterface(a);
			return Count;
		}

		[Benchmark]
		public int M_13_InterfaceGenericCall()
		{
			var a = 0;
			ICompareCalls p = new CompareCallsBenchmark();
			for (var i = 0; i < Count; i++) a = p.CallInterface<object>(a);
			return Count;
		}

		[Benchmark]
		public int M_14_DerivedInterfaceGenericCall()
		{
			var a = 0;
			ICompareCalls p = new CompareCallsDerived();
			for (var i = 0; i < Count; i++) a = p.CallInterface<object>(a);
			return Count;
		}

		[Benchmark]
		public int M_15_LambdaCached()
		{
			var a1 = 0;
			Func<int, int> x = a => a + 1;
			for (var i = 0; i < Count; i++) a1 = x(a1);
			return Count;
		}

		[Benchmark]
		public int M_16_LambdaNew()
		{
			var a1 = 0;
			for (var i = 0; i < Count; i++)
			{
				Func<int, int> x = a => a + 1;
				a1 = x(a1);
			}
			return Count;
		}

		[Benchmark]
		public int M_17_LambdaClosure()
		{
			var a1 = 0;
			int t;
			for (var i = 0; i < Count; i++)
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
			var a1 = 0;
			for (var i = 0; i < Count; i++)
			{
				var t = 1;
				Func<int, int> x = a => a + t;
				a1 = x(a1);
			}
			return Count;
		}

		[Benchmark]
		public int M_19_FuncCached()
		{
			var a = 0;
			Func<int, int> x = Call;
			for (var i = 0; i < Count; i++) a = x(a);
			return Count;
		}

		[Benchmark]
		public int M_20_FuncCachedInstance()
		{
			var a = 0;
			Func<int, int> x = new CompareCallsBenchmark().CallInst;
			for (var i = 0; i < Count; i++) a = x(a);
			return Count;
		}

		[Benchmark]
		public int M_21_FuncNew()
		{
			var a = 0;
			for (var i = 0; i < Count; i++)
			{
				Func<int, int> x = Call;
				a = x(a);
			}
			return Count;
		}
		// ReSharper restore UnusedMember.Global
	}
}
