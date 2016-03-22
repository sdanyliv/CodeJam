using System;
using System.Xml;
using System.Xml.Linq;

using CodeJam.Collections;

using JetBrains.Annotations;

namespace CodeJam.Xml
{
	/// <summary>
	/// Extensions for XLinq.
	/// </summary>
	[PublicAPI]
	public static class XNodeExtensions
	{
		/// <summary>
		/// Returns <paramref name="document"/> root, or throw an exception, if root is null.
		/// </summary>
		[NotNull]
		[Pure]
		public static XElement RequiredRoot([NotNull] this XDocument document)
		{
			if (document == null) throw new ArgumentNullException(nameof(document));
			if (document.Root == null)
				throw new XmlException("Document root is required");
			return document.Root;
		}

		/// <summary>
		/// Returns <paramref name="document"/> root, or throws an exception, if root is null or has another name.
		/// </summary>
		[NotNull]
		[Pure]
		public static XElement RequiredRoot([NotNull] this XDocument document, [NotNull] XName rootName)
		{
			if (rootName == null) throw new ArgumentNullException(nameof(rootName));
			var root = document.RequiredRoot();
			if (root.Name != rootName)
				throw new XmlException($"Document root '{rootName}' not found, '{root.Name}' found instead.");
			return root;
		}

		/// <summary>
		/// Returns child element with name <paramref name="name"/>, or throws an exception if element does not exists.
		/// </summary>
		[NotNull]
		[Pure]
		public static XElement RequiredElement([NotNull] this XElement parent, [NotNull] XName name)
		{
			if (parent == null) throw new ArgumentNullException(nameof(parent));
			if (name == null) throw new ArgumentNullException(nameof(name));
			var element = parent.Element(name);
			if (element == null)
				throw new XmlException($"Element with name '{name}' does not exists.");
			return element;
		}

		/// <summary>
		/// Returns child element with one of names in <paramref name="names"/>,
		/// or throws an exception if element does not exists.
		/// </summary>
		[NotNull]
		[Pure]
		public static XElement RequiredElement([NotNull] this XElement parent, params XName[] names)
		{
			if (parent == null) throw new ArgumentNullException(nameof(parent));

			var namesSet = names.ToHashSet();
			foreach (var element in parent.Elements())
				if (namesSet.Contains(element.Name))
					return element;
			throw new XmlException($"Element with names {names.Join(", ")} not exists.");
		}

		/// <summary>
		/// Returns attribute with name <paramref name="name"/>, or thows an exception if attribute does not exists.
		/// </summary>
		[NotNull]
		[Pure]
		public static XAttribute RequiredAttribute([NotNull] this XElement element, [NotNull] XName name)
		{
			if (element == null) throw new ArgumentNullException(nameof(element));
			if (name == null) throw new ArgumentNullException(nameof(name));
			var attr = element.Attribute(name);
			if (attr == null)
				throw new XmlException($"Element contains no attribute '{name}'");
			return attr;
		}
	}
}