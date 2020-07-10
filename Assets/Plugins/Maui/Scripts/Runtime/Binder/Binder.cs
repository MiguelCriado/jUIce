using System;
using System.Reflection;
using UnityEngine;

namespace Maui
{
	public abstract class Binder<T> : MonoBehaviour, IBinder<T>
	{
		[SerializeField] private ViewModelComponent viewModelContainer;
		[SerializeField] private string propertyName;

		private IObservableVariable<T> boundProperty;

		protected virtual void Reset()
		{
			viewModelContainer = GetComponentInParent<ViewModelComponent>();

			IViewModel viewModel = viewModelContainer.ViewModel;
			PropertyInfo[] properties = viewModel?.GetType().GetProperties();

			if (properties != null)
			{
				PropertyInfo suggestedProperty = null;
				int i = 0;

				while (suggestedProperty == null && i < properties.Length)
				{
					PropertyInfo property = properties[i];

					if (typeof(IObservableVariable<T>).IsAssignableFrom(property.PropertyType))
					{
						suggestedProperty = property;
					}

					i++;
				}
			}
		}

		protected virtual void OnEnable()
		{
			if (viewModelContainer != null)
			{
				if (viewModelContainer.ViewModel != null)
				{
					Bind(viewModelContainer.ViewModel);
				}

				viewModelContainer.ViewModelChanged += ViewModelChangedHandler;
			}
		}

		protected virtual void OnDisable()
		{
			if (viewModelContainer != null)
			{
				Unbind();

				viewModelContainer.ViewModelChanged -= ViewModelChangedHandler;
			}
		}

		protected abstract void Refresh(T value);

		private void Bind(IViewModel viewModel)
		{
			Type viewModelType = viewModel.GetType();
			PropertyInfo propertyInfo = viewModelType.GetProperty(propertyName);
			object value = propertyInfo?.GetValue(viewModel);
			boundProperty = value as IObservableVariable<T>;

			if (boundProperty != null)
			{
				Refresh(boundProperty.Value);

				if (boundProperty != null)
				{
					boundProperty.Changed += OnBoundPropertyChanged;
				}
			}
			else
			{
				Debug.LogError($"Property \"{propertyName}\" not found in {viewModel.GetType()} class.");
			}
		}

		private void Unbind()
		{
			if (boundProperty != null)
			{
				boundProperty.Changed -= OnBoundPropertyChanged;
			}
		}

		private void OnBoundPropertyChanged(T newValue)
		{
			Refresh(newValue);
		}

		private void ViewModelChangedHandler(object sender, IViewModel viewModel)
		{
			Unbind();
			Bind(viewModel);
		}
	}
}
