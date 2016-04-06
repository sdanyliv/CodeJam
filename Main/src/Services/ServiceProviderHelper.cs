using System;

using JetBrains.Annotations;

namespace CodeJam.Services
{
	/// <summary>
	/// <see cref="IServiceProvider"/> helper methods.
	/// </summary>
	[PublicAPI]
	public static class ServiceProviderHelper
	{
		/// <summary>
		/// Gets the service object of the specified type, or throws an exception if service not registered.
		/// </summary>
		/// <param name="provider">Instance of <see cref="IServiceProvider"/>.</param>
		/// <param name="serviceType">An object that specifies the type of service object to get.</param>
		/// <returns>
		/// A service object of type <paramref name="serviceType"/>.
		/// </returns>
		[NotNull]
		public static object GetRequiredService(
			[NotNull] this IServiceProvider provider,
			[NotNull] Type serviceType)
		{
			Code.NotNull(provider, nameof(provider));
			Code.NotNull(serviceType, nameof(serviceType));

			var svc = provider.GetService(serviceType);
			if (svc == null)
				throw new ArgumentException($"Service '{serviceType}' not registered in provider.");
			return svc;
		}

		/// <summary>
		/// Gets the service object of the specified type.
		/// </summary>
		/// <typeparam name="T">An object that specifies the type of service object to get.</typeparam>
		/// <param name="provider">Instance of <see cref="IServiceProvider"/>.</param>
		/// <returns>A service object of type serviceType.</returns>
		[CanBeNull]
		public static T GetService<T> ([NotNull] this IServiceProvider provider)
		{
			Code.NotNull(provider, nameof(provider));
			return (T)provider.GetService(typeof(T));
		}

		/// <summary>
		/// Gets the service object of the specified type, or throws an exception if service not registered.
		/// </summary>
		/// <param name="provider">Instance of <see cref="IServiceProvider"/>.</param>
		/// <typeparam name="T">An object that specifies the type of service object to get.</typeparam>
		/// <returns>
		/// A service object of type <typeparamref name="T"/>.
		/// </returns>
		[NotNull]
		public static object GetRequiredService<T>([NotNull] this IServiceProvider provider)
		{
			Code.NotNull(provider, nameof(provider));

			var svc = provider.GetService(typeof(T));
			if (svc == null)
				throw new ArgumentException($"Service '{typeof(T)}' not registered in provider.");
			return svc;
		}
	}
}