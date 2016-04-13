using System;
using System.Linq;
using System.Xml.Linq;

// ReSharper disable CheckNamespace

namespace BenchmarkDotNet.NUnit
{
	public partial class AnnotateSourceAnalyser
	{
		private bool TryFixBenchmarkResource(
			AnnotateContext annotateContext, string xmlFileName,
			CompetitionTarget competitionTarget)
		{
			var competitionName = competitionTarget.CompetitionName;
			var candidateName = competitionTarget.CandidateName;

			var xdoc = annotateContext.GetXmlAnnotation(xmlFileName);
			var competition = GetOrCreateElement(xdoc.Root, CompetitionTargetHelpers.CompetitionNode, competitionName);
			var candidate = GetOrCreateElement(competition, CompetitionTargetHelpers.CandidateNode, candidateName);

			UpdateAttribute(candidate, CompetitionTargetHelpers.MinRatioAttribute, competitionTarget.MinText);
			UpdateAttribute(candidate, CompetitionTargetHelpers.MaxRatioAttribute, competitionTarget.MaxText);

			annotateContext.MarkAsChanged(xmlFileName);

			return true;
		}

		private XElement GetOrCreateElement(XElement element, XName name, string targetName)
		{
			var result = element
				.Elements(name)
				.SingleOrDefault(e => e.Attribute(CompetitionTargetHelpers.TargetAttribute)?.Value == targetName);

			if (result == null)
			{
				result = new XElement(name);
				UpdateAttribute(result, CompetitionTargetHelpers.TargetAttribute, targetName);
				element.Add(result);
			}

			return result;
		}

		private XAttribute UpdateAttribute(XElement element, XName attributeName, string attributeValue)
		{
			var result = element.Attribute(attributeName);

			if (result == null)
			{
				result = new XAttribute(attributeName, attributeValue);
				element.Add(result);
			}
			else
			{
				result.Value = attributeValue;
			}

			return result;
		}
	}
}