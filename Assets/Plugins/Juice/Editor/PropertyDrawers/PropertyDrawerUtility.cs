using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Juice.Editor
{
	public class PropertyDrawerUtility
	{
		public static T GetActualObjectForSerializedProperty<T>(FieldInfo fieldInfo, SerializedProperty property) where T : class
		{
			T result = null;

			var targetObject = ResolveTargetObject(property);
			var field = fieldInfo.GetValue(targetObject);

			if (field != null)
			{
				if (field.GetType().IsArray)
				{
					var index = GetElementIndex<T>(property);
					result = ((T[])field)[index];
				}
				else if (field is List<T> list)
				{
					var index = GetElementIndex<T>(property);
					result = list[index];
				}
				else
				{
					result = field as T;
				}
			}

			return result;
		}

		public static PropertyDrawer GetCustomPropertyDrawer(FieldInfo fieldInfo)
		{
			// Getting the field type this way assumes that the property instance is not a managed reference (with a SerializeReference attribute); if it was, it should be retrieved in a different way:
			Type fieldType = fieldInfo.FieldType;

			Type propertyDrawerType = (Type) Type.GetType("UnityEditor.ScriptAttributeUtility,UnityEditor")
				.GetMethod("GetDrawerTypeForType", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
				.Invoke(null, new object[] {fieldType});

			PropertyDrawer propertyDrawer = null;

			if (typeof(PropertyDrawer).IsAssignableFrom(propertyDrawerType))
			{
				propertyDrawer = (PropertyDrawer) Activator.CreateInstance(propertyDrawerType);
			}

			if (propertyDrawer != null)
			{
				typeof(PropertyDrawer)
					.GetField("m_FieldInfo", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					.SetValue(propertyDrawer, fieldInfo);
			}

			return propertyDrawer;
		}

		private static int GetElementIndex<T>(SerializedProperty property) where T : class
		{
			var index = Convert.ToInt32(new string(property.propertyPath.Where(c => char.IsDigit(c)).ToArray()));
			return index;
		}

		private static object ResolveTargetObject(SerializedProperty property)
		{
			object targetObject = property.serializedObject.targetObject;
			var path = property.propertyPath.Replace(".Array.data[", "[");
			string[] tokens = path.Split('.');

			for (int i = 0; i < tokens.Length - 1; i++)
			{
				string current = tokens[i];

				if (current.Contains("["))
				{
					var elementName = current.Substring(0, current.IndexOf("["));
					var index = Convert.ToInt32(
						current.Substring(current.IndexOf("["))
							.Replace("[","")
							.Replace("]",""));
					
					targetObject = GetValue(targetObject, elementName, index);
				}
				else
				{
					targetObject = GetValue(targetObject, current);
				}
			}

			return targetObject;
		}

		private static object GetValue(object source, string name, int index)
		{
			var enumerable = GetValue(source, name) as IEnumerable;
			var enumerator = enumerable.GetEnumerator();

			while (index-- >= 0)
			{
				enumerator.MoveNext();
			}

			return enumerator.Current;
		}
		
		private static object GetValue(object source, string name)
		{
			Type type = source.GetType();
			FieldInfo info = null;
			BindingFlags bindingFlags =
				BindingFlags.Public
				| BindingFlags.NonPublic
				| BindingFlags.Static
				| BindingFlags.Instance
				| BindingFlags.DeclaredOnly;

			while (info == null && type != null)
			{
				info = type.GetField(name, bindingFlags);
				type = type.BaseType;
			}

			return info.GetValue(source);
		}
	}
}
