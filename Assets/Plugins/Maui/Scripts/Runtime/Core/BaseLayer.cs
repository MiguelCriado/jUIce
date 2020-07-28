using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Maui
{
	public abstract class BaseLayer<T> : MonoBehaviour where T : IView
	{
		protected readonly Dictionary<Type, T> registeredViews = new Dictionary<Type, T>();

		public virtual void Initialize()
		{

		}

		public abstract Task ShowView(T view);

		public async Task ShowView(Type viewType)
		{
			if (registeredViews.TryGetValue(viewType, out T controller))
			{
				await ShowView(controller);
			}
			else
			{
				Debug.LogError($"View with type {viewType} not registered to this layer!");
			}
		}

		public abstract Task ShowView<TViewModel>(T view, TViewModel properties) where TViewModel : IViewModel;

		public async Task ShowView<TViewModel>(Type viewType, TViewModel properties)
			where TViewModel : IViewModel
		{
			if (registeredViews.TryGetValue(viewType, out T controller))
			{
				await ShowView(controller, properties);
			}
			else
			{
				Debug.LogError($"View with type {viewType} not registered to this layer!");
			}
		}

		public abstract Task HideView(T view);

		public async Task HideView(Type viewType)
		{
			if (registeredViews.TryGetValue(viewType, out T controller))
			{
				await HideView(controller);
			}
			else
			{
				Debug.LogError($"Could not hide view of type {viewType} as it is not registered to this layer!");
			}
		}

		public virtual Task HideAll(bool animate = true)
		{
			Task[] tasks = new Task[registeredViews.Count];
			int i = 0;

			foreach (KeyValuePair<Type,T> viewEntry in registeredViews)
			{
				tasks[i] = viewEntry.Value.Hide(animate);
				i++;
			}

			return Task.WhenAll(tasks);
		}

		public virtual void ReparentView(IView view, Transform viewTransform)
		{
			viewTransform.SetParent(transform, false);
		}

		public void RegisterView(T view)
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

		public void UnregisterView(T view)
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

		public bool IsViewRegistered<TView>() where TView : T
		{
			return registeredViews.ContainsKey(typeof(TView));
		}

		protected virtual void ProcessViewRegister(T view)
		{
			registeredViews.Add(view.GetType(), view);
			view.ViewDestroyed += OnViewDestroyed;
		}

		protected virtual void ProcessViewUnregister(T view)
		{
			view.ViewDestroyed += OnViewDestroyed;
			registeredViews.Remove(view.GetType());
		}

		private void OnViewDestroyed(IView view)
		{
			if (registeredViews.ContainsKey(view.GetType()))
			{
				UnregisterView((T)view);
			}
		}
	}
}
