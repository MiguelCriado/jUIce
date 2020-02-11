using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Muui
{
	public abstract class BaseLayer<T> : MonoBehaviour where T : IScreenController
	{
		protected Dictionary<Type, T> registeredScreens;

		protected virtual void Awake()
		{
			registeredScreens = new Dictionary<Type, T>();
		}

		public virtual void Initialize()
		{

		}

		public abstract Task ShowScreen(T screen);

		public async Task ShowScreen(Type screenType)
		{
			if (registeredScreens.TryGetValue(screenType, out T controller))
			{
				await ShowScreen(controller);
			}
			else
			{
				Debug.LogError($"Screen with type {screenType} not registered to this layer!");
			}
		}

		public abstract Task ShowScreen<TProps>(T screen, TProps properties) where TProps : IScreenProperties;

		public async Task ShowScreen<TProps>(Type screenType, TProps properties)
			where TProps : IScreenProperties
		{
			if (registeredScreens.TryGetValue(screenType, out T controller))
			{
				await ShowScreen(controller, properties);
			}
			else
			{
				Debug.LogError($"Screen with type {screenType} not registered to this layer!");
			}
		}

		public abstract Task HideScreen(T screen);

		public async Task HideScreen(Type screenType)
		{
			if (registeredScreens.TryGetValue(screenType, out T controller))
			{
				await HideScreen(controller);
			}
			else
			{
				Debug.LogError($"Could not hide screen of type {screenType} as it is not registered to this layer!");
			}
		}

		public virtual Task HideAll(bool animate = true)
		{
			Task[] tasks = new Task[registeredScreens.Count];
			int i = 0;

			foreach (KeyValuePair<Type,T> screenEntry in registeredScreens)
			{
				tasks[i] = screenEntry.Value.Hide(animate);
				i++;
			}

			return Task.WhenAll(tasks);
		}

		public virtual void ReparentScreen(IScreenController controller, Transform screenTransform)
		{
			screenTransform.SetParent(transform, false);
		}

		public void RegisterScreen(T controller)
		{
			Type controllerType = controller.GetType();

			if (registeredScreens.ContainsKey(controllerType) == false)
			{
				ProcessScreenRegister(controller);
			}
			else
			{
				Debug.LogError($"Screen controller already registered for type {controllerType}");
			}
		}

		public void UnregisterScreen(T controller)
		{
			Type controllerType = controller.GetType();

			if (registeredScreens.ContainsKey(controllerType))
			{
				ProcessScreenUnregister(controller);
			}
			else
			{
				Debug.LogError($"Screen controller not registered for type {controllerType}");
			}
		}

		public bool IsScreenRegistered<TScreen>() where TScreen : T
		{
			return registeredScreens.ContainsKey(typeof(TScreen));
		}

		protected virtual void ProcessScreenRegister(T controller)
		{
			registeredScreens.Add(controller.GetType(), controller);
			controller.OnScreenDestroyed += OnScreenDestroyed;
		}

		protected virtual void ProcessScreenUnregister(T controller)
		{
			controller.OnScreenDestroyed += OnScreenDestroyed;
			registeredScreens.Remove(controller.GetType());
		}

		private void OnScreenDestroyed(IScreenController screen)
		{
			if (registeredScreens.ContainsKey(screen.GetType()))
			{
				UnregisterScreen((T)screen);
			}
		}
	}
}
