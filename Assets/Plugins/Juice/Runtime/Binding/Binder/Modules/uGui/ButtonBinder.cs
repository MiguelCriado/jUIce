using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(Button))]
	public class ButtonBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo onClick = BindingInfo.Command();
		[SerializeField] private BindingInfo interactable = BindingInfo.Variable<bool>();

		private Button button;
		private VariableBinding<bool> interactableBinding;

		protected override void Awake()
		{
			base.Awake();

			button = GetComponent<Button>();

			RegisterCommand(onClick)
				.AddExecuteTrigger(button.onClick)
				.OnCanExecuteChanged(OnCommandCanExecuteChanged);

			interactableBinding = RegisterVariable<bool>(interactable)
				.OnChanged(OnInteractableChanged)
				.GetBinding();
		}

		protected virtual void OnCommandCanExecuteChanged(bool newValue)
		{
			button.interactable = 
				interactableBinding.IsBound && interactableBinding.Property.HasValue ?
					interactableBinding.Property.Value 
					: newValue;
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/Button/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Button context = (Button) command.context;
			context.GetOrAddComponent<ButtonBinder>();
		}
#endif

		private void OnInteractableChanged(bool newValue)
		{
			button.interactable = newValue;
		}
	}
}
