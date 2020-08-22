using System;
using System.Collections.Generic;
using System.Reflection;
using Maui.Utils;
using UnityEngine;

namespace Maui
{
	public struct BindingEntry
	{
		public ViewModelComponent ViewModelComponent { get; }
		public string PropertyName { get; }
		public bool NeedsToBeBoxed { get; }

		public BindingEntry(ViewModelComponent viewModelComponent, string propertyName, bool needsToBeBoxed)
		{
			ViewModelComponent = viewModelComponent;
			PropertyName = propertyName;
			NeedsToBeBoxed = needsToBeBoxed;
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
			
			public Type TargetType
			{
				get => targetType;
				set
				{
					genericTypeToCheck = null;
					typeToCheck = null;
					Type genericType = value.GetGenericTypeTowardsRoot();
					
					if (genericType != null)
					{
						Type genericTypeDefinition = genericType.GetGenericTypeDefinition();
						
						if (genericType.GenericTypeArguments.Length > 0 
						    && (genericTypeDefinition.ImplementsOrDerives(GenericVariableType) 
						        || genericTypeDefinition.ImplementsOrDerives(GenericCollectionType)
						        || genericTypeDefinition.ImplementsOrDerives(GenericCommandType)))
						{
							genericTypeToCheck = genericTypeDefinition;
							typeToCheck =  genericType.GenericTypeArguments[0];
						}
					}
					else if (CommandType.IsAssignableFrom(value))
					{
						typeToCheck = CommandType;
					}

					targetType = value;
				}
			}

			private Type targetType;
			private Type genericTypeToCheck;
			private Type typeToCheck;

			public bool CanBeBound(Type type)
			{
				bool result = false;

				if (typeToCheck != null)
				{
					if (genericTypeToCheck != null)
					{
						Type genericType = type.GetGenericTypeTowardsRoot();
						
						if (genericType != null && genericType.GenericTypeArguments.Length > 0)
						{
							Type genericTypeDefinition = type.GetGenericTypeDefinition();
							Type genericArgument = genericType.GenericTypeArguments[0];

							result = genericTypeDefinition.ImplementsOrDerives(genericTypeToCheck)
							         && typeToCheck.IsAssignableFrom(genericArgument);
						}
					}
					else
					{
						result = typeToCheck.IsAssignableFrom(type);
					}
				}

				return result;
			}

			public bool NeedsToBeBoxed(Type type)
			{
				Type genericType = type.GetGenericTypeTowardsRoot();

				return typeToCheck != null
				       && targetType.IsGenericType
				       && genericType != null
				       && genericType.GenericTypeArguments.Length > 0
				       && genericType.GenericTypeArguments[0].IsValueType;
			}
		}

		private static readonly BindingTypeChecker Checker = new BindingTypeChecker();
		
		public static IEnumerable<BindingEntry> GetBindings(Transform context, Type targetType)
		{
			Checker.TargetType = targetType;
			
			foreach (var viewModel in GetComponentsInParents<ViewModelComponent>(context, true))
			{
				Type viewModelType = viewModel.ExpectedType;

				if (viewModelType != null)
				{
					foreach (PropertyInfo propertyInfo in viewModelType.GetProperties())
					{
						if (Checker.CanBeBound(propertyInfo.PropertyType))
						{
							bool needsToBeBoxed = Checker.NeedsToBeBoxed(propertyInfo.PropertyType);
							yield return new BindingEntry(viewModel, propertyInfo.Name, needsToBeBoxed);
						}
					}
				}
			}
		}

		public static bool CanBeBound(Type actualType, Type targetType)
		{
			Checker.TargetType = targetType;
			return Checker.CanBeBound(actualType);
		}

		public static bool NeedsToBeBoxed(Type actualType, Type targetType)
		{
			Checker.TargetType = targetType;
			return Checker.NeedsToBeBoxed(actualType);
		}
		
		private static IEnumerable<T> GetComponentsInParents<T>(Component component, bool includeSelf) where T : Component 
		{
			if (includeSelf)
			{
				foreach (T current in component.GetComponents<T>())
				{
					yield return current;
				}
			}

			if (component.transform.parent != null)
			{
				foreach (T current in GetComponentsInParents<T>(component.transform.parent, true))
				{
					yield return current;
				}
			}
		}
	}
}