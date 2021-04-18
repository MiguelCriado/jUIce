using System;
using System.Collections.Generic;

namespace Juice
{
	public delegate void SignalDelegate<T>(T signal) where T : ISignal;

	public class SignalBus
	{
		public static SignalBus Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new SignalBus();
				}

				return defaultInstance;
			}
		}

		private static SignalBus defaultInstance;

		private readonly Dictionary<Type, Delegate> subscriptions;

		public SignalBus()
		{
			subscriptions = new Dictionary<Type, Delegate>();
		}

		public void Subscribe<T>(SignalDelegate<T> callback) where T : ISignal
		{
			subscriptions[typeof(T)] = Delegate.Combine(GetDelegate<T>(), callback);
		}

		public void Unsubscribe<T>(SignalDelegate<T> callback) where T : ISignal
		{
			Delegate strippedDelegate = Delegate.Remove(GetDelegate<T>(), callback);

			if (strippedDelegate == null)
			{
				subscriptions.Remove(typeof(T));
			}
			else
			{
				subscriptions[typeof(T)] = strippedDelegate;
			}
		}

		public void Fire<T>(T signal) where T : ISignal
		{
			if (subscriptions.ContainsKey(typeof(T)))
			{
				GetDelegate<T>().Invoke(signal);
			}
		}

		private SignalDelegate<T> GetDelegate<T>() where T : ISignal
		{
			if (subscriptions.TryGetValue(typeof(T), out var result) == false)
			{
				subscriptions.Add(typeof(T), null);
			}

			return (SignalDelegate<T>)result;
		}
	}
}
