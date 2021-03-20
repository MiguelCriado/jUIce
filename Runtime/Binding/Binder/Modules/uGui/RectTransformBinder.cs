using Juice.Utils;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(RectTransform))]
	public class RectTransformBinder : MonoBehaviour, IBinder<Vector2>
	{
		[SerializeField] private BindingInfo anchoredPosition = new BindingInfo(typeof(IReadOnlyObservableVariable<Vector2>));
		[SerializeField] private BindingInfo anchoredPosition3D = new BindingInfo(typeof(IReadOnlyObservableVariable<Vector3>));
		[SerializeField] private BindingInfo anchorMax = new BindingInfo(typeof(IReadOnlyObservableVariable<Vector2>));
		[SerializeField] private BindingInfo anchorMin = new BindingInfo(typeof(IReadOnlyObservableVariable<Vector2>));
		[SerializeField] private BindingInfo offsetMax = new BindingInfo(typeof(IReadOnlyObservableVariable<Vector2>));
		[SerializeField] private BindingInfo offsetMin = new BindingInfo(typeof(IReadOnlyObservableVariable<Vector2>));
		[SerializeField] private BindingInfo pivot = new BindingInfo(typeof(IReadOnlyObservableVariable<Vector2>));
		[SerializeField] private BindingInfo sizeDelta = new BindingInfo(typeof(IReadOnlyObservableVariable<Vector2>));

		private RectTransform rectTransform;

		private VariableBinding<Vector2> anchoredPositionBinding;
		private VariableBinding<Vector3> anchoredPosition3DBinding;
		private VariableBinding<Vector2> anchorMaxBinding;
		private VariableBinding<Vector2> anchorMinBinding;
		private VariableBinding<Vector2> offsetMaxBinding;
		private VariableBinding<Vector2> offsetMinBinding;
		private VariableBinding<Vector2> pivotBinding;
		private VariableBinding<Vector2> sizeDeltaBinding;

		protected virtual void Awake()
		{
			rectTransform = GetComponent<RectTransform>();

			anchoredPositionBinding = new VariableBinding<Vector2>(anchoredPosition, this);
			anchoredPositionBinding.Property.Changed += OnAnchoredPositionChanged;

			anchoredPosition3DBinding = new VariableBinding<Vector3>(anchoredPosition3D, this);
			anchoredPosition3DBinding.Property.Changed += OnAnchoredPosition3DChanged;

			anchorMaxBinding = new VariableBinding<Vector2>(anchorMax, this);
			anchorMaxBinding.Property.Changed += OnAnchorMaxChanged;

			anchorMinBinding = new VariableBinding<Vector2>(anchorMin, this);
			anchorMinBinding.Property.Changed += OnAnchorMinChanged;

			offsetMaxBinding = new VariableBinding<Vector2>(offsetMax, this);
			offsetMaxBinding.Property.Changed += OnOffsetMaxChanged;

			offsetMinBinding = new VariableBinding<Vector2>(offsetMin, this);
			offsetMinBinding.Property.Changed += OnOffsetMinChanged;

			pivotBinding = new VariableBinding<Vector2>(pivot, this);
			pivotBinding.Property.Changed += OnPivotChanged;

			sizeDeltaBinding = new VariableBinding<Vector2>(sizeDelta, this);
			sizeDeltaBinding.Property.Changed += OnSizeDeltaChanged;
		}

		protected virtual void OnEnable()
		{
			anchoredPositionBinding.Bind();
			anchoredPosition3DBinding.Bind();
			anchorMaxBinding.Bind();
			anchorMinBinding.Bind();
			offsetMaxBinding.Bind();
			offsetMinBinding.Bind();
			pivotBinding.Bind();
			sizeDeltaBinding.Bind();
		}

		protected virtual void OnDisable()
		{
			anchoredPositionBinding.Unbind();
			anchoredPosition3DBinding.Unbind();
			anchorMaxBinding.Unbind();
			anchorMinBinding.Unbind();
			offsetMaxBinding.Unbind();
			offsetMinBinding.Unbind();
			pivotBinding.Unbind();
			sizeDeltaBinding.Unbind();
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
