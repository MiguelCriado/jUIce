using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	[RequireComponent(typeof(GraphicRaycaster))]
	public class InteractionBlocker : MonoBehaviour
	{
		[HideInInspector, SerializeField] private GraphicRaycaster raycaster;

		private InteractionBlockingTracker currentTracker;

		private void Reset()
		{
			raycaster = GetComponent<GraphicRaycaster>();
		}

		private void Awake()
		{
			if (!raycaster)
			{
				raycaster = GetComponent<GraphicRaycaster>();
			}
		}

		private void Start()
		{
			Unsubscribe();
			Subscribe();
		}

		private void OnDestroy()
		{
			Unsubscribe();
		}

		private void OnTransformParentChanged()
		{
			Unsubscribe();
			Subscribe();
		}

		private void Subscribe()
		{
			currentTracker = GetComponentInParent<InteractionBlockingTracker>();

			if (currentTracker)
			{
				OnIsInteractableChanged(currentTracker.IsInteractable);

				currentTracker.IsInteractableChanged += OnIsInteractableChanged;
			}
		}

		private void Unsubscribe()
		{
			if (currentTracker)
			{
				currentTracker.IsInteractableChanged -= OnIsInteractableChanged;
			}
		}

		private void OnIsInteractableChanged(bool isInteractable)
		{
			raycaster.enabled = isInteractable;
		}
	}
}
