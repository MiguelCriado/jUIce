using UnityEditor;
using UnityEngine;

namespace Maui.Editor
{
	[CustomPropertyDrawer(typeof(BindingType))]
	public class BindingTypeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			
			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(position, property, label);
			
			if (EditorGUI.EndChangeCheck())
			{
				BindingInfoTracker.RefreshBindingInfoDrawers();
			}
			
			EditorGUI.EndProperty();
		}
	}
}