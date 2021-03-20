using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(Slider))]
	public class SliderExtraBinder : MonoBehaviour, IBinder<Slider.Direction>
	{
		[SerializeField] private BindingInfo direction = new BindingInfo(typeof(IReadOnlyObservableVariable<Slider.Direction>));
		[SerializeField] private BindingInfo wholeNumbers = new BindingInfo(typeof(IReadOnlyObservableVariable<bool>));

		private Slider slider;
		private VariableBinding<Slider.Direction> directionBinding;
		private VariableBinding<bool> wholeNumbersBinding;

		protected virtual void Awake()
		{
			slider = GetComponent<Slider>();

			directionBinding = new VariableBinding<Slider.Direction>(direction, this);
			directionBinding.Property.Changed += OnDirectionChanged;

			wholeNumbersBinding = new VariableBinding<bool>(wholeNumbers, this);
			wholeNumbersBinding.Property.Changed += OnWholeNumbersChanged;
		}

		protected virtual void OnEnable()
		{
			directionBinding.Bind();
			wholeNumbersBinding.Bind();
		}

		protected virtual void OnDisable()
		{
			directionBinding.Bind();
			wholeNumbersBinding.Unbind();
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
