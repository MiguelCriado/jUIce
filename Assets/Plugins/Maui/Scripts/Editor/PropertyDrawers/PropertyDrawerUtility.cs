using System;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Maui.Editor
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
					var index = Convert.ToInt32(new string(property.propertyPath.Where(c => char.IsDigit(c)).ToArray()));
					result = ((T[])field)[index];
				}
				else
				{
					result = field as T;
				}
			}

			return result;
		}

		private static object ResolveTargetObject(SerializedProperty property)
		{
			object targetObject = property.serializedObject.targetObject;
			string[] tokens = property.propertyPath.Split('.');

			for (int i = 0; i < tokens.Length - 1; i++)
			{
				Type type = targetObject.GetType();
				FieldInfo info = null;
				BindingFlags bindingFlags =
					BindingFlags.Public
					| BindingFlags.NonPublic
					| BindingFlags.Static
					| BindingFlags.Instance
					| BindingFlags.DeclaredOnly;

				while (info == null && type != null)
				{
					info = type.GetField(tokens[i], bindingFlags);
					type = type.BaseType;
				}

				targetObject = info.GetValue(targetObject);
			}

			return targetObject;
		}
	}
}
