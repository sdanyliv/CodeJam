using System;

using JetBrains.Annotations;

// ReSharper disable CheckNamespace
// ReSharper disable once RedundantAttributeUsageProperty

namespace BenchmarkDotNet.NUnit
{
	/// <summary>
	/// Specifies xml resource to be used as benchmark limit data.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	[PublicAPI, MeansImplicitUse]
	public class CompetitionMetadataAttribute : Attribute
	{
		public CompetitionMetadataAttribute(string metadataResourceName)
		{
			MetadataResourceName = metadataResourceName;
		}

		public string MetadataResourceName { get; }
	}
}