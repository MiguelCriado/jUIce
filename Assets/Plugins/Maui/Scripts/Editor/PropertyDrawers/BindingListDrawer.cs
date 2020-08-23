using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Maui.Editor
{
	[CustomPropertyDrawer(typeof(BindingList))]
	public class BindingListDrawer : PropertyDrawer
	{
		private bool isCached;
		private BindingList target;
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
				target = serializedProperty.GetValue<BindingList>();
				listProperty = serializedProperty.FindPropertyRelative("bindings");
				reorderableList = GetList(listProperty);
				isCached = true;
			}
		}
		
		private ReorderableList GetList( SerializedProperty serializedProperty )
		{
			if ( reorderableList == null )
			{
				reorderableList = new ReorderableList( serializedProperty.serializedObject, serializedProperty );
				reorderableList.elementHeightCallback += ElementHeightCallback;
				reorderableList.drawElementCallback += DrawElementCallback;
				reorderableList.onAddCallback += OnAddCallback;
			}

			return reorderableList;
		}

		private float ElementHeightCallback(int index)
		{ 
			SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
			return EditorGUI.GetPropertyHeight(element);
		}

		private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
			EditorGUI.PropertyField(rect, element, GUIContent.none);
		}
		
		private void OnAddCallback(ReorderableList list)
		{
			target.AddElement();
		}
	}
}