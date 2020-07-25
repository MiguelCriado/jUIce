using UnityEditor;

namespace Maui.Editor
{
	[CustomEditor(typeof(ViewModelComponent))]
	public class ViewModelComponentEditor : UnityEditor.Editor
	{
		private ViewModelComponent viewModelComponent => target as ViewModelComponent;

		private SerializedProperty expectedTypeProperty;
		
		private void OnEnable()
		{
			expectedTypeProperty = serializedObject.FindProperty("expectedType");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			
			var injector = viewModelComponent.GetComponent<IViewModelInjector>();
			RefreshType(injector);
			
			bool shouldDisableEditor = injector != null && injector.Target == viewModelComponent;
			EditorGUI.BeginDisabledGroup(shouldDisableEditor);
			
			EditorGUILayout.PropertyField(expectedTypeProperty);

			EditorGUI.EndDisabledGroup();
			
			serializedObject.ApplyModifiedProperties();
		}

		private void RefreshType(IViewModelInjector injector)
		{
			if (injector != null && injector.InjectionType != viewModelComponent.ExpectedType)
			{
				expectedTypeProperty.GetValue<SerializableType>().Type = injector.InjectionType;
			}
		}
	}
}