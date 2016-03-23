using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>Miscellaneous algorithms</summary>
	[PublicAPI]
	public static partial class Algorithms
	{
		/// <summary>Swaps two objects</summary>
		public static void Swap<T>(ref T value1, ref T value2)
		{
			var t = value1;
			value1 = value2;
			value2 = t;
		}
	}
}