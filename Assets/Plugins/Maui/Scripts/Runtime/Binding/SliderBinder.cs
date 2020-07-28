using UnityEngine;
using UnityEngine.UI;

namespace Maui
{
	[RequireComponent(typeof(Slider))]
	public class SliderBinder : MonoBehaviour, IBinder<float>
	{
		[SerializeField] private BindingInfo minValueBindingInfo = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo maxValueBindingInfo = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo valueBindingInfo = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo valueCommandBindingInfo = new BindingInfo(typeof(IObservableCommand<float>));

		private Slider slider;

		private VariableBinding<float> minValueBinding;
		private VariableBinding<float> maxValueBinding;
		private VariableBinding<float> valueBinding;
		private CommandBinding<float> valueCommandBinding;

		protected virtual void Reset()
		{
			minValueBindingInfo = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
			maxValueBindingInfo = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
			valueBindingInfo = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
			valueCommandBindingInfo = new BindingInfo(typeof(IObservableCommand<float>));
		}
		
		protected virtual void Awake()
		{
			slider = GetComponent<Slider>();
			slider.onValueChanged.AddListener(SliderValueChangedHandler);

			minValueBinding = new VariableBinding<float>(minValueBindingInfo, this);
			minValueBinding.Property.Changed += MinValueChangedHandler;
			
			maxValueBinding = new VariableBinding<float>(maxValueBindingInfo, this);
			maxValueBinding.Property.Changed += MaxValueChangedHandler;
			
			valueBinding = new VariableBinding<float>(valueBindingInfo, this);
			valueBinding.Property.Changed += ValueChangedHandler;
			
			valueCommandBinding = new CommandBinding<float>(valueCommandBindingInfo, this);
			valueCommandBinding.Property.CanExecute.Changed += ValueCommandCanExecuteChangedHandler;
		}

		protected virtual void OnEnable()
		{
			minValueBinding.Bind();
			maxValueBinding.Bind();
			valueBinding.Bind();
		}

		protected virtual void OnDisable()
		{
			minValueBinding.Unbind();
			maxValueBinding.Unbind();
			valueBinding.Unbind();
		}

		private void SliderValueChangedHandler(float newValue)
		{
			valueCommandBinding.Property.Execute(newValue);
		}

		private void MinValueChangedHandler(float newValue)
		{
			slider.minValue = newValue;
		}
		
		private void MaxValueChangedHandler(float newValue)
		{
			slider.maxValue = newValue;
		}
		
		private void ValueChangedHandler(float newValue)
		{
			slider.value = newValue;
		}
		
		private void ValueCommandCanExecuteChangedHandler(bool newValue)
		{
			slider.interactable = newValue;
		}
	}
}