using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Maui.Editor
{
	[CustomPropertyDrawer(typeof(BindingType))]
	public class BindingTypeDrawer : PropertyDrawer
	{
		private static readonly GUIContent[] AllBindings;

		static BindingTypeDrawer()
		{
			List<GUIContent> allBindings = new List<GUIContent>();
			
			foreach (string current in Enum.GetNames(typeof(BindingType)))
			{
				allBindings.Add(new GUIContent(current));
			}

			AllBindings = allBindings.ToArray();
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			AllowedBindingTypesAttribute attribute = fieldInfo.GetCustomAttribute<AllowedBindingTypesAttribute>();
			GUIContent[] options = null;

			if (attribute != null)
			{
				options = GetAllowedTypesInAttribute(property, attribute);
			}
			else
			{
				options = AllBindings;
			}

			int currentIndex = GetIndex(options, property);
			int newIndex = EditorGUI.Popup(position, label, currentIndex, options);

			if (newIndex != currentIndex)
			{
				property.intValue = (int) GetValue(options, newIndex);
			}
			
			EditorGUI.EndProperty();
		}

		private static GUIContent[] GetAllowedTypesInAttribute(SerializedProperty property, AllowedBindingTypesAttribute attribute)
		{
			GUIContent[] result = null;
			BindingType[] rawData = null;

			if (attribute.AllowedTypes != null)
			{
				rawData = attribute.AllowedTypes;
			}
			else
			{
				BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
				PropertyInfo propertyInfo = property.serializedObject.targetObject.GetType().GetProperty(attribute.Descriptor, flags);

				if (propertyInfo == null)
				{
					Debug.LogError($"Property {attribute.Descriptor} not found in {property.serializedObject}");
				}
				else
				{
					rawData = propertyInfo.GetValue(property.serializedObject.targetObject) as BindingType[];
				}
			}

			if (rawData != null && rawData.Length > 0)
			{
				result = rawData
					.OrderBy((a) => (int) a)
					.Select(x => new GUIContent(Enum.GetName(typeof(BindingType), x)))
					.ToArray();
			}
			else
			{
				result = AllBindings;
			}

			return result;
		}

		private int GetIndex(GUIContent[] options, SerializedProperty property)
		{
			int result = -1;
			BindingType value = (BindingType)property.intValue;

			int i = 0;

			while (result == -1 && i < options.Length)
			{
				if ((BindingType) Enum.Parse(typeof(BindingType), options[i].text) == value)
				{
					result = i;
				}
				
				i++;
			}

			return result;
		}

		private BindingType GetValue(GUIContent[] options, int index)
		{
			return (BindingType) Enum.Parse(typeof(BindingType), options[index].text);
		}
	}
}