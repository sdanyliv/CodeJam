using System;
using System.Configuration;
using System.Reflection;

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.NUnit;

namespace CodeJam
{
	/// <summary>
	/// Use this to run fast but inaccurate measures
	/// OPTIONAL: Updates source files with actual min..max ratio for [CompetitionBenchmark]
	/// </summary>
	internal class AssemblyWideConfig : ManualConfig
	{
		/// <summary>
		/// OPTIONAL: Set AssemblyWideConfig.AnnotateOnRun=true in app.config Set this to true
		/// to enable auto-annotation of benchmark methods
		/// </summary>
		public static readonly bool AnnotateOnRun = TryGetSwitch(
			nameof(AssemblyWideConfig),
			// ReSharper disable once StaticMemberInitializerReferesToMemberBelow
			nameof(AnnotateOnRun));

		private static bool TryGetSwitch(string scope, string switchName)
		{
			var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

			var name = scope + "." + switchName;
			var value = config.AppSettings.Settings[name]?.Value;
			bool result;
			bool.TryParse(value, out result);
			return result;
		}

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