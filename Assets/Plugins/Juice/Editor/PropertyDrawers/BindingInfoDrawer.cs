using System;
using System.Collections.Generic;
using Juice.Utils;
using UnityEditor;
using UnityEngine;

namespace Juice.Editor
{
	[CustomPropertyDrawer(typeof(BindingInfo))]
	public class BindingInfoDrawer : PropertyDrawer
	{
		private class BindingEntry
		{
			public readonly int Index;
			public readonly ViewModelComponent Component;
			public readonly string PropertyName;
			public readonly Type ObservableType;
			public readonly Type ArgumentType;
			public readonly bool NeedsToBeBoxed;

			public BindingEntry(
				int index,
				ViewModelComponent component,
				string propertyName,
				Type observableType,
				Type argumentType,
				bool needsToBeBoxed)
			{
				Index = index;
				Component = component;
				PropertyName = propertyName;
				ObservableType = observableType;
				ArgumentType = argumentType;
				NeedsToBeBoxed = needsToBeBoxed;
			}
		}

		private class DrawerCache
		{
			public Transform LastParent;
			public Component BaseComponent;
			public Dictionary<string, BindingEntry> BindingMap;
			public string[] CachedOptionIds;
			public string[] CachedOptions;
			public int CurrentIndex;
		}

		private const string ViewModelContainerId = "viewModelContainer";
		private const string PropertyNameId = "propertyName";
		private const string ForceDynamicBindingId = "forceDynamicBinding";

		private static readonly float MoreMenuWidth = EditorGUIUtility.singleLineHeight;
		private static readonly float DynamicBindingIndicatorWidth = EditorGUIUtility.singleLineHeight;

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

			if (IsMouseHovering(position))
			{
				position = DrawMoreMenuButton(position, property);
			}

			if (cache.CachedOptionIds.Length <= 1 || GetForceDynamicBinding(property))
			{
				DrawPropertyNameField(position, property);
			}
			else
			{
				DrawPicker(position, property);
			}

			EditorGUI.EndDisabledGroup();
		}

		private static Rect DrawLabel(Rect position, GUIContent label)
		{
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			return position;
		}

		private static bool IsMouseHovering(Rect position)
		{
			return position.Contains(Event.current.mousePosition);
		}

		private static Rect DrawMoreMenuButton(Rect position, SerializedProperty property)
		{
			Rect moreButtonRect = new Rect(
				position.xMax - MoreMenuWidth,
				position.y,
				MoreMenuWidth,
				position.height);

			if (EditorGUI.DropdownButton(moreButtonRect, EditorGUIUtility.IconContent("_Menu"), FocusType.Passive, "iconButton"))
			{
				var menu = new GenericMenu();

				menu.AddItem(
					new GUIContent("Force dynamic binding", JuiceEditorGuiUtility.Icons.Unlink),
					GetForceDynamicBinding(property),
					() =>
					{
						bool currentValue = GetForceDynamicBinding(property);
						SetForceDynamicBinding(property, !currentValue);
						property.serializedObject.ApplyModifiedProperties();
					});

				menu.DropDown(moreButtonRect);
			}

			position.width -= moreButtonRect.width + EditorGUIUtility.standardVerticalSpacing;
			return position;
		}

		private static void SetBinding(SerializedProperty property, ViewModelComponent viewModelComponent, string propertyName)
		{
			SetViewModelContainer(property, viewModelComponent);
			SetPropertyName(property, propertyName);
		}

		private static ViewModelComponent GetViewModelContainer(SerializedProperty bindingInfoProperty)
		{
			return bindingInfoProperty.FindPropertyRelative(ViewModelContainerId).objectReferenceValue as ViewModelComponent;
		}

		private static void SetViewModelContainer(SerializedProperty bindingInfoProperty, ViewModelComponent value)
		{
			bindingInfoProperty.FindPropertyRelative(ViewModelContainerId).objectReferenceValue = value;
		}

		private static string GetPropertyName(SerializedProperty bindingInfoProperty)
		{
			return bindingInfoProperty.FindPropertyRelative(PropertyNameId).stringValue;
		}

		private static void SetPropertyName(SerializedProperty bindingInfoProperty, string value)
		{
			bindingInfoProperty.FindPropertyRelative(PropertyNameId).stringValue = value;
		}

		private static bool GetForceDynamicBinding(SerializedProperty bindingInfoProperty)
		{
			return bindingInfoProperty.FindPropertyRelative(ForceDynamicBindingId).boolValue;
		}

		private static void SetForceDynamicBinding(SerializedProperty bindingInfoProperty, bool value)
		{
			bindingInfoProperty.FindPropertyRelative(ForceDynamicBindingId).boolValue = value;
		}

		private static string GenerateBindingId(ViewModelComponent component, string propertyName)
		{
			return $"{component.gameObject.name}.{component.Id}/{propertyName}";
		}

		private static string GenerateOptionString(string bindingId, BindingEntry entry)
		{
			string argument = entry.ArgumentType != null ? $" ({entry.ArgumentType.GetPrettifiedName()})" : string.Empty;
			string boxingIcon = entry.NeedsToBeBoxed ? " ⚠" : string.Empty;
			return $"{bindingId}{argument}{boxingIcon}";
		}

		private void SetupCache(SerializedProperty property)
		{
			string cacheId = $"{property.serializedObject.targetObject.GetInstanceID().ToString()}{property.propertyPath}";

			CacheMap.TryGetValue(cacheId, out cache);

			if (cache != null && (cache.BaseComponent || cache.BaseComponent.transform.parent != cache.LastParent))
			{
				cache = null;
			}

			if (cache == null)
			{
				cache = new DrawerCache();
				CacheBaseComponent(property, cache);
				CacheBindingCollections(property, cache);
				CacheMap[cacheId] = cache;
			}
		}

		private void CacheBaseComponent(SerializedProperty property, DrawerCache cache)
		{
			cache.BaseComponent = property.serializedObject.targetObject as Component;

			if (cache.BaseComponent)
			{
				cache.LastParent = cache.BaseComponent.transform.parent;
			}
		}

		private void CacheBindingCollections(SerializedProperty property, DrawerCache cache)
		{
			cache.BindingMap = new Dictionary<string, BindingEntry>();
			List<string> optionIds = new List<string>();
			List<string> options = new List<string>();
			optionIds.Add("None");
			options.Add("None");

			foreach (Juice.BindingEntry current in BindingUtils.GetBindings(cache.BaseComponent.transform, ResolveTarget(property).Type))
			{
				if (current.ViewModelComponent != cache.BaseComponent)
				{
					string id = GenerateBindingId(current.ViewModelComponent, current.PropertyName);
					BindingEntry entry = new BindingEntry(
						optionIds.Count,
						current.ViewModelComponent,
						current.PropertyName,
						current.ObservableType,
						current.GenericArgument,
						current.NeedsToBeBoxed);
					cache.BindingMap.Add(id, entry);
					optionIds.Add(id);
					options.Add(GenerateOptionString(id, entry));
				}
			}

			cache.CachedOptionIds = optionIds.ToArray();
			cache.CachedOptions = options.ToArray();
		}

		private BindingInfo ResolveTarget(SerializedProperty property)
		{
			return PropertyDrawerUtility.GetActualObjectForSerializedProperty<BindingInfo>(fieldInfo, property);
		}

		private void RefreshCurrentIndex(SerializedProperty property)
		{
			if (GetViewModelContainer(property) && string.IsNullOrEmpty(GetPropertyName(property)) == false)
			{
				string bindingId = GenerateBindingId(GetViewModelContainer(property), GetPropertyName(property));

				if (cache.BindingMap.TryGetValue(bindingId, out BindingEntry entry))
				{
					cache.CurrentIndex = entry.Index;
				}
			}
			else if (cache.BindingMap.Count > 0)
			{
				SetBinding(property, null, GetPropertyName(property));
				cache.CurrentIndex = 0;
			}
		}

		private void DrawPicker(Rect position, SerializedProperty property)
		{
			GUIContent content = new GUIContent(cache.CachedOptions[cache.CurrentIndex]);

			if (EditorGUI.DropdownButton(position, content, FocusType.Passive))
			{
				EditorUtility.DisplayCustomMenu(
					position,
					EditorGUIUtility.TrTempContent(cache.CachedOptions),
					cache.CurrentIndex,
					(data, options, selected) =>
					{
						if (selected != cache.CurrentIndex)
						{
							if (selected > 0)
							{
								string bindingId = cache.CachedOptionIds[selected];
								BindingEntry entry = cache.BindingMap[bindingId];
								SetBinding(property, entry.Component, entry.PropertyName);
							}
							else
							{
								SetBinding(property, null, "");
							}

							property.serializedObject.ApplyModifiedProperties();
						}
					},
					null);
			}
		}

		private static void DrawPropertyNameField(Rect position, SerializedProperty property)
		{
			Rect indicatorRect = new Rect(position.x, position.y, DynamicBindingIndicatorWidth, position.height);

			Rect iconRect = indicatorRect;
			iconRect.x += 3;
			iconRect.y += 3;
			iconRect.width -= 6;
			iconRect.height -= 6;

			EditorGUI.LabelField(iconRect, JuiceEditorGuiUtility.ContentIcons.Unlink);

			position.x += indicatorRect.width;
			position.width -= indicatorRect.width;

			EditorGUI.PropertyField(position, property.FindPropertyRelative(PropertyNameId), GUIContent.none);
		}
	}
}
