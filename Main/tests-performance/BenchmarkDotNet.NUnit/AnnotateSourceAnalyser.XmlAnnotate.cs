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
			FinalRunAnalyser.TargetMinMax targetMinMax)
		{
			var competitionName = targetMinMax.CompetitionName;
			var candidateName = targetMinMax.CandidateName;

			var xdoc = annotateContext.GetXmlAnnotation(xmlFileName);
			var competition = GetOrCreateElement(xdoc.Root, FinalRunAnalyser.CompetitionNode, competitionName);
			var candidate = GetOrCreateElement(competition, FinalRunAnalyser.CandidateNode, candidateName);

			UpdateAttribute(candidate, FinalRunAnalyser.MinRatioAttribute, targetMinMax.MinText);
			UpdateAttribute(candidate, FinalRunAnalyser.MaxRatioAttribute, targetMinMax.MaxText);

			annotateContext.MarkAsChanged(xmlFileName);

			return true;
		}

		private XElement GetOrCreateElement(XElement element, XName name, string targetName)
		{
			var result = element
				.Elements(name)
				.SingleOrDefault(e => e.Attribute(FinalRunAnalyser.TargetAttribute)?.Value == targetName);

			if (result == null)
			{
				result = new XElement(name);
				UpdateAttribute(result, FinalRunAnalyser.TargetAttribute, targetName);
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