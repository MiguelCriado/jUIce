using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Maui.Editor
{
	[CustomPropertyDrawer(typeof(BindingInfoList))]
	public class BindingInfoListDrawer : PropertyDrawer
	{
		private class DrawerCache
		{
			public BindingInfoList Target;
			public ReorderableList ReorderableList;
		}
		
		private static readonly float ElementMargin = EditorGUIUtility.standardVerticalSpacing;
		
		private static readonly Dictionary<string, DrawerCache> CacheMap = new Dictionary<string,DrawerCache>();

		private DrawerCache cache;
		
		~BindingInfoListDrawer()
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
			SerializedProperty listProperty = property.FindPropertyRelative("bindingInfoList");
			
			if (CacheMap.TryGetValue(id, out cache))
			{
				cache.ReorderableList.serializedProperty = listProperty;
			}
			else
			{
				cache = new DrawerCache();
				cache.Target = property.GetValue<BindingInfoList>();
				cache.ReorderableList = CreateList(listProperty, cache.Target);
				CacheMap[id] = cache;
			}
		}
		
		private ReorderableList CreateList(SerializedProperty property, BindingInfoList target)
		{
			var result = new ReorderableList(property.serializedObject, property);
			result.drawHeaderCallback += DrawHeaderCallback;
			result.elementHeightCallback += index => ElementHeightCallback(result, index);
			result.drawElementCallback += (rect, index, isActive, isFocused) => DrawElementCallback(result, rect, index);
			result.onAddCallback += list => OnAddCallback(target);
			return result;
		}

		private void DrawHeaderCallback(Rect rect)
		{
			EditorGUI.LabelField(rect, "Binding Info List");
		}

		private float ElementHeightCallback(ReorderableList list, int index)
		{ 
			SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
			return EditorGUI.GetPropertyHeight(element) + ElementMargin;
		}

		private void DrawElementCallback(ReorderableList list, Rect rect, int index)
		{
			SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += ElementMargin * 0.5f;
			rect.height -= ElementMargin * 0.5f;
			
			EditorGUI.PropertyField(rect, element, GUIContent.none);
		}
		
		private void OnAddCallback(BindingInfoList target)
		{
			target.AddElement();
		}
	}
}