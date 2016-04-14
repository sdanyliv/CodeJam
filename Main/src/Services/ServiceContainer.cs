﻿using System;
using System.Collections.Concurrent;
using System.Linq;

using JetBrains.Annotations;

namespace CodeJam.Services
{
	/// <summary>
	/// Service container.
	/// </summary>
	[PublicAPI]
	public class ServiceContainer : IServicePublisher, IDisposable
	{
		[CanBeNull]
		private readonly IServiceProvider _parentProvider;

		private readonly ConcurrentDictionary<Type, IServiceBag> _services =
			new ConcurrentDictionary<Type, IServiceBag>();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
		public ServiceContainer([CanBeNull] IServiceProvider parentProvider, bool publishSelf = true)
		{
			_parentProvider = parentProvider;
			if (publishSelf)
				this.Publish<IServicePublisher>(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
		public ServiceContainer(bool publishSelf = true) : this(null, publishSelf) { }

		private void ConcealService(Type serviceType)
		{
			IServiceBag bag;
			if (!_services.TryRemove(serviceType, out bag))
				throw new ArgumentException($"Service with type '{serviceType}' not registered.");
			bag.Dispose();
		}

		#region Implementation of IServiceProvider
		/// <summary>Gets the service object of the specified type.</summary>
		/// <returns>A service object of type <paramref name="serviceType" />.-or- null if there is no service object of type <paramref name="serviceType" />.</returns>
		/// <param name="serviceType">An object that specifies the type of service object to get. </param>
		[CanBeNull]
		public object GetService(Type serviceType)
		{
			IServiceBag bag;
			return
				_services.TryGetValue(serviceType, out bag)
					? bag.GetInstance(this, serviceType)
					: _parentProvider?.GetService(serviceType);
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
			if (!_services.TryAdd(serviceType, new InstanceBag(serviceInstance)))
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
			if (!_services.TryAdd(serviceType, new FactoryBag(instanceFactory)))
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

		#region ServiceBag classes
		private interface IServiceBag
		{
			object GetInstance(IServicePublisher publisher, Type serviceType);
			void Dispose();
		}

		private class InstanceBag : IServiceBag
		{
			private readonly object _instance;
			public InstanceBag(object instance)
			{
				_instance = instance;
			}

			#region Overrides of ServiceBag
			public object GetInstance(IServicePublisher publisher, Type serviceType) => _instance;

			// Do not dispose services created outside.
			public void Dispose() { }
			#endregion
		}

		private class FactoryBag : IServiceBag
		{
			private readonly Func<IServicePublisher, object> _factory;
			private object _instance;

			/// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
			public FactoryBag(Func<IServicePublisher, object> factory)
			{
				_factory = factory;
			}

			#region Overrides of ServiceBag
			public object GetInstance(IServicePublisher publisher, Type serviceType)
			{
				if (_instance == null)
					lock (this)
						if (_instance == null)
						{
							_instance = _factory(publisher);
							if (_instance == null)
								throw new InvalidOperationException($"Factory for service of type '{serviceType}' returns null");
						}
				return _instance;
			}

			public void Dispose()
			{
				if (_instance != null)
					lock (this)
						(_instance as IDisposable)?.Dispose();
			}
			#endregion
		}
		#endregion

		#region Implementation of IDisposable
		/// <summary>
		/// Calls <see cref="IDisposable.Dispose"/> methods in all created service instances, that implements
		/// <see cref="IDisposable"/>.
		/// </summary>
		public void Dispose()
		{
			while (_services.Count > 0)
			{
				var type = _services.Keys.FirstOrDefault();
				if (type != null)
				{
					IServiceBag bag;
					if (_services.TryRemove(type, out bag))
						bag.Dispose();
				}
			}
		}
		#endregion
	}
}