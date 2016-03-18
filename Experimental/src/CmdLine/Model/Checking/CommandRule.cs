using System;

using JetBrains.Annotations;

namespace CodeJam.CmdLine
{
	///<summary>
	/// Command rule.
	///</summary>
	[PublicAPI]
	public class CommandRule
	{
		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public CommandRule([NotNull] string name, string description = "")
		{
			if (name == null) throw new ArgumentNullException(nameof(name));
			Name = name;
			Description = description;
		}

		/// <summary>
		/// Command name.
		/// </summary>
		[NotNull]
		public string Name { get; }

		/// <summary>
		/// Command description.
		/// </summary>
		public string Description { get; }
	}
}