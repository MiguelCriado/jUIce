using System.IO;
using UnityEditor;
using UnityEngine;

namespace Juice.Editor
{
	[CustomPropertyDrawer(typeof(DrawIfAttribute))]
	public class DrawIfPropertyDrawer : PropertyDrawer
	{
		private DrawIfAttribute drawIf;
		private SerializedProperty comparedField;
		private PropertyDrawer cachedPropertyDrawer;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float result = 0f;

			if (ShouldBeShown(property) || drawIf.DisablingMode == DrawIfAttribute.DisablingType.ReadOnly)
			{
				result = base.GetPropertyHeight(property, label);
			}

			return result;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (ShouldBeShown(property))
			{
				DrawProperty(position, property, label);
			}
			else if (drawIf.DisablingMode == DrawIfAttribute.DisablingType.ReadOnly)
			{
				GUI.enabled = false;
				DrawProperty(position, property, label);
				GUI.enabled = true;
			}
		}

		private bool ShouldBeShown(SerializedProperty property)
		{
			drawIf = attribute as DrawIfAttribute;
			string path = property.propertyPath.Contains(".")
				? Path.ChangeExtension(property.propertyPath, drawIf.ComparedPropertyName)
				: drawIf.ComparedPropertyName;

			comparedField = property.serializedObject.FindProperty(path);

			if (comparedField == null)
			{
				Debug.LogError("Cannot find property with name: " + path);
				return true;
			}

			switch (comparedField.type)
			{
				case "bool": return comparedField.boolValue.Equals(drawIf.ComparedValue);
				case "Enum": return comparedField.enumValueIndex.Equals((int) drawIf.ComparedValue);
				default:
					Debug.LogError("Error: " + comparedField.type + " is not supported of " + path);
					return true;
			}
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
