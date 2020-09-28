using UnityEditor;
using UnityEngine;

namespace Maui.Editor
{
	[CustomPropertyDrawer(typeof(DisableAtRuntimeAttribute))]
	public class DisableAtRuntimeAttributeDrawer : PropertyDrawer
	{
		private PropertyDrawer cachedPropertyDrawer;
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginDisabledGroup(Application.isPlaying);

			CachePropertyDrawer();
			
			if (cachedPropertyDrawer != null)
			{
				cachedPropertyDrawer.OnGUI(position, property, label);
			}
			else
			{
				EditorGUI.PropertyField(position, property, label, true);
			}
			
			EditorGUI.EndDisabledGroup();
		}

		private void CachePropertyDrawer()
		{
			if (cachedPropertyDrawer == null)
			{
				cachedPropertyDrawer = PropertyDrawerUtility.GetCustomPropertyDrawer(fieldInfo);
			}
		}
	}
}