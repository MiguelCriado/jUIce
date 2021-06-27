using System;
using System.Collections.Generic;
using System.Reflection;

namespace Juice
{
	public class ViewModelComponentInfo
	{
		public string Id => Component.Id;
		public ViewModelComponent Component { get; }

		private readonly Dictionary<string, object> properties;
		private readonly Func<string, object> getProperty;

		public ViewModelComponentInfo(ViewModelComponent component)
		{
			Component = component;

			if (AotBridge.CanGetProperties)
			{
				getProperty = GetAotProperty;
			}
			else if (JitBridge.IsAvailable)
			{
				getProperty = GetJitProperty;
			}
			else
			{
				properties = new Dictionary<string, object>();
				getProperty = GetReflectionProperty;

				Component.ViewModelChanged += OnViewModelChanged;
			}
		}

		public object GetProperty(string name)
		{
			return getProperty(name);
		}

		private object GetAotProperty(string name)
		{
			return AotBridge.GetProperty(Component.ViewModel, name);
		}

		private object GetJitProperty(string name)
		{
			return JitBridge.GetProperty(Component.ViewModel, name);
		}

		private object GetReflectionProperty(string name)
		{
			if (properties.TryGetValue(name, out var result) == false && Component.ViewModel != null)
			{
				Type viewModelType = Component.ViewModel.GetType();
				PropertyInfo propertyInfo = viewModelType.GetProperty(name);

				if (propertyInfo != null)
				{
					result = propertyInfo.GetValue(Component.ViewModel);

					if (result != null)
					{
						properties[name] = result;
					}
				}
			}

			return result;
		}

		private void OnViewModelChanged(IViewModelProvider<IViewModel> source, IViewModel lastViewmodel, IViewModel newViewmodel)
		{
			properties.Clear();
		}
	}
}
