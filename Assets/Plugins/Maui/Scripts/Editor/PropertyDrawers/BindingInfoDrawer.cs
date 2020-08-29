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
			public readonly int Index;
			public readonly ViewModelComponent Component;
			public readonly string PropertyName;
			public readonly bool NeedsToBeBoxed;

			public BindingEntry(
				int index,
				ViewModelComponent component,
				string propertyName,
				bool needsToBeBoxed)
			{
				Index = index;
				Component = component;
				PropertyName = propertyName;
				NeedsToBeBoxed = needsToBeBoxed;
			}
		}

		private class DrawerCache
		{
			public BindingInfo Target;
			public Transform LastParent;
			public Component BaseComponent;
			public Dictionary<string, BindingEntry> BindingMap;
			public string[] CachedOptions;
			public int CurrentIndex;
		}

		private const string ViewModelContainerId = "viewModelContainer";
		private const string PropertyNameId = "propertyName";
		
		private static readonly Dictionary<string, DrawerCache> CacheMap = new Dictionary<string,DrawerCache>();

		private DrawerCache cache;
		
		public BindingInfoDrawer()
		{
			BindingInfoTracker.Register(this);
		}

		~BindingInfoDrawer()
		{
			BindingInfoTracker.Unregister(this);
			CacheMap.Clear();
		}
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property); 
			
			SetupCache(property);
			RefreshCurrentIndex(property);
			
			position = DrawLabel(position, label);

			DrawContent(position, property);

			EditorGUI.EndProperty();
		}

		public void SetDirty()
		{
			CacheMap.Clear();
		}

		protected virtual void DrawContent(Rect position, SerializedProperty property)
		{
			DrawBindingInfo(position, property);
		}

		protected void DrawBindingInfo(Rect position, SerializedProperty property)
		{
			EditorGUI.BeginDisabledGroup(Application.isPlaying);

			if (cache.CachedOptions.Length > 0)
			{
				DrawPicker(position, property);
			}
			else
			{
				DrawPropertyNameField(position, property);
			}

			EditorGUI.EndDisabledGroup();
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
		
		private static string GenerateBindingId(ViewModelComponent component, string propertyName)
		{
			return $"{component.gameObject.name}.{component.Id}/{propertyName}";
		}

		private void SetupCache(SerializedProperty property)
		{
			string id = $"{property.serializedObject.targetObject.GetInstanceID().ToString()}{property.propertyPath}";

			CacheMap.TryGetValue(id, out cache);
			
			if (cache != null && (cache.BaseComponent == null || cache.BaseComponent.transform.parent != cache.LastParent))
			{
				cache = null;
			}

			if (cache == null)
			{
				cache = new DrawerCache();
				CacheTarget(property, cache);
				CacheBaseComponent(property, cache);
				CacheBindingCollections(cache);
				CacheMap[id] = cache;
			}
		}

		private void CacheTarget(SerializedProperty property, DrawerCache cache)
		{
			cache.Target = PropertyDrawerUtility.GetActualObjectForSerializedProperty<BindingInfo>(fieldInfo, property);
		}

		private void CacheBaseComponent(SerializedProperty property, DrawerCache cache)
		{
			cache.BaseComponent = property.serializedObject.targetObject as Component;
			cache.LastParent = cache.BaseComponent.transform.parent;
		}

		private void CacheBindingCollections(DrawerCache cache)
		{
			cache.BindingMap = new Dictionary<string, BindingEntry>();
			List<string> options = new List<string>();

			foreach (Maui.BindingEntry current in BindingUtils.GetBindings(cache.BaseComponent.transform, cache.Target.Type))
			{
				string id = GenerateBindingId(current.ViewModelComponent, current.PropertyName);
				BindingEntry entry = new BindingEntry(options.Count, current.ViewModelComponent, current.PropertyName, current.NeedsToBeBoxed);
				cache.BindingMap.Add(id, entry);
				options.Add(id);
			}
			
			cache.CachedOptions = options.ToArray();
		}

		private void RefreshCurrentIndex(SerializedProperty property)
		{
			if (cache.Target.ViewModelContainer != null && string.IsNullOrEmpty(cache.Target.PropertyName) == false)
			{
				string bindingId = GenerateBindingId(cache.Target.ViewModelContainer, cache.Target.PropertyName);
				
				if (cache.BindingMap.TryGetValue(bindingId, out BindingEntry entry))
				{
					cache.CurrentIndex = entry.Index;
				}
			}
			else if (cache.BindingMap.Count > 0)
			{
				BindingEntry firstOption = cache.BindingMap[cache.CachedOptions[0]];
				SetBinding(property, firstOption.Component, firstOption.PropertyName);
				cache.CurrentIndex = 0;
			}
		}

		private void DrawPicker(Rect position, SerializedProperty property)
		{
			int index = EditorGUI.Popup(position, cache.CurrentIndex, cache.CachedOptions);

			if (index != cache.CurrentIndex)
			{
				string bindingId = cache.CachedOptions[index];
				BindingEntry entry = cache.BindingMap[bindingId];
				SetBinding(property, entry.Component, entry.PropertyName);

				cache.CurrentIndex = index;
			}
		}

		private static void DrawPropertyNameField(Rect position, SerializedProperty property)
		{
			EditorGUI.PropertyField(position, property.FindPropertyRelative(PropertyNameId), GUIContent.none);
		}
	}
}