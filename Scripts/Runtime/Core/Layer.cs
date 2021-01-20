using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public abstract class Layer<TView, TShowSettings, THideSettings> : MonoBehaviour
		where TView : IView
		where TShowSettings : IViewShowSettings
		where THideSettings : IViewHideSettings
	{
		protected readonly Dictionary<Type, TView> registeredViews = new Dictionary<Type, TView>();

		protected UIFrame uiFrame;

		public virtual void Initialize(UIFrame uiFrame)
		{
			this.uiFrame = uiFrame;
		}

		public void ShowView(TShowSettings settings)
		{
			if (registeredViews.TryGetValue(settings.ViewType, out TView view))
			{
				ShowView(view, settings);
			}
			else
			{
				Debug.LogError($"View with type {settings.ViewType} not registered to this layer!");
			}
		}

		public async Task HideView(THideSettings settings)
		{
			if (registeredViews.TryGetValue(settings.ViewType, out TView view))
			{
				await HideView(view, settings);
			}
			else
			{
				Debug.LogError($"Could not hide view of type {settings.ViewType} as it is not registered to this layer!");
			}
		}

		public virtual Task HideAll()
		{
			Task[] tasks = new Task[registeredViews.Count];
			int i = 0;

			foreach (KeyValuePair<Type,TView> viewEntry in registeredViews)
			{
				tasks[i] = viewEntry.Value.Hide();
				i++;
			}

			return Task.WhenAll(tasks);
		}

		public virtual void ReparentView(IView view, Transform viewTransform)
		{
			viewTransform.SetParent(transform, false);
		}

		public void RegisterView(TView view)
		{
			Type viewType = view.GetType();

			if (registeredViews.ContainsKey(viewType) == false)
			{
				ProcessViewRegister(view);
			}
			else
			{
				Debug.LogError($"View view already registered for type {viewType}");
			}
		}

		public void UnregisterView(TView view)
		{
			Type viewType = view.GetType();

			if (registeredViews.ContainsKey(viewType))
			{
				ProcessViewUnregister(view);
			}
			else
			{
				Debug.LogError($"View view not registered for type {viewType}");
			}
		}

		public bool IsViewRegistered<T>() where T : TView
		{
			return registeredViews.ContainsKey(typeof(T));
		}

		protected abstract void ShowView(TView view, TShowSettings settings);

		protected abstract Task HideView(TView view, THideSettings settings);

		protected virtual void ProcessViewRegister(TView view)
		{
			registeredViews.Add(view.GetType(), view);
			view.ViewDestroyed += OnViewDestroyed;
		}

		protected virtual void ProcessViewUnregister(TView view)
		{
			view.ViewDestroyed -= OnViewDestroyed;
			registeredViews.Remove(view.GetType());
		}

		private void OnViewDestroyed(IView view)
		{
			if (registeredViews.ContainsKey(view.GetType()))
			{
				UnregisterView((TView)view);
			}
		}
	}
}
