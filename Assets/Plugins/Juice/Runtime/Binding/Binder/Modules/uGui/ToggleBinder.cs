using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(Toggle))]
	public class ToggleBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo isOn = BindingInfo.Variable<bool>();
		[SerializeField] private BindingInfo onValueChanged = BindingInfo.Command<bool>();

		private Toggle toggle;

		protected override void Awake()
		{
			base.Awake();

			toggle = GetComponent<Toggle>();

			RegisterVariable<bool>(isOn).OnChanged(Refresh);

			RegisterCommand<bool>(onValueChanged)
				.AddExecuteTrigger(toggle.onValueChanged)
				.OnCanExecuteChanged(OnCanExecuteChanged);
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/Toggle/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Toggle context = (Toggle) command.context;
			context.GetOrAddComponent<ToggleBinder>();
		}
#endif

		private void Refresh(bool value)
		{
			toggle.isOn = value;
		}

		private void OnCanExecuteChanged(bool newValue)
		{
			toggle.interactable = newValue;
		}
	}
}
