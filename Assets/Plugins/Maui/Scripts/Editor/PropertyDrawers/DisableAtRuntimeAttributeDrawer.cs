using UnityEditor;
using UnityEngine;

namespace Maui.Editor
{
	[CustomPropertyDrawer(typeof(DisableAtRuntimeAttribute))]
	public class DisableAtRuntimeAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginDisabledGroup(Application.isPlaying);
			EditorGUI.PropertyField(position, property, label, true);
			EditorGUI.EndDisabledGroup();
		}
	}
}