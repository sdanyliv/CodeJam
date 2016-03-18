using JetBrains.Annotations;

namespace CodeJam
{
	[PublicAPI]
	public static class Array<T>
	{
		/// <summary>
		/// Empty instance of <typeparamref name="T"/>[].
		/// </summary>
		[NotNull]
		public static readonly T[] Empty = new T[0];
	}
}