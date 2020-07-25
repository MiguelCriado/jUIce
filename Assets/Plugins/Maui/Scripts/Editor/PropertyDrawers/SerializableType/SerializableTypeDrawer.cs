using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Maui.Editor
{
	[CustomPropertyDrawer(typeof(SerializableType))]
	public class SerializableTypeDrawer : PropertyDrawer
	{
		private class TypeEntry
		{
			public int Index;
			public Type Type;

			public TypeEntry(int index, Type type)
			{
				Index = index;
				Type = type;
			}
		}

		private static readonly string TypeReferenceFieldName;

		private bool isCached;
		private SerializableType target;
		private Dictionary<string, TypeEntry> typeMap;
		private string[] cachedOptions;
		private int currentIndex;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			CacheElements(property);
			RefreshCurrentIndex(property);

			position = DrawLabel(position, label);

			// Don't make child fields be indented
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			int index = EditorGUI.Popup(position, currentIndex, cachedOptions);

			if (index != currentIndex)
			{
				Undo.RecordObject(property.serializedObject.targetObject, $"{fieldInfo.Name} type changed");
				
				string selectedType = cachedOptions[index];
				target.Type = typeMap[selectedType].Type;

				currentIndex = index;
			}

			// Set indent back to what it was
			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}

		private static Rect DrawLabel(Rect position, GUIContent label)
		{
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			return position;
		}

		private void CacheElements(SerializedProperty property)
		{
			if (isCached == false)
			{
				CacheTarget(property);
				CacheTypeCollections();

				isCached = true;
			}
		}

		private void CacheTarget(SerializedProperty property)
		{
			target = PropertyDrawerUtility.GetActualObjectForSerializedProperty<SerializableType>(fieldInfo, property);
		}

		private void CacheTypeCollections()
		{
			TypeConstraintAttribute typeConstraint = fieldInfo.GetCustomAttribute<TypeConstraintAttribute>();
			typeMap = new Dictionary<string, TypeEntry>();
			List<string> options = new List<string>();

			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (IsMatchingType(type, typeConstraint))
					{
						string typeName = type.FullName;

						if (typeMap.ContainsKey(typeName) == false)
						{
							typeMap.Add(typeName, new TypeEntry(options.Count, type));
							options.Add(typeName);
						}
					}
				}
			}

			cachedOptions = options.ToArray();
		}

		private static bool IsMatchingType(Type type, TypeConstraintAttribute constraint)
		{
			return  constraint == null 
			        || (DoesMatchInstantiableRestriction(type, constraint)
			            && IsAssignableFrom(constraint.BaseType, type));
		}

		private static bool DoesMatchInstantiableRestriction(Type type, TypeConstraintAttribute constraint)
		{
			return !constraint.ForceInstantiableType ||
			       (!type.IsAbstract && !type.IsInterface && !type.IsGenericTypeDefinition);
		}

		private static bool IsAssignableFrom(Type a, Type b)
		{
			return a.IsAssignableFrom(b) || IsAssignableToGenericType(b, a);
		}

		private static bool IsAssignableToGenericType(Type givenType, Type genericType)
		{
			var interfaceTypes = givenType.GetInterfaces();

			foreach (var it in interfaceTypes)
			{
				if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
				{
					return true;
				}
			}

			if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
			{
				return true;
			}

			Type baseType = givenType.BaseType;

			if (baseType == null)
			{
				return false;
			}

			return IsAssignableToGenericType(baseType, genericType);
		}

		private void RefreshCurrentIndex(SerializedProperty property)
		{
			if (target.Type != null)
			{
				string typeName = target.Type.FullName;

				if (typeMap.TryGetValue(typeName, out TypeEntry entry))
				{
					currentIndex = entry.Index;
				}
			}
			else if (typeMap.Count > 0)
			{
				TypeEntry firstEntry = typeMap[cachedOptions[0]];
				target.Type = firstEntry.Type;
			}
		}
	}
}
