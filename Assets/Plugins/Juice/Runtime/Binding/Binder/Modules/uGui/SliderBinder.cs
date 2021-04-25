using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(Slider))]
	public class SliderBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo minValue = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo maxValue = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo value = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
		[SerializeField] private BindingInfo onValueChangedCommand = new BindingInfo(typeof(IObservableCommand<float>));

		private Slider slider;

		protected virtual void Reset()
		{
			minValue = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
			maxValue = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
			value = new BindingInfo(typeof(IReadOnlyObservableVariable<float>));
			onValueChangedCommand = new BindingInfo(typeof(IObservableCommand<float>));
		}

		protected override void Awake()
		{
			base.Awake();

			slider = GetComponent<Slider>();

			RegisterVariable<float>(minValue).OnChanged(OnMinValueChanged);
			RegisterVariable<float>(maxValue).OnChanged(OnMaxValueChanged);
			RegisterVariable<float>(value).OnChanged(OnValueChanged);
			RegisterCommand<float>(onValueChangedCommand)
				.AddExecuteTrigger(slider.onValueChanged)
				.OnCanExecuteChanged(OnCanExecuteChanged);
		}


#if UNITY_EDITOR
		[MenuItem("CONTEXT/Slider/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Slider context = (Slider) command.context;
			context.GetOrAddComponent<SliderBinder>();
		}
#endif

		private void OnMinValueChanged(float newValue)
		{
			slider.minValue = newValue;
		}

		private void OnMaxValueChanged(float newValue)
		{
			slider.maxValue = newValue;
		}

		private void OnValueChanged(float newValue)
		{
			slider.value = newValue;
		}

		private void OnCanExecuteChanged(bool newValue)
		{
			slider.interactable = newValue;
		}
	}
}
