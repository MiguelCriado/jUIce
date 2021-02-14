using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Juice.Editor
{
	[CustomPropertyDrawer(typeof(SerializableType))]
	public class SerializableTypeDrawer : PropertyDrawer
	{
		private class TypeEntry
		{
			public int Index;
			public Type Type;

			public TypeEntry(int index, Type type)
			{
				Index = index;
				Type = type;
			}
		}

		private class DrawerCache
		{
			public SerializableType Target;
			public Dictionary<string, TypeEntry> TypeMap;
			public string[] CachedOptions;
			public int CurrentIndex;
		}

		private static readonly string TypeReferenceFieldName = "typeReference";
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
			RefreshCurrentIndex(property);

			position = DrawLabel(position, label);

			// Don't make child fields be indented
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			int index = EditorGUI.Popup(position, cache.CurrentIndex, cache.CachedOptions);

			if (index != cache.CurrentIndex)
			{
				Undo.RecordObject(property.serializedObject.targetObject, $"{fieldInfo.Name} type changed");
				
				string selectedType = cache.CachedOptions[index];
				SetType(property, cache.TypeMap[selectedType].Type);

				cache.CurrentIndex = index;
			}

			// Set indent back to what it was
			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}

		private static Rect DrawLabel(Rect position, GUIContent label)
		{
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			return position;
		}

		private void SetupCache(SerializedProperty property)
		{
			string id = $"{property.serializedObject.targetObject.GetInstanceID().ToString()}{property.propertyPath}";
			
			if (CacheMap.TryGetValue(id, out cache) == false)
			{
				cache = new DrawerCache();
				CacheTarget(property, cache);
				CacheTypeCollections(cache);

				CacheMap[id] = cache;
			}
		}

		private void CacheTarget(SerializedProperty property, DrawerCache cache)
		{
			cache.Target = PropertyDrawerUtility.GetActualObjectForSerializedProperty<SerializableType>(fieldInfo, property);
		}

		private void CacheTypeCollections(DrawerCache cache)
		{
			TypeConstraintAttribute typeConstraint = fieldInfo.GetCustomAttribute<TypeConstraintAttribute>();
			cache.TypeMap = new Dictionary<string, TypeEntry>();
			List<string> options = new List<string>();

			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (IsMatchingType(type, typeConstraint))
					{
						string typeName = type.FullName;

						if (cache.TypeMap.ContainsKey(typeName) == false)
						{
							cache.TypeMap.Add(typeName, new TypeEntry(options.Count, type));
							options.Add(typeName);
						}
					}
				}
			}

			cache.CachedOptions = options.ToArray();
		}

		private static bool IsMatchingType(Type type, TypeConstraintAttribute constraint)
		{
			return  constraint == null 
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

		private void RefreshCurrentIndex(SerializedProperty property)
		{
			if (cache.Target.Type != null)
			{
				string typeName = cache.Target.Type.FullName;

				if (cache.TypeMap.TryGetValue(typeName, out TypeEntry entry))
				{
					cache.CurrentIndex = entry.Index;
				}
			}
			else if (cache.TypeMap.Count > 0)
			{
				TypeEntry firstEntry = cache.TypeMap[cache.CachedOptions[0]];
				SetType(property, firstEntry.Type);
			}
		}

		private void SetType(SerializedProperty property, Type type)
		{
			SerializedProperty typeName = property.FindPropertyRelative(TypeReferenceFieldName);
			cache.Target.Type = type;
			typeName.stringValue = type.AssemblyQualifiedName;
		}
	}
}
