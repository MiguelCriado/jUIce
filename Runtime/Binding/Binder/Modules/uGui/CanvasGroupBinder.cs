using UnityEngine;

namespace Juice
{
	[RequireComponent(typeof(CanvasGroup))]
	public class CanvasGroupBinder : MonoBehaviour, IBinder<float>
	{
		[SerializeField] private BindingInfo alpha = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo interactable = new BindingInfo(typeof(IReadOnlyObservableVariable<bool>));
		[SerializeField] private BindingInfo blocksRaycasts = new BindingInfo(typeof(IReadOnlyObservableVariable<bool>));
		[SerializeField] private BindingInfo ignoreParentGroups = new BindingInfo(typeof(IReadOnlyObservableVariable<bool>));

		private CanvasGroup canvasGroup;

		private VariableBinding<float> alphaBinding;
		private VariableBinding<bool> interactableBinding;
		private VariableBinding<bool> blocksRaycastsBinding;
		private VariableBinding<bool> ignoreParentGroupsBinding;

		protected virtual void Awake()
		{
			canvasGroup = GetComponent<CanvasGroup>();

			alphaBinding = new VariableBinding<float>(alpha, this);
			alphaBinding.Property.Changed += OnAlphaChanged;

			interactableBinding = new VariableBinding<bool>(interactable, this);
			interactableBinding.Property.Changed += OnInteractableChanged;

			blocksRaycastsBinding = new VariableBinding<bool>(blocksRaycasts, this);
			blocksRaycastsBinding.Property.Changed += OnBlocksRaycastsChanged;

			ignoreParentGroupsBinding = new VariableBinding<bool>(ignoreParentGroups, this);
			ignoreParentGroupsBinding.Property.Changed += OnIgnoreParentGroupsChanged;
		}

		protected virtual void OnEnable()
		{
			alphaBinding.Bind();
			interactableBinding.Bind();
			blocksRaycastsBinding.Bind();
			ignoreParentGroupsBinding.Bind();
		}

		protected virtual void OnDisable()
		{
			alphaBinding.Unbind();
			interactableBinding.Unbind();
			blocksRaycastsBinding.Unbind();
			ignoreParentGroupsBinding.Unbind();
		}

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
