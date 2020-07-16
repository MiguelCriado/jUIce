using System;
using System.Reflection;
using UnityEngine;

namespace Maui
{
	public abstract class Binding : IBinding
	{
		protected BindingInfo bindingInfo;
		protected Component context;

		public Binding(BindingInfo bindingInfo, Component context)
		{
			this.bindingInfo = bindingInfo;
			this.context = context;
		}
		
		public void Bind()
		{
			if (bindingInfo.ViewModelContainer == null && string.IsNullOrEmpty(bindingInfo.PropertyName) == false)
			{
				Type bindingType = GetBindingType();
				bindingInfo.ViewModelContainer = FindViewModelComponent(bindingType, context.transform);
			}
			
			if (bindingInfo.ViewModelContainer != null)
			{
				if (bindingInfo.ViewModelContainer.ViewModel != null)
				{
					IViewModel viewModel = bindingInfo.ViewModelContainer.ViewModel;
					string propertyName = bindingInfo.PropertyName;
					Type viewModelType = viewModel.GetType();
					PropertyInfo propertyInfo = viewModelType.GetProperty(propertyName);
					object value = propertyInfo?.GetValue(viewModel);

					if (value != null)
					{
						BindProperty(value);
					}
					else
					{
						Debug.LogError($"Property \"{bindingInfo.PropertyName}\" not found in {viewModel.GetType()} class.");
					}
				}

				bindingInfo.ViewModelContainer.ViewModelChanged += ViewModelChangedHandler;
			}
		}

		public void Unbind()
		{
			UnbindProperty();

			if (bindingInfo.ViewModelContainer != null)
			{
				bindingInfo.ViewModelContainer.ViewModelChanged -= ViewModelChangedHandler;
			}
		}

		protected abstract Type GetBindingType();

		protected abstract void BindProperty(object property);

		protected abstract void UnbindProperty();
		
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
		
		private void ViewModelChangedHandler(object sender, IViewModel viewModel)
		{
			Unbind();
			Bind();
		}
	}
}