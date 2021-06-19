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

		public ViewModelComponentInfo(ViewModelComponent component)
		{
			properties = new Dictionary<string, object>();
			Component = component;

			Component.ViewModelChanged += OnViewModelChanged;
		}

		public object GetProperty(string name)
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
