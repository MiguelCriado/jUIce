using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	[RequireComponent(typeof(Slider))]
	public class SliderBinder : MonoBehaviour, IBinder<float>
	{
		[SerializeField] private BindingInfo minValue = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo maxValue = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo value = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo valueChangedCommand = new BindingInfo(typeof(IObservableCommand<float>));

		private Slider slider;

		private VariableBinding<float> minValueBinding;
		private VariableBinding<float> maxValueBinding;
		private VariableBinding<float> valueBinding;
		private CommandBinding<float> valueChangedCommandBinding;

		protected virtual void Reset()
		{
			minValue = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
			maxValue = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
			value = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
			valueChangedCommand = new BindingInfo(typeof(IObservableCommand<float>));
		}

		protected virtual void Awake()
		{
			slider = GetComponent<Slider>();
			slider.onValueChanged.AddListener(OnSliderValueChanged);

			minValueBinding = new VariableBinding<float>(minValue, this);
			minValueBinding.Property.Changed += OnMinValueBindingChanged;

			maxValueBinding = new VariableBinding<float>(maxValue, this);
			maxValueBinding.Property.Changed += OnMaxValueBindingChanged;

			valueBinding = new VariableBinding<float>(value, this);
			valueBinding.Property.Changed += OnValueBindingChanged;

			valueChangedCommandBinding = new CommandBinding<float>(valueChangedCommand, this);
			valueChangedCommandBinding.Property.CanExecute.Changed += OnValueChangedCommandBindingCanExecuteChanged;
		}

		protected virtual void OnEnable()
		{
			minValueBinding.Bind();
			maxValueBinding.Bind();
			valueBinding.Bind();
			valueChangedCommandBinding.Bind();
		}

		protected virtual void OnDisable()
		{
			minValueBinding.Unbind();
			maxValueBinding.Unbind();
			valueBinding.Unbind();
			valueChangedCommandBinding.Unbind();
		}

		private void OnSliderValueChanged(float newValue)
		{
			valueChangedCommandBinding.Property.Execute(newValue);
		}

		private void OnMinValueBindingChanged(float newValue)
		{
			slider.minValue = newValue;
		}

		private void OnMaxValueBindingChanged(float newValue)
		{
			slider.maxValue = newValue;
		}

		private void OnValueBindingChanged(float newValue)
		{
			slider.value = newValue;
		}

		private void OnValueChangedCommandBindingCanExecuteChanged(bool newValue)
		{
			slider.interactable = newValue;
		}
	}
}
