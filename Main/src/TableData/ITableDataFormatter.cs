﻿using JetBrains.Annotations;

namespace CodeJam.TableData
{
	/// <summary>
	/// Formatter interface.
	/// </summary>
	[PublicAPI]
	public interface ITableDataFormatter
	{
		/// <summary>
		/// Returns length of formatted value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Length of formatted value representation.</returns>
		int GetValueLength(string value);

		/// <summary>
		/// Prints line of table data.
		/// </summary>
		/// <param name="values">Line values.</param>
		/// <param name="columnWidths">Array of column widths.</param>
		/// <returns>String representatiopn of values</returns>
		[NotNull]
		string FormatLine([NotNull] string[] values, [NotNull] int[] columnWidths);
	}
}