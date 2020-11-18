using System;
using UnityEditor;
using UnityEngine;

namespace Juice.Editor
{
	public class SerializableDictionaryNewElementWindow : EditorWindow
	{
		private static readonly float LabelWidth = 42f;
		
		private SerializedProperty key;
		private SerializedProperty value;
		private SerializedProperty dictionary;
		private Action<SerializedProperty, SerializedProperty> creationCallback;
		
		public static void Show(
			Rect buttonRect,
			SerializedProperty key,
			SerializedProperty value,
			SerializedProperty dictionary,
			Action<SerializedProperty, SerializedProperty> creationCallback)
		{
			var window = CreateInstance<SerializableDictionaryNewElementWindow>();
			window.key = key;
			window.value = value;
			window.dictionary = dictionary;
			window.creationCallback = creationCallback;
			
			float width = 200;
			float height = EditorGUI.GetPropertyHeight(key);
			height += EditorGUI.GetPropertyHeight(value);
			height += EditorGUIUtility.singleLineHeight;
			height += EditorGUIUtility.standardVerticalSpacing + 7;
			Vector2 windowSize = new Vector2(width, height);

			buttonRect.y -= buttonRect.height;
			buttonRect.x -= (windowSize.x) / 2f;
			
			window.ShowAsDropDown(buttonRect, windowSize);
		}

		private void OnGUI()
		{
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.LabelField("Key", GUILayout.MaxWidth(LabelWidth));
				EditorGUILayout.PropertyField(key, GUIContent.none);
			}

			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.LabelField("Value", GUILayout.MaxWidth(LabelWidth));
				EditorGUILayout.PropertyField(value, GUIContent.none);
			}
			
			GUI.enabled = IsKeyAvailable();
			
			if (GUILayout.Button("Add"))
			{
				creationCallback.Invoke(key, value);
				Close();
			}

			GUI.enabled = true;
		}

		private bool IsKeyAvailable()
		{
			SerializedProperty keys = dictionary.FindPropertyRelative("keys");

			bool isTaken = false;
			int i = 0;

			while (isTaken == false && i < keys.arraySize)
			{
				isTaken = keys.GetArrayElementAtIndex(i).GetPropertyValue().Equals(key.GetPropertyValue());

				i++;
			}

			return !isTaken;
		}
	}
}