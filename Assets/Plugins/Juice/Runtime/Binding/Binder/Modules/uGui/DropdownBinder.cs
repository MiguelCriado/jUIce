using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(Dropdown))]
	public class DropdownBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo onValueChanged = BindingInfo.Command<int>();

		private Dropdown dropdown;

		protected override void Awake()
		{
			base.Awake();

			dropdown = GetComponent<Dropdown>();

			RegisterCommand<int>(onValueChanged)
				.AddExecuteTrigger(dropdown.onValueChanged)
				.OnCanExecuteChanged(OnCommandCanExecuteChanged);
		}

		protected virtual void OnCommandCanExecuteChanged(bool newValue)
		{
			dropdown.interactable = newValue;
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/Dropdown/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Dropdown context = (Dropdown) command.context;
			context.GetOrAddComponent<DropdownBinder>();
		}
#endif
	}
}
