using System;

using JetBrains.Annotations;

namespace CodeJam.Reflection
{
	/// <summary>
	/// Parameter data for CreateInstance method.
	/// </summary>
	[PublicAPI]
	public class ParamInfo
	{
		/// <summary>Initializes a new instance of the <see cref="ParamInfo" /> class.</summary>
		public ParamInfo([NotNull] string name, [CanBeNull] object value, bool required = true)
		{
			if (name == null) throw new ArgumentNullException(nameof(name));
			Name = name;
			Value = value;
			Required = required;
		}

		/// <summary>Initializes a new instance of the <see cref="ParamInfo" /> class.</summary>
		public static ParamInfo Param([NotNull] string name, [CanBeNull] object value, bool required = false) =>
			new ParamInfo(name, value, required);

		/// <summary>
		/// Parameter name.
		/// </summary>
		[NotNull]
		public string Name { get; set; }

		/// <summary>
		/// Parameter value.
		/// </summary>
		[CanBeNull]
		public object Value { get; set; }

		/// <summary>
		/// True, if parameter required.
		/// </summary>
		public bool Required { get; set; }
	}
}