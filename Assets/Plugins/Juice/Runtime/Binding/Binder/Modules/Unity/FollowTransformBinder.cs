using UnityEngine;

namespace Juice
{
	public class FollowTransformBinder : ComponentBinder
	{
		[SerializeField] private ConstantBindingInfo<Transform> target = new ConstantBindingInfo<Transform>();
		[SerializeField] private ConstantBindingInfo<Vector2> offset = new ConstantBindingInfo<Vector2>();
		
		private UiFrame UIFrame => GetUIFrameCache();

		private Camera mainCamera;
		private UiFrame uiFrameCache;
		private RectTransform referenceTransform;
		private VariableBinding<Transform> targetBinding;
		private VariableBinding<Vector2> offsetBinding;

		protected override void Awake()
		{
			base.Awake();

			targetBinding = RegisterVariable<Transform>(target).GetBinding();
			offsetBinding = RegisterVariable<Vector2>(offset).GetBinding();
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			CacheFields();
		}

		private void LateUpdate()
		{
			if (targetBinding.IsBound && UIFrame && mainCamera && offsetBinding.IsBound)
			{
				Vector2 screenPosition = mainCamera.WorldToScreenPoint(targetBinding.Property.Value.position);
				RectTransformUtility.ScreenPointToLocalPointInRectangle(referenceTransform, screenPosition, UIFrame.UICamera, out Vector2 localPoint);
				((RectTransform) transform).anchoredPosition = localPoint + offsetBinding.Property.Value;
			}
		}

		private UiFrame GetUIFrameCache()
		{
			if (!uiFrameCache)
			{
				uiFrameCache = GetComponentInParent<UiFrame>();
			}

			return uiFrameCache;
		}

		private void CacheFields()
		{
			mainCamera = Camera.main;

			if (transform.parent)
			{
				referenceTransform = transform.parent as RectTransform;
			}
			else
			{
				referenceTransform = transform as RectTransform;
			}
		}
	}
}
