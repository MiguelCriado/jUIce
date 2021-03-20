using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(Dropdown))]
	public class DropdownBinder : CommandBinder<int>
	{
		protected override string BindingInfoName { get; } = "OnValueChanged Command";

		private Dropdown dropdown;

		protected override void Awake()
		{
			base.Awake();

			dropdown = GetComponent<Dropdown>();
			dropdown.onValueChanged.AddListener(OnValueChanged);
		}

		protected override void OnCommandCanExecuteChanged(bool newValue)
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

		private void OnValueChanged(int newValue)
		{
			ExecuteCommand(newValue);
		}
	}
}
