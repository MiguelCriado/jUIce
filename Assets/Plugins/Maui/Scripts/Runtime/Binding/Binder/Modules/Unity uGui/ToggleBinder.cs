using UnityEngine;
using UnityEngine.UI;

namespace Maui
{
	[RequireComponent(typeof(Toggle))]
	public class ToggleBinder : VariableBinder<bool>
	{
		[SerializeField] private BindingInfo commandBindingInfo = new BindingInfo(typeof(IObservableCommand<bool>));

		private Toggle toggle;
		private CommandBinding<bool> commandBinding;

		protected override void Awake()
		{
			base.Awake();

			toggle = GetComponent<Toggle>();
			
			commandBinding = new CommandBinding<bool>(commandBindingInfo, this);
			commandBinding.Property.CanExecute.Changed += CanExecuteChangedHandler;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			
			toggle.onValueChanged.AddListener(ToggleValueChangedHandler);
			
			commandBinding.Bind();
			ToggleValueChangedHandler(toggle.isOn);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			
			toggle.onValueChanged.RemoveListener(ToggleValueChangedHandler);
			
			commandBinding.Unbind();
		}

		protected override void Refresh(bool value)
		{
			toggle.isOn = value;
		}

		private void ToggleValueChangedHandler(bool newValue)
		{
			commandBinding.Property.Execute(newValue);
		}
		
		private void CanExecuteChangedHandler(bool newValue)
		{
			toggle.interactable = newValue;
		}
	}
}