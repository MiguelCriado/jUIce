using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(Slider))]
	public class SliderExtraBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo direction = new BindingInfo(typeof(IReadOnlyObservableVariable<Slider.Direction>));
		[SerializeField] private BindingInfo wholeNumbers = new BindingInfo(typeof(IReadOnlyObservableVariable<bool>));

		private Slider slider;

		protected override void Awake()
		{
			base.Awake();

			slider = GetComponent<Slider>();

			RegisterVariable<Slider.Direction>(direction).OnChanged(OnDirectionChanged);
			RegisterVariable<bool>(wholeNumbers).OnChanged(OnWholeNumbersChanged);
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/Slider/Add Extra Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Slider context = (Slider) command.context;
			context.GetOrAddComponent<SliderExtraBinder>();
		}
#endif

		private void OnDirectionChanged(Slider.Direction newValue)
		{
			slider.direction = newValue;
		}

		private void OnWholeNumbersChanged(bool newValue)
		{
			slider.wholeNumbers = newValue;
		}
	}
}
