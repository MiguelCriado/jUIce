using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(Toggle))]
	public class ToggleBinder : VariableBinder<bool>
	{
		[SerializeField] private BindingInfo onValueChangedCommand = new BindingInfo(typeof(IObservableCommand<bool>));

		protected override string BindingInfoName { get; } = "Is On";

		private Toggle toggle;
		private CommandBinding<bool> onValueChangedCommandBinding;

		protected override void Awake()
		{
			base.Awake();

			toggle = GetComponent<Toggle>();

			onValueChangedCommandBinding = new CommandBinding<bool>(onValueChangedCommand, this);
			onValueChangedCommandBinding.Property.CanExecute.Changed += OnCanExecuteChanged;
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			toggle.onValueChanged.AddListener(OnToggleValueChanged);

			onValueChangedCommandBinding.Bind();
			OnToggleValueChanged(toggle.isOn);
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			toggle.onValueChanged.RemoveListener(OnToggleValueChanged);

			onValueChangedCommandBinding.Unbind();
		}

		protected override void Refresh(bool value)
		{
			toggle.isOn = value;
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/Toggle/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Toggle context = (Toggle) command.context;
			context.GetOrAddComponent<ToggleBinder>();
		}
#endif

		private void OnToggleValueChanged(bool newValue)
		{
			onValueChangedCommandBinding.Property.Execute(newValue);
		}

		private void OnCanExecuteChanged(bool newValue)
		{
			toggle.interactable = newValue;
		}
	}
}
