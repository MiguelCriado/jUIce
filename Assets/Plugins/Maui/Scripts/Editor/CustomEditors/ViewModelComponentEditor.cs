using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Maui.Editor
{
	[CustomEditor(typeof(ViewModelComponent), true)]
	public class ViewModelComponentEditor : UnityEditor.Editor
	{
		private ViewModelComponent viewModelComponent => target as ViewModelComponent;

		private SerializedProperty expectedTypeProperty;
		private SerializedProperty idProperty;
		
		protected virtual void OnEnable()
		{
			expectedTypeProperty = serializedObject.FindProperty("expectedType");
			idProperty = serializedObject.FindProperty("id");
			BindingInfoTracker.RefreshBindingInfoDrawers();
		}

		protected virtual void OnDisable()
		{
			BindingInfoTracker.RefreshBindingInfoDrawers();
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			DrawBaseInspector();
			DrawChildFields();
			serializedObject.ApplyModifiedProperties();
		}

		protected virtual void DrawBaseInspector()
		{
			RefreshType();
			DrawExpectedType();
			DrawId();
		}

		protected virtual void DrawChildFields()
		{
			List<FieldInfo> childFields = new List<FieldInfo>();
			Type type = target.GetType();

			CustomEditor customEditor = GetType().GetCustomAttribute<CustomEditor>();
			FieldInfo fieldInfo = typeof(CustomEditor)
				.GetField("m_InspectedType",
				BindingFlags.NonPublic 
				| BindingFlags.Instance 
				| BindingFlags.GetField);
			Type baseType = fieldInfo.GetValue(customEditor) as Type;
			
			while (type != null && type != baseType)
			{
				childFields.InsertRange(0, type.GetFields(
					BindingFlags.DeclaredOnly
					| BindingFlags.Instance
					| BindingFlags.Public
					| BindingFlags.NonPublic));
				
				type = type.BaseType;
			}

			foreach (FieldInfo field in childFields)
			{
				if (field.IsPublic || field.GetCustomAttribute(typeof(SerializeField)) != null)
				{
					EditorGUILayout.PropertyField(serializedObject.FindProperty(field.Name));
				}
			}
		}

		protected void DrawExpectedType()
		{
			var injector = viewModelComponent.GetComponents<IViewModelInjector>().First(x => x.Target == viewModelComponent);
			bool shouldDisableEditor = injector != null;
			EditorGUI.BeginDisabledGroup(shouldDisableEditor);

			EditorGUILayout.PropertyField(expectedTypeProperty);

			EditorGUI.EndDisabledGroup();
		}
		
		protected void DrawId()
		{
			EditorGUILayout.PropertyField(idProperty);
		}
		
		private void RefreshType()
		{
			var injectors = viewModelComponent.GetComponents<IViewModelInjector>();

			foreach (IViewModelInjector current in injectors)
			{
				if (current.Target == viewModelComponent && current.InjectionType != viewModelComponent.ExpectedType)
				{
					expectedTypeProperty.GetValue<SerializableType>().Type = current.InjectionType;
				}
			}
		}
	}
}