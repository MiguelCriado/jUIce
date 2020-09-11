 using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Reflection;
 using UnityEditor;
 using UnityEngine;

 namespace Maui.Editor 
 {
	 /// <summary>
	 /// Extension class for SerializedProperties
	 /// See also: http://answers.unity3d.com/questions/627090/convert-serializedproperty-to-custom-class.html
	 /// </summary>
	 public static class SerializedPropertyExtensions
	 {
		 private static readonly Dictionary<SerializedPropertyType, PropertyInfo> PropertyAccessorsMap;
		 
		 static SerializedPropertyExtensions()
		 {
			 Dictionary<SerializedPropertyType, string> serializedPropertyValueAccessorsNameDict = new Dictionary<SerializedPropertyType, string>() {
				{ SerializedPropertyType.Integer, "intValue" },
				{ SerializedPropertyType.Boolean, "boolValue" },
				{ SerializedPropertyType.Float, "floatValue" },
				{ SerializedPropertyType.String, "stringValue" },
				{ SerializedPropertyType.Color, "colorValue" },
				{ SerializedPropertyType.ObjectReference, "objectReferenceValue" },
				{ SerializedPropertyType.LayerMask, "intValue" },
				{ SerializedPropertyType.Enum, "intValue" },
				{ SerializedPropertyType.Vector2, "vector2Value" },
				{ SerializedPropertyType.Vector3, "vector3Value" },
				{ SerializedPropertyType.Vector4, "vector4Value" },
				{ SerializedPropertyType.Rect, "rectValue" },
				{ SerializedPropertyType.ArraySize, "intValue" },
				{ SerializedPropertyType.Character, "intValue" },
				{ SerializedPropertyType.AnimationCurve, "animationCurveValue" },
				{ SerializedPropertyType.Bounds, "boundsValue" },
				{ SerializedPropertyType.Quaternion, "quaternionValue" },
			 };
			 
			 Type serializedPropertyType = typeof(SerializedProperty);

			 PropertyAccessorsMap = new Dictionary<SerializedPropertyType, PropertyInfo>();
			 BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

			 foreach(var kvp in serializedPropertyValueAccessorsNameDict)
			 {
				PropertyInfo propertyInfo = serializedPropertyType.GetProperty(kvp.Value, flags);
				PropertyAccessorsMap.Add(kvp.Key, propertyInfo);
			 }
		 }

		 public static object GetPropertyValue(this SerializedProperty property)
		 {
			object result;
			
			if (PropertyAccessorsMap.TryGetValue(property.propertyType, out PropertyInfo propertyInfo))
			{
				result = propertyInfo.GetValue(property);
			}
			else if (property.isArray)
			{
				result = property.GetPropertyValueArray();
			}
			else
			{
				result = property.GetPropertyValueGeneric();
			}

			return result;
		 }

		 public static object[] GetPropertyValueArray(this SerializedProperty property)
		 {
			 object[] result = new object[property.arraySize];
			 
			 for(int i = 0; i < property.arraySize; i++)
			 {
				 SerializedProperty item = property.GetArrayElementAtIndex(i);
				 result[i] = GetPropertyValue(item);
			 }
			 
			 return result;
		 }

		 public static object GetPropertyValueGeneric(this SerializedProperty property)
		 {
			 Dictionary<string, object> result = new Dictionary<string, object>();
			 var iterator = property.Copy();
			 
			 if (iterator.Next(true))
			 {
				 var end = property.GetEndProperty();
				 
				 do
				 {
					 string name = iterator.name;
					 object value = GetPropertyValue(iterator);
					 result.Add(name, value);
				 } 
				 while(iterator.Next(false) && iterator.propertyPath != end.propertyPath);
			 }
			 
			 return result;
		 }

		 public static void SetPropertyValue(this SerializedProperty property, object value)
		 {
			 if (PropertyAccessorsMap.TryGetValue(property.propertyType, out PropertyInfo propertyInfo))
			 {
				 propertyInfo.SetValue(property, value);
			 }
			 else if (property.isArray)
			 {
				 property.SetPropertyValueArray(value);
			 }
			 else
			 {
				 property.SetPropertyValueGeneric(value);
			 }
		 }

		 public static void SetPropertyValueArray(this SerializedProperty property, object value)
		 {
			 object[] array = (object[])value;
			 property.arraySize = array.Length;
			 
			 for(int i = 0; i < property.arraySize; i++)
			 {
				 SerializedProperty item = property.GetArrayElementAtIndex(i);
				 item.SetPropertyValue(array[i]);
			 }
		 }

		 public static void SetPropertyValueGeneric(this SerializedProperty property, object value)
		 {
			 Dictionary<string, object> dict = (Dictionary<string, object>)value;
			 var iterator = property.Copy();
			 
			 if (iterator.Next(true))
			 {
				 var end = property.GetEndProperty();
				 
				 do
				 {
					 string name = iterator.name;
					 SetPropertyValue(iterator, dict[name]);
				 }
				 while(iterator.Next(false) && iterator.propertyPath != end.propertyPath);
			 }
		 }

		 /// <summary>
		 /// Get the object the serialized serializedProperty holds by using reflection
		 /// </summary>
		 /// <typeparam name="T">The object type that the serializedProperty contains</typeparam>
		 /// <param name="property"></param>
		 /// <returns>Returns the object type T if it is the type the serializedProperty actually contains</returns>
		 public static T GetValue<T>(this SerializedProperty property)
		 {
			 return GetNestedObject<T>(property.propertyPath, GetSerializedPropertyRootComponent(property), true);
		 }
 
		 /// <summary>
		 /// Set the value of a field of the serializedProperty with the type T
		 /// </summary>
		 /// <typeparam name="T">The type of the field that is set</typeparam>
		 /// <param name="property">The serialized serializedProperty that should be set</param>
		 /// <param name="value">The new value for the specified serializedProperty</param>
		 /// <returns>Returns if the operation was successful or failed</returns>
		 public static bool SetValue<T>(this SerializedProperty property, T value)
		 {
			 
			 object obj = GetSerializedPropertyRootComponent(property);
			 //Iterate to parent object of the value, necessary if it is a nested object
			 string[] fieldStructure = property.propertyPath.Split('.');
			 
			 for (int i = 0; i < fieldStructure.Length - 1; i++)
			 {
				 obj = GetFieldOrPropertyValue<object>(fieldStructure[i], obj, true);
			 }
			 string fieldName = fieldStructure.Last();
 
			 return SetFieldOrPropertyValue(fieldName, obj, value, true);
		 }
 
		 /// <summary>
		 /// Get the component of a serialized serializedProperty
		 /// </summary>
		 /// <param name="property">The serializedProperty that is part of the component</param>
		 /// <returns>The root component of the serializedProperty</returns>
		 public static Component GetSerializedPropertyRootComponent(SerializedProperty property)
		 {
			 return (Component)property.serializedObject.targetObject;
		 }
 
		 /// <summary>
		 /// Iterates through objects to handle objects that are nested in the root object
		 /// </summary>
		 /// <typeparam name="T">The type of the nested object</typeparam>
		 /// <param name="path">Path to the object through other properties e.g. PlayerInformation.Health</param>
		 /// <param name="obj">The root object from which this path leads to the serializedProperty</param>
		 /// <param name="includeAllBases">Include base classes and interfaces as well</param>
		 /// <returns>Returns the nested object casted to the type T</returns>
		 public static T GetNestedObject<T>(string path, object obj, bool includeAllBases = false)
		 {
			 foreach (string part in path.Split('.'))
			 {
				 obj = GetFieldOrPropertyValue<object>(part, obj, includeAllBases);
			 }
			 
			 return (T)obj;
		 }
 
		 public static T GetFieldOrPropertyValue<T>(string fieldName, object obj, bool includeAllBases = false, BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
		 {
			 FieldInfo field = obj.GetType().GetField(fieldName, bindings);
			 if (field != null) return (T)field.GetValue(obj);
 
			 PropertyInfo property = obj.GetType().GetProperty(fieldName, bindings);
			 if (property != null) return (T)property.GetValue(obj, null);
 
			 if (includeAllBases)
			 {
				 foreach (Type type in GetBaseClassesAndInterfaces(obj.GetType()))
				 {
					 field = type.GetField(fieldName, bindings);
					 if (field != null) return (T)field.GetValue(obj);
 
					 property = type.GetProperty(fieldName, bindings);
					 if (property != null) return (T)property.GetValue(obj, null);
				 }
			 }
 
			 return default(T);
		 }
 
		 public static bool SetFieldOrPropertyValue(string fieldName, object obj, object value, bool includeAllBases = false, BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
		 {
			 FieldInfo field = obj.GetType().GetField(fieldName, bindings);
			 if (field != null)
			 {
				 field.SetValue(obj, value);
				 return true;
			 }
 
			 PropertyInfo property = obj.GetType().GetProperty(fieldName, bindings);
			 if (property != null)
			 {
				 property.SetValue(obj, value, null);
				 return true;
			 }
 
			 if (includeAllBases)
			 {
				 foreach (Type type in GetBaseClassesAndInterfaces(obj.GetType()))
				 {
					 field = type.GetField(fieldName, bindings);
					 if (field != null)
					 {
						 field.SetValue(obj, value);
						 return true;
					 }
 
					 property = type.GetProperty(fieldName, bindings);
					 if (property != null)
					 {
						 property.SetValue(obj, value, null);
						 return true;
					 }
				 }
			 }
			 return false;
		 }
 
		 public static IEnumerable<Type> GetBaseClassesAndInterfaces(this Type type, bool includeSelf = false)
		 {
			 List<Type> allTypes = new List<Type>();
 
			 if (includeSelf) allTypes.Add(type);
 
			 if (type.BaseType == typeof(object))
			 {
				 allTypes.AddRange(type.GetInterfaces());
			 }
			 else {
				 allTypes.AddRange(
						 Enumerable
						 .Repeat(type.BaseType, 1)
						 .Concat(type.GetInterfaces())
						 .Concat(type.BaseType.GetBaseClassesAndInterfaces())
						 .Distinct());
			 }
 
			 return allTypes;
		 }

		 public static string GetUniqueId(this SerializedProperty property)
		 {
			 return $"{property.serializedObject.targetObject.GetInstanceID().ToString()}{property.propertyPath}";
		 }
	 }
 }