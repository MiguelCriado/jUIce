using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Juice.Utils;
using UnityEditor;
using UnityEngine;

namespace Juice.Editor
{
	[CustomPropertyDrawer(typeof(SerializableType))]
	public class SerializableTypeDrawer : PropertyDrawer
	{
		private static readonly string TypeReferenceFieldName = "typeReference";
		private static readonly string NoneSelectionEntry = "None";
		private static readonly Dictionary<string, SerializableTypeDrawerCache> CacheMap = new Dictionary<string, SerializableTypeDrawerCache>();

		~SerializableTypeDrawer()
		{
			CacheMap.Clear();
		}

		private SerializableTypeDrawerCache cache;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			SetupCache(property);
			RefreshCurrentSelection(property);

			position = DrawLabel(position, label);

			DrawPopup(position, property);

			EditorGUI.EndProperty();
		}

		private static Rect DrawLabel(Rect position, GUIContent label)
		{
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			return position;
		}

		private void DrawPopup(Rect position, SerializedProperty property)
		{
			if (GUI.Button(position, cache.CurrentSelection, EditorStyles.popup))
			{
				SerializableTypeEditorWindow.Show(
					position,
					cache.TypeMapByType.Values.Prepend("None"),
					(type) => OnTypeSelected(property, type));
			}
		}

		private void SetupCache(SerializedProperty property)
		{
			string id = $"{property.serializedObject.targetObject.GetInstanceID().ToString()}{property.propertyPath}";

			if (CacheMap.TryGetValue(id, out cache) == false)
			{
				var typeConstraint = fieldInfo.GetCustomAttribute<TypeConstraintAttribute>();
				cache = new SerializableTypeDrawerCache(typeConstraint);

				CacheMap[id] = cache;
			}
		}

		private static Type GetType(SerializedProperty property)
		{
			Type result = null;
			string stringValue = property.FindPropertyRelative(TypeReferenceFieldName).stringValue;

			if (string.IsNullOrEmpty(stringValue) == false)
			{
				result = Type.GetType(stringValue);
			}

			return result;
		}

		private static void SetType(SerializedProperty property, Type type)
		{
			string stringValue = type != null ? type.AssemblyQualifiedName : string.Empty;
			property.FindPropertyRelative(TypeReferenceFieldName).stringValue = stringValue;
		}

		private void RefreshCurrentSelection(SerializedProperty property)
		{
			Type currentType = GetType(property);

			if (currentType != null && cache.TypeMapByType.TryGetValue(currentType, out string entry))
			{
				cache.CurrentSelection = entry;
			}
			else
			{
				cache.CurrentSelection = NoneSelectionEntry;
			}
		}

		private void OnTypeSelected(SerializedProperty property, string typeId)
		{
			if (cache.TypeMapByString.TryGetValue(typeId, out Type selectedType))
			{
				SetType(property, selectedType);
			}
			else if (typeId == NoneSelectionEntry)
			{
				SetType(property, null);
			}

			property.serializedObject.ApplyModifiedProperties();
		}
	}
}
