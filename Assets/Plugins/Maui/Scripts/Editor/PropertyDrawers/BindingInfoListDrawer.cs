using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Maui.Editor
{
	[CustomPropertyDrawer(typeof(BindingInfoList))]
	public class BindingInfoListDrawer : PropertyDrawer
	{
		private static readonly float ElementMargin = EditorGUIUtility.standardVerticalSpacing;

		private bool isCached;
		private BindingInfoList target;
		private SerializedProperty listProperty;
		private ReorderableList reorderableList;

		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			EditorGUI.BeginProperty(position, label, property);
			
			CacheData(property);
			
			reorderableList.DoList( position );
			
			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight( SerializedProperty serializedProperty, GUIContent label )
		{
			CacheData(serializedProperty);
			return reorderableList.GetHeight();
		}

		private void CacheData(SerializedProperty serializedProperty)
		{
			if (isCached == false)
			{
				target = serializedProperty.GetValue<BindingInfoList>();
				listProperty = serializedProperty.FindPropertyRelative("bindingInfoList");
				reorderableList = GetList(listProperty);
				isCached = true;
			}
		}
		
		private ReorderableList GetList( SerializedProperty serializedProperty )
		{
			if ( reorderableList == null )
			{
				reorderableList = new ReorderableList( serializedProperty.serializedObject, serializedProperty );
				reorderableList.drawHeaderCallback += DrawHeaderCallback;
				reorderableList.elementHeightCallback += ElementHeightCallback;
				reorderableList.drawElementCallback += DrawElementCallback;
				reorderableList.onAddCallback += OnAddCallback;
			}

			return reorderableList;
		}

		private void DrawHeaderCallback(Rect rect)
		{
			EditorGUI.LabelField(rect, "Binding Info List");
		}

		private float ElementHeightCallback(int index)
		{ 
			SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
			return EditorGUI.GetPropertyHeight(element) + ElementMargin;
		}

		private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += ElementMargin * 0.5f;
			rect.height -= ElementMargin * 0.5f;
			
			EditorGUI.PropertyField(rect, element, GUIContent.none);
		}
		
		private void OnAddCallback(ReorderableList list)
		{
			target.AddElement();
		}
	}
}