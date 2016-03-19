using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// The <see cref="IDisposable"/> extensions.
	/// </summary>
	[PublicAPI]
	public static class DisposableExtensions
	{
		/// <summary>
		/// Invokes the dispose for each item in the specified collection.
		/// </summary>
		/// <param name="collection">The multiple <see cref="IDisposable"/> instances.</param>
		public static void DisposeAll([NotNull, InstantHandle] this IEnumerable<IDisposable> collection)
		{
			List<Exception> exceptions = null;

			foreach (var item in collection)
			{
				try
				{
					item.Dispose();
				}
				catch (Exception ex)
				{
					if (exceptions == null)
						exceptions = new List<Exception>();

					exceptions.Add(ex);
				}
			}

			if (exceptions != null)
				throw new AggregateException(exceptions);
		}

		/// <summary>
		/// Invokes the dispose for each item in the specified collection.
		/// </summary>
		/// <param name="collection">The multiple <see cref="IDisposable"/> instances.</param>
		/// <param name="exceptionHandler">The exception handler.</param>
		public static void DisposeAll(
			[NotNull, InstantHandle] this IEnumerable<IDisposable> collection,
			[NotNull] Func<Exception, bool> exceptionHandler)
		{
			foreach (var item in collection)
			{
				try
				{
					item.Dispose();
				}
				catch (Exception ex) when (exceptionHandler(ex))
				{
				}
			}
		}
	}
}
