using System.Collections.Generic;
using Maui.Collections;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Maui.Editor
{
	[CustomPropertyDrawer(typeof(SerializableDictionary), true)]
	public class SerializableDictionaryDrawer : PropertyDrawer
	{
		private class DrawerCache
		{
			public SerializedProperty DictionaryProperty;
			public ReorderableList ReorderableList;
		}

		private static readonly float HeaderPadding = 6f;
		private static readonly float ReorderableListHandlerWidth = 20f;
		private static readonly float ElementMargin = EditorGUIUtility.standardVerticalSpacing;
		private static readonly float ElementSpacerWidth = EditorGUIUtility.standardVerticalSpacing;
		private static readonly string KeysName = "keys";
		private static readonly string ValuesName = "values";
		private static readonly string NewKeyHelperName = "newKeyHelper";
		private static readonly string NewValueHelperName = "newValueHelper";

		private static readonly Dictionary<string, DrawerCache> CacheMap = new Dictionary<string, DrawerCache>();

		private DrawerCache cache;

		~SerializableDictionaryDrawer()
		{
			CacheMap.Clear();
		}
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			
			CacheData(property);
			cache.ReorderableList.DoList(position);
			
			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			CacheData(property);
			return cache.ReorderableList.GetHeight();
		}

		private void CacheData(SerializedProperty property)
		{
			string id = property.GetUniqueId();
			object[] listProperty = property.FindPropertyRelative(KeysName).GetPropertyValueArray();

			if (CacheMap.TryGetValue(id, out cache))
			{
				cache.ReorderableList.list = listProperty;
			}
			else
			{
				cache = new DrawerCache();
				cache.ReorderableList = CreateList(listProperty);
				CacheMap[id] = cache;
			}
			
			cache.DictionaryProperty = property;
		}

		private ReorderableList CreateList(object[] keysProperty)
		{
			var result = new ReorderableList(keysProperty, typeof(object));
			result.headerHeight = EditorGUIUtility.singleLineHeight * 2 
			                      + EditorGUIUtility.standardVerticalSpacing;
			result.drawHeaderCallback += DrawHeaderCallback;
			result.elementHeightCallback += ElementHeightCallback;
			result.drawElementCallback += DrawElementCallback;
			result.onReorderCallbackWithDetails += ReorderCallback;
			result.onAddDropdownCallback += AddDropdownCallback;
			result.onRemoveCallback += RemoveCallback;
			return result;
		}

		private void DrawHeaderCallback(Rect rect)
		{
			Vector2 labelPosition = rect.position;
			Vector2 labelSize = new Vector2(rect.width, rect.height / 2);
			Rect labelRect = new Rect(labelPosition, labelSize);
			EditorGUI.LabelField(labelRect, cache.DictionaryProperty.displayName);
			
			Vector2 headersPosition = new Vector2(rect.x - HeaderPadding, labelRect.yMax);
			Vector2 headersSize = new Vector2(rect.width + HeaderPadding * 2, rect.height / 2);
			Rect headersRect = new Rect(headersPosition, headersSize);
			
			GUI.Box(headersRect, GUIContent.none);
			
			Vector2 spacerPosition = new Vector2(headersRect.x, headersRect.y);
			Vector2 spacerSize = new Vector2(ReorderableListHandlerWidth, headersRect.height);
			Rect spacerRect = new Rect(spacerPosition, spacerSize);
			
			Vector2 keysPosition = new Vector2(headersRect.x + spacerRect.width, headersRect.y);
			Vector2 keysSize = new Vector2(EditorGUIUtility.labelWidth, headersRect.height);
			Rect keysRect = new Rect(keysPosition, keysSize);
			EditorGUI.LabelField(keysRect, "Keys");
			
			Vector2 valuesPosition = new Vector2(headersRect.x + spacerRect.width + keysRect.width, headersRect.y);
			Vector2 valuesSize = new Vector2(headersRect.width - spacerRect.width - keysRect.width, headersRect.height);
			Rect valuesRect = new Rect(valuesPosition, valuesSize);
			EditorGUI.LabelField(valuesRect, "Values");
		}

		private float ElementHeightCallback(int index)
		{
			SerializedProperty dictionary = cache.DictionaryProperty;
			SerializedProperty keyProperty = dictionary.FindPropertyRelative(KeysName).GetArrayElementAtIndex(index);
			SerializedProperty valueProperty = dictionary.FindPropertyRelative(ValuesName).GetArrayElementAtIndex(index);
			float keyHeight = EditorGUI.GetPropertyHeight(keyProperty);
			float valueHeight = EditorGUI.GetPropertyHeight(valueProperty);
			return Mathf.Max(keyHeight, valueHeight) + ElementMargin;
		}

		private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty dictionary = cache.DictionaryProperty;
			SerializedProperty keyProperty = dictionary.FindPropertyRelative(KeysName).GetArrayElementAtIndex(index);
			SerializedProperty valueProperty = dictionary.FindPropertyRelative(ValuesName).GetArrayElementAtIndex(index);

			GUI.enabled = false;
			Vector2 keyPosition = new Vector2(rect.x, rect.y + ElementMargin * 0.5f);
			Vector2 keySize = new Vector2(EditorGUIUtility.labelWidth - ElementSpacerWidth, EditorGUI.GetPropertyHeight(keyProperty));
			Rect keyRect = new Rect(keyPosition, keySize);
			EditorGUI.PropertyField(keyRect, keyProperty, GUIContent.none);
			GUI.enabled = true;
			
			Vector2 spacerPosition = new Vector2(keyRect.xMax, rect.y);
			Vector2 spacerSize = new Vector2(ElementSpacerWidth, rect.height);
			Rect spacerRect = new Rect(spacerPosition, spacerSize);
			
			Vector2 valuePosition = new Vector2(spacerRect.xMax, rect.y + ElementMargin * 0.5f);
			Vector2 valueSize = new Vector2(rect.width - spacerRect.width - keyRect.width, EditorGUI.GetPropertyHeight(valueProperty));
			Rect valueRect = new Rect(valuePosition, valueSize);
			EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);
		}
		
		private void ReorderCallback(ReorderableList list, int oldIndex, int newIndex)
		{
			SerializedProperty dictionary = cache.DictionaryProperty;
			
			SerializedProperty keysProperty = dictionary.FindPropertyRelative(KeysName);
			keysProperty.MoveArrayElement(oldIndex, newIndex);

			SerializedProperty valuesProperty = dictionary.FindPropertyRelative(ValuesName);
			valuesProperty.MoveArrayElement(oldIndex, newIndex);
		}
		
		private void AddDropdownCallback(Rect buttonRect, ReorderableList list)
		{
			SerializedProperty dictionary = cache.DictionaryProperty;

			SerializedProperty keysProperty = dictionary.FindPropertyRelative(KeysName);
			SerializedProperty keyHelper = dictionary.FindPropertyRelative(NewKeyHelperName);

			SerializedProperty valuesProperty = dictionary.FindPropertyRelative(ValuesName);
			SerializedProperty valueHelper = dictionary.FindPropertyRelative(NewValueHelperName);

			buttonRect.position = GUIUtility.GUIToScreenPoint(buttonRect.position);
			
			SerializableDictionaryNewElementWindow.Show(
				buttonRect,
				keyHelper,
				valueHelper,
				dictionary,
				(key, value ) =>
				{
					keysProperty.InsertArrayElementAtIndex(keysProperty.arraySize);
					SerializedProperty newKey = keysProperty.GetArrayElementAtIndex(keysProperty.arraySize - 1);
					newKey.SetPropertyValue(key.GetPropertyValue());
					
					valuesProperty.InsertArrayElementAtIndex(valuesProperty.arraySize);
					SerializedProperty newValue = valuesProperty.GetArrayElementAtIndex(valuesProperty.arraySize - 1);
					newValue.SetPropertyValue(value.GetPropertyValue());

					cache.DictionaryProperty.serializedObject.ApplyModifiedProperties();
					cache.DictionaryProperty.serializedObject.Update();
				});
		}
		
		private void RemoveCallback(ReorderableList list)
		{
			SerializedProperty dictionary = cache.DictionaryProperty;

			SerializedProperty keysProperty = dictionary.FindPropertyRelative(KeysName);
			keysProperty.DeleteArrayElementAtIndex(list.index);
			
			SerializedProperty valuesProperty = dictionary.FindPropertyRelative(ValuesName);
			valuesProperty.DeleteArrayElementAtIndex(list.index);
		}
	}
}