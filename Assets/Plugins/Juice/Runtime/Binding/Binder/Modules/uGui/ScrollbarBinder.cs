using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

		protected override void Awake()
		{
			base.Awake();

			scrollbar = GetComponent<Scrollbar>();
			scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);

			RegisterVariable<Scrollbar.Direction>(direction).OnChanged(OnDirectionChanged);
			RegisterVariable<float>(value).OnChanged(OnValueChanged);
			RegisterVariable<float>(size).OnChanged(OnSizeChanged);
			RegisterVariable<int>(numberOfSteps).OnChanged(OnNumberOfStepsChanged);
		}

		protected override void OnCommandCanExecuteChanged(bool newValue)
		{
			scrollbar.interactable = newValue;
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/Scrollbar/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Scrollbar context = (Scrollbar) command.context;
			context.GetOrAddComponent<ScrollbarBinder>();
		}
#endif

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
