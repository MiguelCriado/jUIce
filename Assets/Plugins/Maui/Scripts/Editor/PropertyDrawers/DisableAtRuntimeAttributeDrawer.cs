using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Maui.Editor
{
	[CustomPropertyDrawer(typeof(DisableAtRuntimeAttribute))]
	public class DisableAtRuntimeAttributeDrawer : PropertyDrawer
	{
		private PropertyDrawer cachedPropertyDrawer;
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginDisabledGroup(Application.isPlaying);

			CachePropertyDrawer();
			
			if (cachedPropertyDrawer != null)
			{
				cachedPropertyDrawer.OnGUI(position, property, label);
			}
			else
			{
				EditorGUI.PropertyField(position, property, label, true);
			}
			
			EditorGUI.EndDisabledGroup();
		}

		private void CachePropertyDrawer()
		{
			if (cachedPropertyDrawer == null)
			{
				cachedPropertyDrawer = GetPropertyDrawer();
			}
		}
		
		private PropertyDrawer GetPropertyDrawer()
		{
			// Getting the field type this way assumes that the property instance is not a managed reference (with a SerializeReference attribute); if it was, it should be retrieved in a different way:
			Type fieldType = fieldInfo.FieldType;

			Type propertyDrawerType = (Type) Type.GetType("UnityEditor.ScriptAttributeUtility,UnityEditor")
				.GetMethod("GetDrawerTypeForType", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
				.Invoke(null, new object[] {fieldType});

			PropertyDrawer propertyDrawer = null;

			if (typeof(PropertyDrawer).IsAssignableFrom(propertyDrawerType))
			{
				propertyDrawer = (PropertyDrawer) Activator.CreateInstance(propertyDrawerType);
			}

			if (propertyDrawer != null)
			{
				typeof(PropertyDrawer)
					.GetField("m_FieldInfo", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					.SetValue(propertyDrawer, fieldInfo);
			}

			return propertyDrawer;
		}
	}
}