using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	[RequireComponent(typeof(LayoutElement))]
	public class LayoutElementBinder : MonoBehaviour, IBinder<bool>
	{
		[SerializeField] private BindingInfo ignoreLayout = new BindingInfo(typeof(IReadOnlyObservableVariable<bool>));
		[SerializeField] private BindingInfo minWidth = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo minHeight = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo preferredWidth = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo preferredHeight = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo flexibleWidth = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo flexibleHeight = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo layoutPriority = new BindingInfo(typeof(IReadOnlyObservableVariable<int>));

		private LayoutElement layoutElement;

		private VariableBinding<bool> ignoreLayoutBinding;
		private VariableBinding<float> minWidthBinding;
		private VariableBinding<float> minHeightBinding;
		private VariableBinding<float> preferredWidthBinding;
		private VariableBinding<float> preferredHeightBinding;
		private VariableBinding<float> flexibleWidthBinding;
		private VariableBinding<float> flexibleHeightBinding;
		private VariableBinding<int> layoutPriorityBinding;

		protected virtual void Awake()
		{
			layoutElement = GetComponent<LayoutElement>();

			ignoreLayoutBinding = new VariableBinding<bool>(ignoreLayout, this);
			ignoreLayoutBinding.Property.Changed += OnIgnoreLayoutChanged;

			minWidthBinding = new VariableBinding<float>(minWidth, this);
			minWidthBinding.Property.Changed += OnMinWidthChanged;

			minHeightBinding = new VariableBinding<float>(minHeight, this);
			minHeightBinding.Property.Changed += OnMinHeightChanged;

			preferredWidthBinding = new VariableBinding<float>(preferredWidth, this);
			preferredWidthBinding.Property.Changed += OnPreferredWidthChanged;

			preferredHeightBinding = new VariableBinding<float>(preferredHeight, this);
			preferredHeightBinding.Property.Changed += OnPreferredHeightChanged;

			flexibleWidthBinding = new VariableBinding<float>(flexibleWidth, this);
			flexibleWidthBinding.Property.Changed += OnFlexibleWidthChanged;

			flexibleHeightBinding = new VariableBinding<float>(flexibleHeight, this);
			flexibleHeightBinding.Property.Changed += OnFlexibleHeightChanged;

			layoutPriorityBinding = new VariableBinding<int>(layoutPriority, this);
			layoutPriorityBinding.Property.Changed += OnLayoutPriorityChanged;
		}

		protected virtual void OnEnable()
		{
			ignoreLayoutBinding.Bind();
			minWidthBinding.Bind();
			minHeightBinding.Bind();
			preferredWidthBinding.Bind();
			preferredHeightBinding.Bind();
			flexibleWidthBinding.Bind();
			flexibleHeightBinding.Bind();
			layoutPriorityBinding.Bind();
		}

		protected virtual void OnDisable()
		{
			ignoreLayoutBinding.Unbind();
			minWidthBinding.Unbind();
			minHeightBinding.Unbind();
			preferredWidthBinding.Unbind();
			preferredHeightBinding.Unbind();
			flexibleWidthBinding.Unbind();
			flexibleHeightBinding.Unbind();
			layoutPriorityBinding.Unbind();
		}

		private void OnIgnoreLayoutChanged(bool newValue)
		{
			layoutElement.ignoreLayout = newValue;
		}

		private void OnMinWidthChanged(float newValue)
		{
			layoutElement.minWidth = newValue;
		}

		private void OnMinHeightChanged(float newValue)
		{
			layoutElement.minHeight = newValue;
		}

		private void OnPreferredWidthChanged(float newValue)
		{
			layoutElement.preferredWidth = newValue;
		}

		private void OnPreferredHeightChanged(float newValue)
		{
			layoutElement.preferredHeight = newValue;
		}

		private void OnFlexibleWidthChanged(float newValue)
		{
			layoutElement.flexibleWidth = newValue;
		}

		private void OnFlexibleHeightChanged(float newValue)
		{
			layoutElement.flexibleHeight = newValue;
		}

		private void OnLayoutPriorityChanged(int newValue)
		{
			layoutElement.layoutPriority = newValue;
		}
	}
}
