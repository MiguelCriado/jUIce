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
			CacheElements(property);
			RefreshCurrentIndex();

			EditorGUI.BeginProperty(position, label, property);

			position = DrawLabel(position, label);

			// Don't make child fields be indented
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			int index = EditorGUI.Popup(position, currentIndex, cachedOptions);

			if (index != currentIndex)
			{
				string selectedType = cachedOptions[index];

				Undo.RecordObject(property.serializedObject.targetObject, $"{fieldInfo.Name} type changed");
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
					if (typeConstraint.BaseType.IsAssignableFrom(type))
					{
						string typeName = type.FullName;
						typeMap.Add(typeName, new TypeEntry(options.Count, type));
						options.Add(typeName);
					}
				}
			}

			cachedOptions = options.ToArray();
		}

		private void RefreshCurrentIndex()
		{
			string typeName = target.Type.FullName;
			currentIndex = typeMap[typeName].Index;
		}
	}
}
