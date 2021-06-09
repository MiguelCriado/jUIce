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

		private Button button;

		protected override void Awake()
		{
			base.Awake();

			button = GetComponent<Button>();

			RegisterCommand(onClick)
				.AddExecuteTrigger(button.onClick)
				.OnCanExecuteChanged(OnCommandCanExecuteChanged);
		}

		protected virtual void OnCommandCanExecuteChanged(bool newValue)
		{
			button.interactable = newValue;
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/Button/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Button context = (Button) command.context;
			context.GetOrAddComponent<ButtonBinder>();
		}
#endif
	}
}
