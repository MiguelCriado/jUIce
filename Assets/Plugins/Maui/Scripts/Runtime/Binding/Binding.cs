using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Maui
{
	public abstract class Binding : IBinding
	{
		protected readonly BindingInfo bindingInfo;
		protected readonly Component context;

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
				bindingInfo.ViewModelContainer = FindViewModelComponent(context.transform, bindingType);
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
		
		private static ViewModelComponent FindViewModelComponent(Transform context, Type targetType)
		{
			ViewModelComponent result = null;
			
			using (var iterator = BindingUtils.GetBindings(context, targetType).GetEnumerator())
			{
				if (iterator.MoveNext())
				{
					result = iterator.Current.ViewModelComponent;
				}
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