using Juice.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Juice.Editor
{
	[CustomEditor(typeof(InteractionBlockingTracker))]
	public class InteractionBlockingTrackerEditor : UnityEditor.Editor
	{
		private static readonly string UnsafeRaycastersFoundMessage = $"Some {nameof(GraphicRaycaster)} components found down the hierarchy without an {nameof(InteractionBlocker)} attached to them.";

		private InteractionBlockingTracker blockingTracker => target as InteractionBlockingTracker;

		private bool unsafeRaycastersFound;

		private void OnEnable()
		{
			unsafeRaycastersFound = CheckForUnsafeRaycasters(blockingTracker);
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			serializedObject.Update();

			if (unsafeRaycastersFound)
			{
				using (new EditorGUILayout.VerticalScope("box"))
				{
					EditorGUILayout.HelpBox(UnsafeRaycastersFoundMessage, MessageType.Warning);

					if (GUILayout.Button("Fix"))
					{
						FixUnsafeRaycasters(blockingTracker);
						unsafeRaycastersFound = false;
					}
				}
			}

			serializedObject.ApplyModifiedProperties();
		}

		private bool CheckForUnsafeRaycasters(Component current)
		{
			bool result = false;
			var raycaster = current.GetComponent<GraphicRaycaster>();
			var interactionBlocker = current.GetComponent<InteractionBlocker>();

			if (raycaster && !interactionBlocker)
			{
				result = true;
			}
			else
			{
				int i = 0;

				while (result == false && i < current.transform.childCount)
				{
					result = CheckForUnsafeRaycasters(current.transform.GetChild(i));
					i++;
				}
			}

			return result;
		}

		private void FixUnsafeRaycasters(Component current)
		{
			if (current.GetComponent<GraphicRaycaster>())
			{
				current.GetOrAddComponent<InteractionBlocker>();
			}

			foreach (Transform currentChild in current.transform)
			{
				FixUnsafeRaycasters(currentChild);
			}
		}
	}
}
