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
		[SerializeField] private BindingInfo minValue = BindingInfo.Variable<float>();
		[SerializeField] private BindingInfo maxValue = BindingInfo.Variable<float>();
		[SerializeField] private BindingInfo value = BindingInfo.Variable<float>();
		[SerializeField] private BindingInfo onValueChanged = BindingInfo.Command<float>();

		private Slider slider;

		protected override void Awake()
		{
			base.Awake();

			slider = GetComponent<Slider>();

			RegisterVariable<float>(minValue).OnChanged(OnMinValueChanged);
			RegisterVariable<float>(maxValue).OnChanged(OnMaxValueChanged);
			RegisterVariable<float>(value).OnChanged(OnValueChanged);
			RegisterCommand<float>(onValueChanged)
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
