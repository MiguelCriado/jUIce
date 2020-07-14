using System;
using System.Reflection;
using UnityEngine;

namespace Maui
{
	public class Binding<T> : IBinding
	{
		public IReadOnlyObservableVariable<T> Property => exposedProperty;
		
		private readonly BindingInfo bindingInfo;
		private readonly Component context;
		private readonly ObservableVariable<T> exposedProperty;
		private IObservableVariable<T> boundProperty;

		public Binding(BindingInfo bindingInfo, Component context)
		{
			this.bindingInfo = bindingInfo;
			this.context = context;
			exposedProperty = new ObservableVariable<T>();
		}

		public void Bind()
		{
			if (bindingInfo.ViewModelContainer == null && string.IsNullOrEmpty(bindingInfo.PropertyName) == false)
			{
				Type targetType = typeof(IReadOnlyObservableVariable<>);
				targetType = targetType.MakeGenericType(typeof(T));

				bindingInfo.ViewModelContainer = FindViewModelComponent(targetType, context.transform);
			}
			
			if (bindingInfo.ViewModelContainer != null)
			{
				if (bindingInfo.ViewModelContainer.ViewModel != null)
				{
					Bind(bindingInfo.ViewModelContainer.ViewModel, bindingInfo.PropertyName);
				}

				bindingInfo.ViewModelContainer.ViewModelChanged += ViewModelChangedHandler;
			}
		}

		public void Unbind()
		{
			if (boundProperty != null)
			{
				boundProperty.Changed -= BoundPropertyChangedHandler;
			}

			if (bindingInfo.ViewModelContainer != null)
			{
				bindingInfo.ViewModelContainer.ViewModelChanged -= ViewModelChangedHandler;
			}

			boundProperty = null;
		}
		
		private ViewModelComponent FindViewModelComponent(Type targetType, Transform transform)
		{
			ViewModelComponent result = null;
			ViewModelComponent[] viewModels = transform.GetComponents<ViewModelComponent>();

			int i = 0;

			while (result == null && i < viewModels.Length)
			{
				ViewModelComponent next = viewModels[i];
				Type type = next.ExpectedType;
					
				if (type != null)
				{
					PropertyInfo propertyInfo = type.GetProperty(bindingInfo.PropertyName);

					if (propertyInfo != null && targetType.IsAssignableFrom(propertyInfo.PropertyType))
					{
						result = next;
					}
				}
					
				i++;
			}

			if (result == null && transform.parent != null)
			{
				result = FindViewModelComponent(targetType, transform.parent);
			}

			return result;
		}

		private void Bind(IViewModel viewModel, string propertyName)
		{
			Type viewModelType = viewModel.GetType();
			PropertyInfo propertyInfo = viewModelType.GetProperty(propertyName);
			object value = propertyInfo?.GetValue(viewModel);
			boundProperty = value as IObservableVariable<T>;

			if (boundProperty != null)
			{
				boundProperty.Changed += BoundPropertyChangedHandler;
				BoundPropertyChangedHandler(boundProperty.Value);
			}
			else
			{
				Debug.LogError($"Property \"{bindingInfo.PropertyName}\" not found in {viewModel.GetType()} class.");
			}
		}

		private void BoundPropertyChangedHandler(T newValue)
		{
			exposedProperty.Value = newValue;
		}

		private void ViewModelChangedHandler(object sender, IViewModel viewModel)
		{
			Unbind();
			Bind(viewModel, bindingInfo.PropertyName);
		}
	}
}