using System;

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.NUnit;

// ReSharper disable CheckNamespace

namespace CodeJam
{
	/// <summary>
	/// Use this to run fast but inaccurate measures
	/// OPTIONAL: Updates source files with actual min..max ratio for [CompetitionBenchmark]
	/// </summary>
	internal class AssemblyWideConfig : ManualConfig
	{
		/// <summary>
		/// OPTIONAL: Set this to true to enable auto-annotation of benchmark methods
		/// </summary>
		public static readonly bool AnnotateOnRun = false; // = true;

		/// <summary>
		/// Instance of the config
		/// </summary>
		public static IConfig RunConfig => new AssemblyWideConfig(true);

		/// <summary> 
		/// Constructor
		/// </summary>
		public AssemblyWideConfig() : this(false) { }

		/// <summary> 
		/// Constructor
		/// </summary>
		public AssemblyWideConfig(bool asRunConfig)
		{
			if (asRunConfig)
				Add(DefaultConfig.Instance);

			Add(FastRunConfig.Instance);

			if (AnnotateOnRun)
				Add(
					new AnnotateSourceAnalyser
					{
						RerunIfModified = true
					});
		}
	}
}