using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Maui.Editor
{
	[CustomPropertyDrawer(typeof(BindingInfo))]
	public class BindingInfoDrawer : PropertyDrawer
	{
		private class BindingEntry
		{
			public int Index;
			public ViewModelComponent Component;
			public string PropertyName;

			public BindingEntry(int index, ViewModelComponent component, string propertyName)
			{
				Index = index;
				Component = component;
				PropertyName = propertyName;
			}
		}

		private const string ViewModelContainerId = "viewModelContainer";
		private const string PropertyNameId = "propertyName";

		private bool isCached;
		private BindingInfo target;
		private Transform lastParent;
		private Component baseComponent;
		private Dictionary<string, BindingEntry> bindingMap;
		private string[] cachedOptions;
		private int currentIndex;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			CacheElements(property);
			RefreshCurrentIndex(property);

			position = DrawLabel(position, label);
			
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			EditorGUI.BeginDisabledGroup(Application.isPlaying);
			
			if (cachedOptions.Length > 0)
			{
				DrawPicker(position, property);
			}
			else
			{
				DrawPropertyNameField(position, property);
			}
			
			EditorGUI.EndDisabledGroup();

			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}

		private static Rect DrawLabel(Rect position, GUIContent label)
		{
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			return position;
		}

		private static void SetBinding(SerializedProperty property, ViewModelComponent viewModelComponent, string propertyName)
		{
			property.FindPropertyRelative(ViewModelContainerId).objectReferenceValue = viewModelComponent;
			property.FindPropertyRelative(PropertyNameId).stringValue = propertyName;
		}

		private void CacheElements(SerializedProperty property)
		{
			if (baseComponent == null || baseComponent.transform.parent != lastParent)
			{
				isCached = false;
			}

			if (isCached == false)
			{
				CacheTarget(property);
				CacheBaseComponent(property);
				CacheBindingCollections();
				isCached = true;
			}
		}

		private void CacheTarget(SerializedProperty property)
		{
			target = PropertyDrawerUtility.GetActualObjectForSerializedProperty<BindingInfo>(fieldInfo, property);
		}

		private void CacheBaseComponent(SerializedProperty property)
		{
			baseComponent = property.serializedObject.targetObject as Component;
			lastParent = baseComponent.transform.parent;
		}

		private void CacheBindingCollections()
		{
			bindingMap = new Dictionary<string, BindingEntry>();
			List<string> options = new List<string>();

			foreach (Maui.BindingEntry entry in BindingUtils.GetBindings(baseComponent.transform, target.Type))
			{
				string id = $"{entry.ViewModelComponent.gameObject.name}/{entry.PropertyName}";
				bindingMap.Add(id, new BindingEntry(options.Count, entry.ViewModelComponent, entry.PropertyName));
				options.Add(id);
			}
			
			cachedOptions = options.ToArray();
		}

		private void RefreshCurrentIndex(SerializedProperty property)
		{
			if (target.ViewModelContainer != null && string.IsNullOrEmpty(target.PropertyName) == false)
			{
				string bindName = $"{target.ViewModelContainer.gameObject.name}/{target.PropertyName}";
				
				if (bindingMap.TryGetValue(bindName, out BindingEntry entry))
				{
					currentIndex = entry.Index;
				}
			}
			else if (bindingMap.Count > 0)
			{
				BindingEntry firstOption = bindingMap[cachedOptions[0]];
				SetBinding(property, firstOption.Component, firstOption.PropertyName);
				currentIndex = 0;
			}
		}

		private void DrawPicker(Rect position, SerializedProperty property)
		{
			int index = EditorGUI.Popup(position, currentIndex, cachedOptions);

			if (index != currentIndex)
			{
				string bindingId = cachedOptions[index];
				BindingEntry entry = bindingMap[bindingId];
				SetBinding(property, entry.Component, entry.PropertyName);

				currentIndex = index;
			}
		}

		private static void DrawPropertyNameField(Rect position, SerializedProperty property)
		{
			EditorGUI.PropertyField(position, property.FindPropertyRelative(PropertyNameId), GUIContent.none);
		}
	}
}