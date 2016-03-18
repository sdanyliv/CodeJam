using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// Helper methods for <see cref="IDisposable"/>
	/// </summary>
	[PublicAPI]
	public static class Disposable
	{
		/// <summary>
		/// <see cref="IDisposable"/> instance without any code in <see cref="IDisposable.Dispose"/>.
		/// </summary>
		public static readonly EmptyDisposable Empty;

		/// <summary>
		/// Creates <see cref="IDisposable"/> instanse that calls <paramref name="disposeAction"/> on disposing.
		/// </summary>
		public static AnonymousDisposable Create(Action disposeAction) => new AnonymousDisposable(disposeAction);

		/// <summary>
		/// Combine multiple <see cref="IDisposable"/> instances into single one.
		/// </summary>
		public static IDisposable Merge(this IEnumerable<IDisposable> disposables) =>
			Create(
				() =>
				{
					foreach (var disposable in disposables)
						disposable.Dispose();
				});

		/// <summary>
		/// Combine multiple <see cref="IDisposable"/> instances into single one.
		/// </summary>
		public static IDisposable Merge(params IDisposable[] disposables) => Merge((IEnumerable<IDisposable>)disposables);

		public struct EmptyDisposable : IDisposable
		{
			public void Dispose()
			{}
		}

		public struct AnonymousDisposable : IDisposable
		{
			private readonly Action _disposeAction;

			public AnonymousDisposable(Action disposeAction)
			{
				_disposeAction = disposeAction;
			}

			public void Dispose() => _disposeAction();
		}
	}
}