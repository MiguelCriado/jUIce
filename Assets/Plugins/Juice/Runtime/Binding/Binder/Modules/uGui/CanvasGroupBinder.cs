using Juice.Utils;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(CanvasGroup))]
	public class CanvasGroupBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo alpha = BindingInfo.Variable<float>();
		[SerializeField] private BindingInfo interactable = BindingInfo.Variable<bool>();
		[SerializeField] private BindingInfo blocksRaycasts = BindingInfo.Variable<bool>();
		[SerializeField] private BindingInfo ignoreParentGroups = BindingInfo.Variable<bool>();

		private CanvasGroup canvasGroup;

		protected override void Awake()
		{
			base.Awake();

			canvasGroup = GetComponent<CanvasGroup>();

			RegisterVariable<float>(alpha).OnChanged(OnAlphaChanged);
			RegisterVariable<bool>(interactable).OnChanged(OnInteractableChanged);
			RegisterVariable<bool>(blocksRaycasts).OnChanged(OnBlocksRaycastsChanged);
			RegisterVariable<bool>(ignoreParentGroups).OnChanged(OnIgnoreParentGroupsChanged);
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/CanvasGroup/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			CanvasGroup context = (CanvasGroup) command.context;
			context.GetOrAddComponent<CanvasGroupBinder>();
		}
#endif

		private void OnAlphaChanged(float newValue)
		{
			canvasGroup.alpha = newValue;
		}

		private void OnInteractableChanged(bool newValue)
		{
			canvasGroup.interactable = newValue;
		}

		private void OnBlocksRaycastsChanged(bool newValue)
		{
			canvasGroup.blocksRaycasts = newValue;
		}

		private void OnIgnoreParentGroupsChanged(bool newValue)
		{
			canvasGroup.ignoreParentGroups = newValue;
		}
	}
}
