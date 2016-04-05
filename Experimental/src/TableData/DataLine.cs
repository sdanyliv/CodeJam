using System;

using JetBrains.Annotations;

namespace CodeJam.TableData
{
	[PublicAPI]
	public struct DataLine
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
		public DataLine(int lineNum, string[] values)
		{
			LineNum = lineNum;
			Values = values;
		}

		public int LineNum { get; }

		public string[] Values { get; }

		#region Overrides of ValueType
		/// <summary>Returns the fully qualified type name of this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> containing a fully qualified type name.</returns>
		public override string ToString() => $"({LineNum}) {Values.Join(", ")}";
		#endregion
	}
}