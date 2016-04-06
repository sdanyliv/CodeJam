using System;
using System.Collections.Concurrent;

using JetBrains.Annotations;

namespace CodeJam.Services
{
	/// <summary>
	/// Service container.
	/// </summary>
	[PublicAPI]
	public class ServiceContainer : IServicePublisher
	{
		private static readonly ConcurrentDictionary<Type, Func<IServicePublisher, object>> _services =
			new ConcurrentDictionary<Type, Func<IServicePublisher, object>>();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
		public ServiceContainer(bool publishSelf = true)
		{
			if (publishSelf)
				this.Publish<IServicePublisher>(this);
		}

		private void ConcealService(Type serviceType)
		{
			Func<IServicePublisher, object> func;
			if (!_services.TryRemove(serviceType, out func))
				throw new ArgumentException($"Service with type '{serviceType}' not registered.");
		}

		#region Implementation of IServiceProvider
		/// <summary>Gets the service object of the specified type.</summary>
		/// <returns>A service object of type <paramref name="serviceType" />.-or- null if there is no service object of type <paramref name="serviceType" />.</returns>
		/// <param name="serviceType">An object that specifies the type of service object to get. </param>
		public object GetService(Type serviceType)
		{
			Func<IServicePublisher, object> func;
			if (_services.TryGetValue(serviceType, out func))
				return func(this);
			return null;
		}
		#endregion

		#region Implementation of IServicePublisher
		/// <summary>
		/// Publish service.
		/// </summary>
		/// <param name="serviceType">Type of service object to publish.</param>
		/// <param name="serviceInstance">Instance of service of type <paramref name="serviceType"/>.</param>
		/// <returns>Disposable cookie to conceal published service</returns>
		public IDisposable Publish(Type serviceType, object serviceInstance)
		{
			if (!_services.TryAdd(serviceType, sp => serviceInstance))
				throw new ArgumentException("Service with the same type already published.");
			// All code below is always run in no more than one thread for specific type
			var removed = false;
			return
				Disposable.Create(
					() =>
					{
						if (!removed)
						{
							ConcealService(serviceType);
							removed = true;
						}
					});
		}

		/// <summary>
		/// Publish service.
		/// </summary>
		/// <param name="serviceType">Type of service object to publish.</param>
		/// <param name="instanceFactory">Factory to create service instance</param>
		/// <returns>Disposable cookie to conceal published service</returns>
		public IDisposable Publish(Type serviceType, Func<IServicePublisher, object> instanceFactory)
		{
			var createLock = new object();
			object instance = null;
			if (!_services.TryAdd(
				serviceType,
				sp =>
				{
					if (instance == null)
						lock (createLock)
							if (instance == null)
								instance = instanceFactory(sp);
					return instance;
				}))
				throw new ArgumentException("Service with the same type already published.");
			// All code below is always run in no more than one thread for specific type
			var removed = false;
			return
				Disposable.Create(
					() =>
					{
						if (!removed)
						{
							ConcealService(serviceType);
							removed = true;
						}
					});
		}
		#endregion
	}
}