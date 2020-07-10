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
			var obj = fieldInfo.GetValue(property.serializedObject.targetObject);

			if (obj != null)
			{
				if (obj.GetType().IsArray)
				{
					var index = Convert.ToInt32(new string(property.propertyPath.Where(c => char.IsDigit(c)).ToArray()));
					result = ((T[])obj)[index];
				}
				else
				{
					result = obj as T;
				}
			}

			return result;
		}
	}
}
