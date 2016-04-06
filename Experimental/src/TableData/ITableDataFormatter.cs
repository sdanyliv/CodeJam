using JetBrains.Annotations;

namespace CodeJam.TableData
{
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
		/// <param name="columnWidths">Array of column widths. If null - value is ignored.</param>
		/// <returns>String representatiopn of values</returns>
		[NotNull]
		string FormatLine([NotNull] string[] values, [CanBeNull] int[] columnWidths = null);
	}
}