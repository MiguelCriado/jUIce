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
		private class DrawerCache
		{
			public Dictionary<Type, string> TypeMapByType;
			public Dictionary<string, Type> TypeMapByString;
			public string CurrentSelection;
		}

		private static readonly string TypeReferenceFieldName = "typeReference";
		private static readonly string NoneSelectionEntry = "None";
		private static readonly Dictionary<string, DrawerCache> CacheMap = new Dictionary<string, DrawerCache>();

		~SerializableTypeDrawer()
		{
			CacheMap.Clear();
		}

		private DrawerCache cache;

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
				cache = new DrawerCache();
				CacheTypeCollections(cache);

				CacheMap[id] = cache;
			}
		}

		private void CacheTypeCollections(DrawerCache cache)
		{
			var typeConstraint = fieldInfo.GetCustomAttribute<TypeConstraintAttribute>();
			cache.TypeMapByType = new Dictionary<Type, string>();
			cache.TypeMapByString = new Dictionary<string, Type>();
			var matchingGroups = new Dictionary<string, List<Type>>();

			foreach (Type current in TypeCache.GetTypesDerivedFrom(typeConstraint.BaseType))
			{
				if (IsMatchingType(current, typeConstraint))
				{
					AddTypeToCache(cache, current, matchingGroups);
				}
			}
		}

		private static bool IsMatchingType(Type type, TypeConstraintAttribute constraint)
		{
			return constraint == null
			        || (DoesMatchInstantiableRestriction(type, constraint)
			            && IsAssignableFrom(constraint.BaseType, type));
		}

		private static bool DoesMatchInstantiableRestriction(Type type, TypeConstraintAttribute constraint)
		{
			return !constraint.ForceInstantiableType ||
			       (!type.IsAbstract && !type.IsInterface && !type.IsGenericTypeDefinition);
		}

		private static bool IsAssignableFrom(Type a, Type b)
		{
			return a.IsAssignableFrom(b) || IsAssignableToGenericType(b, a);
		}

		private static bool IsAssignableToGenericType(Type givenType, Type genericType)
		{
			var interfaceTypes = givenType.GetInterfaces();

			foreach (var it in interfaceTypes)
			{
				if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
				{
					return true;
				}
			}

			if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
			{
				return true;
			}

			Type baseType = givenType.BaseType;

			if (baseType == null)
			{
				return false;
			}

			return IsAssignableToGenericType(baseType, genericType);
		}

		private static void AddTypeToCache(DrawerCache cache, Type current, Dictionary<string, List<Type>> matchingGroups)
		{
			string typeId = current.GetPrettifiedName();

			if (matchingGroups.TryGetValue(typeId, out List<Type> group))
			{
				group.Add(current);
				FixMatchingTypeIds(cache, group);
			}
			else
			{
				var newGroup = new List<Type> {current};
				matchingGroups.Add(typeId, newGroup);
				AddTypeToCache(current, typeId, cache);
			}
		}

		private static void FixMatchingTypeIds(DrawerCache cache, List<Type> group)
		{
			for (int i = 0; i < group.Count; i++)
			{
				Type target = group[i];

				for (int j = i + 1; j < group.Count; j++)
				{
					Type other = group[j];

					string targetId = GetDistinctName(target, other);
					AddTypeToCache(target, targetId, cache);

					string otherId = GetDistinctName(other, target);
					AddTypeToCache(other, otherId, cache);
				}
			}
		}

		private static string GetDistinctName(Type target, Type other)
		{
			string result = target?.GetPrettifiedName();

			if (target != null && other != null && result == other.GetPrettifiedName())
			{
				string[] targetPath = target.FullName?.Split('.');
				string[] otherPath = other.FullName?.Split('.');

				if (targetPath != null && otherPath != null)
				{
					int targetIndex = targetPath.Length - 2;
					int otherIndex = otherPath.Length - 2;

					while (targetIndex >= 0 && otherIndex >= 0 && targetPath[targetIndex] == otherPath[otherIndex])
					{
						result = $"{targetPath[targetIndex]}.{result}";
						targetIndex--;
						otherIndex--;
					}

					if (targetIndex < 0)
					{
						result = $"{result} ({target.Assembly.GetName().Name})";
					}
					else
					{
						result = $"{targetPath[targetIndex]}.{result}";
					}
				}
			}

			return result;
		}

		private static void AddTypeToCache(Type type, string newId, DrawerCache cache)
		{
			if (cache.TypeMapByType.TryGetValue(type, out string currentId) == false
			    || newId.Length > currentId.Length)
			{
				cache.TypeMapByType[type] = newId;
				cache.TypeMapByString[newId] = type;
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
