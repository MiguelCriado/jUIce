using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Juice.Editor
{
	[CustomPropertyDrawer(typeof(RenameAttribute))]
	public class RenameAttributeDrawer : PropertyDrawer
	{
		private PropertyDrawer cachedPropertyDrawer;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			GUIContent finalLabel = ResolveFinalLabel(property, label);
			DrawProperty(position, property, finalLabel);
		}

		private GUIContent ResolveFinalLabel(SerializedProperty property, GUIContent label)
		{
			GUIContent result = label;

			string propertyLookupName = (attribute as RenameAttribute)?.PropertyLookupName;

			if (string.IsNullOrEmpty(propertyLookupName) == false)
			{
				Type type = property.serializedObject.targetObject.GetType();
				PropertyInfo propertyInfo = type.GetProperty(propertyLookupName,
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

				if (propertyInfo?.GetValue(property.serializedObject.targetObject) is string newName)
				{
					result = new GUIContent(newName);
				}
			}

			return result;
		}

		private void DrawProperty(Rect position, SerializedProperty property, GUIContent label)
		{
			CachePropertyDrawer();

			if (cachedPropertyDrawer != null)
			{
				cachedPropertyDrawer.OnGUI(position, property, label);
			}
			else
			{
				EditorGUI.PropertyField(position, property, label);
			}
		}

		private void CachePropertyDrawer()
		{
			if (cachedPropertyDrawer == null)
			{
				cachedPropertyDrawer = PropertyDrawerUtility.GetCustomPropertyDrawer(fieldInfo);
			}
		}
	}
}
