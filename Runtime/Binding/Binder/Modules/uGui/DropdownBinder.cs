using UnityEngine;
using UnityEngine.UI;

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

		private void OnValueChanged(int newValue)
		{
			ExecuteCommand(newValue);
		}
	}
}
