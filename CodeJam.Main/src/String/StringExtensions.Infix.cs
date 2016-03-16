namespace CodeJam
{
	public static partial class StringExtensions
	{
		/// <summary>
		/// Infix form of <see cref="string.IsNullOrEmpty"/>.
		/// </summary>
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		/// <summary>
		/// Returns true if argument is not null nor empty.
		/// </summary>
		public static bool NotNullNorEmpty(this string str)
		{
			return !string.IsNullOrEmpty(str);
		}

		/// <summary>
		/// Infix form of <see cref="string.IsNullOrWhiteSpace"/>.
		/// </summary>
		public static bool IsNullOrWhiteSpace(this string str)
		{
			return string.IsNullOrWhiteSpace(str);
		}

		/// <summary>
		/// Returns true if argument is not null nor whitespace.
		/// </summary>
		public static bool NotNullNorWhiteSpace(this string str)
		{
			return !string.IsNullOrWhiteSpace(str);
		}
	}
}