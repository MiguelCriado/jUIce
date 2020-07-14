using System;
using System.Reflection;
using UnityEngine;

namespace Maui
{
	public class Binding<T> : IBinding
	{
		public IReadOnlyObservableVariable<T> Property => exposedProperty;
		
		private BindingInfo bindingInfo;
		private Component context;
		private IObservableVariable<T> boundProperty;
		private ObservableVariable<T> exposedProperty;

		public Binding(BindingInfo bindingInfo, Component context)
		{
			this.bindingInfo = bindingInfo;
			this.context = context;
			exposedProperty = new ObservableVariable<T>();
		}

		public void Bind()
		{
			// TODO: find ViewModelContainer if bindingInfo doesn't have one
			
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