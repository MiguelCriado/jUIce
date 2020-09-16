using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Maui.Editor
{
	public class ViewCreationWizardEditorWindow : EditorWindow
	{
		private static readonly string DefaultNamespaceKey = "Key_ViewWizard_DefaultNamespace";

		private static readonly string DefaultViewName = "MyView";

		private static readonly float WindowWidth = 600f;

		private static readonly float WindowHeight = 7 * EditorGUIUtility.singleLineHeight
		                                             + 6 * EditorGUIUtility.standardVerticalSpacing;

		[SerializeField] private string @namespace;
		[SerializeField] [TypeConstraint(typeof(IView))] private SerializableType supertype;
		[SerializeField] [TypeConstraint(typeof(IViewModel))] private SerializableType viewModelType;
		[SerializeField] private string className;
		[SerializeField] private string uri;

		private SerializedObject serializedObject;
		private SerializedProperty namespaceProp;
		private SerializedProperty supertypeProp;
		private SerializedProperty viewModelTypeProp;
		private SerializedProperty classNameProp;
		private SerializedProperty uriProp;

		private void OnEnable()
		{
			serializedObject = new SerializedObject(this);
			namespaceProp = serializedObject.FindProperty(nameof(@namespace));
			supertypeProp = serializedObject.FindProperty(nameof(supertype));
			viewModelTypeProp = serializedObject.FindProperty(nameof(viewModelType));
			classNameProp = serializedObject.FindProperty(nameof(className));
			uriProp = serializedObject.FindProperty(nameof(uri));
		}

		[MenuItem("Window/View Wizard")]
		public static void ShowWindow()
		{
			ShowForWindow();
		}

		public static void ShowForWindow()
		{
			ShowWindow(typeof(Window));
		}

		public static void ShowForPanel()
		{
			ShowWindow(typeof(Panel));
		}

		public static void ShowWindow(Type supertype)
		{
			var window = CreateInstance<ViewCreationWizardEditorWindow>();
			
			string saveDirectory = ProjectViewUtility.GetSelectedPathOrFallback();
			
			window.SetupNamespace();
			window.SetupSupertype(supertype);
			window.SetupViewModelType(typeof(IViewModel));
			window.SetupClassName(DefaultViewName);
			window.SetupUri(saveDirectory);

			window.titleContent = new GUIContent("View Creation Wizard");
			window.minSize = new Vector2(WindowWidth, WindowHeight);
			window.maxSize = new Vector2(WindowWidth * 2, WindowHeight);
			
			try
			{
				window.ShowModal();
			}
			catch (NullReferenceException e)
			{
				
			}
		}

		public void OnGUI()
		{
			serializedObject.Update();
			
			DrawNamespace();
			DrawSuperType();
			DrawViewModelType();
			DrawName();
			DrawUri();
			DrawCreateButton();

			if (serializedObject?.targetObject)
			{
				serializedObject.ApplyModifiedProperties();
			}
		}

		private void SetupNamespace()
		{
			string rootNamespace = EditorSettings.projectGenerationRootNamespace;

			@namespace = EditorPrefs.GetString(DefaultNamespaceKey, rootNamespace);
		}

		private void SetupSupertype(Type targetType)
		{
			supertype.Type = targetType;
		}

		private void SetupViewModelType(Type targetType)
		{
			viewModelType.Type = targetType;
		}

		private void SetupClassName(string value)
		{
			className = value;
		}

		private void SetupUri(string value)
		{
			uri = value;
		}
		
		private void DrawNamespace()
		{
			EditorGUILayout.PropertyField(namespaceProp);
		}

		private void DrawSuperType()
		{
			EditorGUILayout.PropertyField(supertypeProp);
		}

		private void DrawViewModelType()
		{
			EditorGUILayout.PropertyField(viewModelTypeProp);
		}

		private void DrawName()
		{
			EditorGUILayout.PropertyField(classNameProp);
		}

		private void DrawUri()
		{
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUI.BeginDisabledGroup(true);
				EditorGUILayout.PropertyField(uriProp);
				EditorGUI.EndDisabledGroup();

				if (GUILayout.Button("Edit", GUILayout.MaxWidth(36f)))
				{
					string selectedPath = EditorUtility.SaveFilePanel
					(
						"Save new View",
						uri,
						$"{className}.cs",
						"cs"
					);

					if (selectedPath.Length > 0)
					{
						uriProp.stringValue = Path.GetDirectoryName(selectedPath);
						classNameProp.stringValue = Path.GetFileNameWithoutExtension(selectedPath);
					}
				}
			}
		}

		private void DrawCreateButton()
		{
			GUILayout.Space(EditorGUIUtility.singleLineHeight / 2f);
			
			if (GUILayout.Button("Create View"))
			{
				EditorPrefs.SetString(DefaultNamespaceKey, @namespace);
				
				FileInfo fileInfo = new FileInfo(Path.Combine(uri, $"{className}.cs"));

				ClassFileWriter.ClassDefinition classDefinition = new ClassFileWriter.ClassDefinition
				{
					Namespace = @namespace,
					Imports = new List<string> (new HashSet<string> {viewModelType.Type.Namespace, supertype.Type.Namespace}),
					Name = className,
					Superclass = supertype.Type.Name,
					SuperclassGenerics = new List<string> {viewModelType.Type.Name}
				};

				ClassFileWriter.WriteClassFile(fileInfo, classDefinition);
				AssetDatabase.Refresh();
				
				Close();
			}
		}
	}
}