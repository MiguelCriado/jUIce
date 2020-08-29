using UnityEditor;
using UnityEngine;

namespace Maui.Editor
{
	[CustomPropertyDrawer(typeof(MathComparisonType))]
	public class MathComparisonTypeDrawer : PropertyDrawer
	{
		private static readonly string[] Options = {"=", "!=", ">", ">=", "<", "<="};

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			property.intValue = EditorGUI.Popup(position, label.text, property.intValue, Options);
		}
	}
}