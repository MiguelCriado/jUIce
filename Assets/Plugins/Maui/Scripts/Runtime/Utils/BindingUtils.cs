using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Maui
{
	public struct BindingEntry
	{
		public ViewModelComponent ViewModelComponent { get; }
		public string PropertyName { get; }

		public BindingEntry(ViewModelComponent viewModelComponent, string propertyName)
		{
			ViewModelComponent = viewModelComponent;
			PropertyName = propertyName;
		}
	}
	
	public static class BindingUtils
	{
		private class BindingTypeChecker
		{
			private static readonly Type GenericVariableType = typeof(IReadOnlyObservableVariable<>);
			private static readonly Type GenericCollectionType = typeof(IReadOnlyObservableCollection<>);
			private static readonly Type GenericCommandType = typeof(IObservableCommand<>);
			private static readonly Type CommandType = typeof(IObservableCommand);
			
			public Type Type
			{
				get => type;
				set
				{
					isGenericType = value.IsGenericType;
					
					if (isGenericType)
					{
						Type genericType = value.GetGenericTypeDefinition();
						
						if (value.GenericTypeArguments.Length > 0 
						    && (genericType.IsAssignableFrom(GenericVariableType) 
						        || genericType.IsAssignableFrom(GenericCollectionType) 
						        || genericType.IsAssignableFrom(GenericCommandType)))
						{
							genericTypeToCheck = genericType;
							typeToCheck =  value.GenericTypeArguments[0];
						}
					}
					else if (value.IsAssignableFrom(CommandType))
					{
						typeToCheck = CommandType;
					}

					type = value;
				}
			}

			private Type type;
			private bool isGenericType;
			private Type genericTypeToCheck;
			private Type typeToCheck;

			public bool Check(Type type)
			{
				bool result = false;

				if (typeToCheck != null)
				{
					if (isGenericType)
					{
						if (type.IsGenericType && type.GenericTypeArguments.Length > 0)
						{
							Type genericType = type.GetGenericTypeDefinition();
							Type genericParameter = type.GenericTypeArguments[0];

							result = genericTypeToCheck.IsAssignableFrom(genericType) 
							         && typeToCheck.IsAssignableFrom(genericParameter);
						}
					}
					else
					{
						result = typeToCheck.IsAssignableFrom(type);
					}
				}

				return result;
			}
		}

		private static readonly BindingTypeChecker Checker = new BindingTypeChecker();
		
		public static IEnumerable<BindingEntry> GetBindings(Transform context, Type targetType)
		{
			Checker.Type = targetType;
			ViewModelComponent viewModel = GetComponentInParents<ViewModelComponent>(context, true);

			while (viewModel != null)
			{
				Type viewModelType = viewModel.ExpectedType;

				if (viewModelType != null)
				{
					foreach (PropertyInfo propertyInfo in viewModelType.GetRuntimeProperties())
					{
						if (Checker.Check(propertyInfo.PropertyType))
						{
							yield return new BindingEntry(viewModel, propertyInfo.Name);
						}
					}
				}

				viewModel = GetComponentInParents<ViewModelComponent>(viewModel, false);
			}
		}
		
		private static T GetComponentInParents<T>(Component component, bool includeSelf) where T : Component 
		{
			T result = null;

			if (includeSelf)
			{
				result = component.GetComponent<T>();
			}

			if (result == null && component.transform.parent != null)
			{
				result = GetComponentInParents<T>(component.transform.parent, true);
			}

			return result;
		}
	}
}