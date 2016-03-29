using System;

using JetBrains.Annotations;

namespace CodeJam
{
	[PublicAPI]
	public static class Fn<T>
	{
		/// <summary>
		/// Gets the function that always returns true.
		/// </summary>
		public static readonly Func<T, bool> True = o => true;

		/// <summary>
		/// Gets the function that returns false.
		/// </summary>
		public static readonly Func<T, bool> False = o => false;

		/// <summary>
		/// Gets the function that returns true.
		/// </summary>
		public static readonly Predicate<T> TruePredicate = o => true;

		/// <summary>
		/// Gets the function that always returns false.
		/// </summary>
		public static readonly Predicate<T> FalsePredicate = o => false;

		/// <summary>
		/// Gets the function that returns the same object which was passed as parameter.
		/// </summary>
		public static readonly Func<T, T> Identity = o => o;

		/// <summary>
		/// Gets the function that returns the same object which was passed as parameter.
		/// </summary>
		public static readonly Converter<T, T> IdentityConverter = o => o;

		/// <summary>
		/// Gets the function that returns true if an object is not null.
		/// </summary>
		public static readonly Func<T, bool> IsNotNull = o => o != null;
	}
}
