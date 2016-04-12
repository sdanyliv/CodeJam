using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

using BenchmarkDotNet.NUnit;

using JetBrains.Annotations;

using NUnit.Framework;

using static CodeJam.AssemblyWideConfig;

namespace CodeJam
{
	/// <summary>
	/// Prooftest: JIT optimizations on handwritten method dispatching
	/// </summary>
	[TestFixture(Category = BenchmarkConstants.BenchmarkCategory + ": Self-testing")]
	[SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
	[SuppressMessage("ReSharper", "ConvertToConstant.Local")]
	[PublicAPI]
	public class DispatchingOptimizationBenchmark
	{
		// Use case:
		// 1. We have multiple implementations for the same algorithm.
		// 2. We want to choose implementation depending on process' environment: feature switches, FW version etc.
		// 3. We want as few penalty for dispatching as it is possible;
		[Test]
		[Explicit(BenchmarkConstants.ExplicitExcludeReason)]
		public void BenchmarkJitOptimizedDispatch() =>
			CompetitionBenchmarkRunner.Run(this, RunConfig);

		#region Assertion to proof the idea works at all
		[Test]
		public void AssertJitOptimizedDispatch()
		{
			const int someNum = 1024;
			var impl2 = Implementation2(someNum);
			var impl3 = Implementation3(someNum);

			// 1. Jitting the methods. Impl2 should be used.
			Assert.AreEqual(DirectCall(someNum), impl2);
			Assert.AreEqual(SwitchOverRoField(someNum), impl2);
			Assert.AreEqual(SwitchOverStaticField(someNum), impl2);

			// 2. Update the field values:

			// 2.1. Updating the readonly field.
			// Should be ignored;
			var bf = BindingFlags.Static | BindingFlags.NonPublic;
			// ReSharper disable once PossibleNullReferenceException
			typeof(DispatchingOptimizationBenchmark)
				.GetField(nameof(_implementationToUse1), bf)
				.SetValue(null, ImplementationToUse.Implementation3);

			//2.2. Updating the field.
			// Should NOT be ignored;
			_implementationToUse2 = ImplementationToUse.Implementation3;

			// 3. Now, the assertions:
			// Nothing changed
			Assert.AreEqual(DirectCall(someNum), impl2);
			// Same as previous call (switch thrown away by JIT)
			Assert.AreEqual(SwitchOverRoField(someNum), impl2);
			// Uses implementation 3
			Assert.AreEqual(SwitchOverStaticField(someNum), impl3);
		}
		#endregion

		// Here we go:
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int Implementation1(int i) => i * i;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int Implementation2(int i) => i + 1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int Implementation3(int i) => i / 2;

		private enum ImplementationToUse
		{
			Implementation1,
			Implementation2,
			Implementation3
		}

		#region Dispatching implementations
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static int DirectCall(int i) => Implementation2(i);

		private static readonly ImplementationToUse _implementationToUse1 = ImplementationToUse.Implementation2;

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static int SwitchOverRoField(int i)
		{
			switch (_implementationToUse1)
			{
				case ImplementationToUse.Implementation1:
					return Implementation1(i);
				case ImplementationToUse.Implementation2:
					return Implementation2(i);
				case ImplementationToUse.Implementation3:
					return Implementation3(i);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static volatile ImplementationToUse _implementationToUse2 = ImplementationToUse.Implementation2;

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static int SwitchOverStaticField(int i)
		{
			switch (_implementationToUse2)
			{
				case ImplementationToUse.Implementation1:
					return Implementation1(i);
				case ImplementationToUse.Implementation2:
					return Implementation2(i);
				case ImplementationToUse.Implementation3:
					return Implementation3(i);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		#endregion

		#region Competition
		public const int Count = 10 * 1000 * 1000;

		[CompetitionBaseline]
		public int Test00Baseline()
		{
			var sum = 0;
			var count = Count;
			for (var i = 0; i < count; i++)
			{
				sum += DirectCall(i);
			}
			return sum;
		}

		[CompetitionBenchmark(0.89, 1.09)]
		public int Test01SwitchOverRoField()
		{
			var sum = 0;
			var count = Count;
			for (var i = 0; i < count; i++)
			{
				sum += SwitchOverRoField(i);
			}

			return sum;
		}

		[CompetitionBenchmark(1.18, 1.64)]
		public int Test02SwitchOverStaticField()
		{
			var sum = 0;
			var count = Count;
			for (var i = 0; i < count; i++)
			{
				sum += SwitchOverStaticField(i);
			}

			return sum;
		}
		#endregion
	}
}