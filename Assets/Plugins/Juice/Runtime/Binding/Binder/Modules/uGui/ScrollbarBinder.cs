using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(Scrollbar))]
	public class ScrollbarBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo onValueChanged = BindingInfo.Command<float>();
		[SerializeField] private BindingInfo direction = BindingInfo.Variable<Scrollbar.Direction>();
		[SerializeField] private BindingInfo value = BindingInfo.Variable<float>();
		[SerializeField] private BindingInfo size = BindingInfo.Variable<float>();
		[SerializeField] private BindingInfo numberOfSteps = BindingInfo.Variable<int>();

		private Scrollbar scrollbar;

		protected override void Awake()
		{
			base.Awake();

			scrollbar = GetComponent<Scrollbar>();

			RegisterCommand<float>(onValueChanged)
				.AddExecuteTrigger(scrollbar.onValueChanged)
				.OnCanExecuteChanged(OnCommandCanExecuteChanged);
			RegisterVariable<Scrollbar.Direction>(direction).OnChanged(OnDirectionChanged);
			RegisterVariable<float>(value).OnChanged(OnValueChanged);
			RegisterVariable<float>(size).OnChanged(OnSizeChanged);
			RegisterVariable<int>(numberOfSteps).OnChanged(OnNumberOfStepsChanged);
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/Scrollbar/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Scrollbar context = (Scrollbar) command.context;
			context.GetOrAddComponent<ScrollbarBinder>();
		}
#endif

		private void OnCommandCanExecuteChanged(bool newValue)
		{
			scrollbar.interactable = newValue;
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
