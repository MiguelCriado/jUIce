using UnityEngine;

namespace Juice
{
	public class FollowTransformBinder : ConstantBinder<Transform>
	{
		[SerializeField] private ConstantBindingInfo<Transform> transformToFollow = new ConstantBindingInfo<Transform>();
		[SerializeField] private ConstantBindingInfo<Vector2> offset = new ConstantBindingInfo<Vector2>();

		protected override ConstantBindingInfo<Transform> BindingInfo => transformToFollow;

		private UiFrame UIFrame => GetUIFrameCache();

		private Camera mainCamera;
		private Transform target;
		private UiFrame uiFrameCache;
		private RectTransform referenceTransform;
		private VariableBinding<Vector2> offsetBinding;

		protected override void Awake()
		{
			base.Awake();

			offsetBinding = new VariableBinding<Vector2>(offset, this);
			offsetBinding.Property.Changed += OnOffsetBindingChanged;
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			offsetBinding.Bind();

			CacheFields();
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			offsetBinding.Unbind();
		}

		private void LateUpdate()
		{
			if (target && UIFrame && mainCamera)
			{
				Vector2 screenPosition = mainCamera.WorldToScreenPoint(target.position);
				RectTransformUtility.ScreenPointToLocalPointInRectangle(referenceTransform, screenPosition, UIFrame.UICamera, out Vector2 localPoint);
				((RectTransform) transform).anchoredPosition = localPoint + offsetBinding.Property.Value;
			}
		}

		protected override void Refresh(Transform value)
		{
			target = value;
		}

		private static void OnOffsetBindingChanged(Vector2 newValue)
		{

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

		private UiFrame GetUIFrameCache()
		{
			if (!uiFrameCache)
			{
				uiFrameCache = GetComponentInParent<UiFrame>();
			}

			return uiFrameCache;
		}
	}
}
