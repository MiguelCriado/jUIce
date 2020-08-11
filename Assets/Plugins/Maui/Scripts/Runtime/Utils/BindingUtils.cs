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
			
			public Type TargetType
			{
				get => targetType;
				set
				{
					genericTypeToCheck = null;
					typeToCheck = null;
					Type genericType = GetGenericTypeInHierarchy(value);
					
					if (genericType != null)
					{
						Type genericTypeDefinition = genericType.GetGenericTypeDefinition();
						
						if (genericType.GenericTypeArguments.Length > 0 
						    && (ImplementsOrDerives(genericTypeDefinition, GenericVariableType) 
						        || ImplementsOrDerives(genericTypeDefinition, GenericCollectionType)
						        || ImplementsOrDerives(genericTypeDefinition, GenericCommandType)))
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
						Type genericType = GetGenericTypeInHierarchy(type);
						
						if (genericType != null && genericType.GenericTypeArguments.Length > 0)
						{
							Type genericTypeDefinition = type.GetGenericTypeDefinition();
							Type genericArgument = genericType.GenericTypeArguments[0];

							result = ImplementsOrDerives(genericTypeDefinition, genericTypeToCheck)
							         && (typeToCheck == genericArgument
							             || (!typeToCheck.IsValueType && typeToCheck.IsAssignableFrom(genericArgument)) 
							             || (typeToCheck.IsValueType && AdapterSupportsType(typeToCheck, genericArgument)));
						}
					}
					else
					{
						result = typeToCheck.IsAssignableFrom(type);
					}
				}

				return result;
			}

			public bool CanBeAdapted(Type type)
			{
				bool result = false;
				Type genericType = GetGenericTypeInHierarchy(type);

				if (typeToCheck != null && targetType.IsGenericType && genericType != null && genericType.GenericTypeArguments.Length > 0)
				{
					Type genericArgument = type.GenericTypeArguments[0];
					result = AdapterSupportsType(targetType, genericArgument);
				}

				return result;
			}

			private bool ImplementsOrDerives(Type type, Type baseType)
			{
				bool result = false;

				if (type != null)
				{
					if (baseType.IsGenericType)
					{
						if (baseType.IsGenericTypeDefinition)
						{
							if (baseType.IsInterface)
							{
								int i = 0;
								Type[] interfaces = type.GetInterfaces();

								while (result == false && i < interfaces.Length)
								{
									Type @interface = interfaces[i];
									result = @interface.IsGenericType && @interface.GetGenericTypeDefinition() == baseType;
									i++;
								}
							}
							
							result |= type.IsGenericType && type.GetGenericTypeDefinition() == baseType;
						}
						else
						{
							result = baseType.IsAssignableFrom(type);
						}
					}
					else
					{
						result = baseType.IsAssignableFrom(type);
					}
					
					result |= ImplementsOrDerives(type.BaseType, baseType);
				}

				return result;
			}

			private Type GetGenericTypeInHierarchy(Type type)
			{
				Type result = null;

				if (type != null)
				{
					if (type.IsGenericType)
					{
						result = type;
					}
					else
					{
						result = GetGenericTypeInHierarchy(type.BaseType);
					}
				}

				return result;
			}

			private bool AdapterSupportsType(Type genericType, Type parameterType)
			{
				bool result = false;

				if (ImplementsOrDerives(genericType, GenericVariableType))
				{
					result = ObservableVariableAdapter.SupportedTypes.Contains(parameterType);
				}
				else if (ImplementsOrDerives(genericType, GenericCollectionType))
				{
					// TODO
				}
				else if (ImplementsOrDerives(genericType, GenericCommandType))
				{
					// TODO
				}

				return result;
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
							yield return new BindingEntry(viewModel, propertyInfo.Name);
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

		public static bool CanBeAdapted(Type actualType, Type targetType)
		{
			Checker.TargetType = targetType;
			return Checker.CanBeAdapted(actualType);
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