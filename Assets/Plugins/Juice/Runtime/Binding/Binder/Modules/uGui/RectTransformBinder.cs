using Juice.Utils;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(RectTransform))]
	public class RectTransformBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo anchoredPosition = BindingInfo.Variable<Vector2>();
		[SerializeField] private BindingInfo anchoredPosition3D = BindingInfo.Variable<Vector3>();
		[SerializeField] private BindingInfo anchorMax = BindingInfo.Variable<Vector2>();
		[SerializeField] private BindingInfo anchorMin = BindingInfo.Variable<Vector2>();
		[SerializeField] private BindingInfo offsetMax = BindingInfo.Variable<Vector2>();
		[SerializeField] private BindingInfo offsetMin = BindingInfo.Variable<Vector2>();
		[SerializeField] private BindingInfo pivot = BindingInfo.Variable<Vector2>();
		[SerializeField] private BindingInfo sizeDelta = BindingInfo.Variable<Vector2>();

		private RectTransform rectTransform;

		protected override void Awake()
		{
			base.Awake();

			rectTransform = GetComponent<RectTransform>();

			RegisterVariable<Vector2>(anchoredPosition).OnChanged(OnAnchoredPositionChanged);
			RegisterVariable<Vector3>(anchoredPosition3D).OnChanged(OnAnchoredPosition3DChanged);
			RegisterVariable<Vector2>(anchorMax).OnChanged(OnAnchorMaxChanged);
			RegisterVariable<Vector2>(anchorMin).OnChanged(OnAnchorMinChanged);
			RegisterVariable<Vector2>(offsetMax).OnChanged(OnOffsetMaxChanged);
			RegisterVariable<Vector2>(offsetMin).OnChanged(OnOffsetMinChanged);
			RegisterVariable<Vector2>(pivot).OnChanged(OnPivotChanged);
			RegisterVariable<Vector2>(sizeDelta).OnChanged(OnSizeDeltaChanged);
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/RectTransform/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			RectTransform context = (RectTransform) command.context;
			context.GetOrAddComponent<RectTransformBinder>();
		}
#endif

		private void OnAnchoredPositionChanged(Vector2 newValue)
		{
			rectTransform.anchoredPosition = newValue;
		}

		private void OnAnchoredPosition3DChanged(Vector3 newValue)
		{
			rectTransform.anchoredPosition3D = newValue;
		}

		private void OnAnchorMaxChanged(Vector2 newValue)
		{
			rectTransform.anchorMax = newValue;
		}

		private void OnAnchorMinChanged(Vector2 newValue)
		{
			rectTransform.anchorMin = newValue;
		}

		private void OnOffsetMaxChanged(Vector2 newValue)
		{
			rectTransform.offsetMax = newValue;
		}

		private void OnOffsetMinChanged(Vector2 newValue)
		{
			rectTransform.offsetMin = newValue;
		}

		private void OnPivotChanged(Vector2 newValue)
		{
			rectTransform.pivot = newValue;
		}

		private void OnSizeDeltaChanged(Vector2 newValue)
		{
			rectTransform.sizeDelta = newValue;
		}
	}
}
