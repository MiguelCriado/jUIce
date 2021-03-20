using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	[RequireComponent(typeof(Scrollbar))]
	public class ScrollbarBinder : CommandBinder<float>
	{
		protected override string BindingInfoName { get; } = "OnValueChanged Command";

		[SerializeField] private BindingInfo direction = new BindingInfo(typeof(IReadOnlyObservableVariable<Scrollbar.Direction>));
		[SerializeField] private BindingInfo value = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo size = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo numberOfSteps = new BindingInfo(typeof(IReadOnlyObservableVariable<int>));

		private Scrollbar scrollbar;

		private VariableBinding<Scrollbar.Direction> directionBinding;
		private VariableBinding<float> valueBinding;
		private VariableBinding<float> sizeBinding;
		private VariableBinding<int> numberOfStepsBinding;

		protected override void Awake()
		{
			base.Awake();

			scrollbar = GetComponent<Scrollbar>();
			scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);

			directionBinding = new VariableBinding<Scrollbar.Direction>(direction, this);
			directionBinding.Property.Changed += OnDirectionChanged;

			valueBinding = new VariableBinding<float>(value, this);
			valueBinding.Property.Changed += OnValueChanged;

			sizeBinding = new VariableBinding<float>(size, this);
			sizeBinding.Property.Changed += OnSizeChanged;

			numberOfStepsBinding = new VariableBinding<int>(numberOfSteps, this);
			numberOfStepsBinding.Property.Changed += OnNumberOfStepsChanged;
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			directionBinding.Bind();
			valueBinding.Bind();
			sizeBinding.Bind();
			numberOfStepsBinding.Bind();
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			directionBinding.Unbind();
			valueBinding.Unbind();
			sizeBinding.Unbind();
			numberOfStepsBinding.Unbind();
		}

		protected override void OnCommandCanExecuteChanged(bool newValue)
		{
			scrollbar.interactable = newValue;
		}

		private void OnScrollbarValueChanged(float newValue)
		{
			ExecuteCommand(newValue);
		}

		private void OnDirectionChanged(Scrollbar.Direction newValue)
		{
			scrollbar.direction = newValue;
		}

		private void OnValueChanged(float newValue)
		{
			scrollbar.value = newValue;
		}

		private void OnSizeChanged(float newValue)
		{
			scrollbar.size = newValue;
		}

		private void OnNumberOfStepsChanged(int newValue)
		{
			scrollbar.numberOfSteps = newValue;
		}
	}
}
