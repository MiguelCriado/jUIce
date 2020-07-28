using UnityEngine;
using UnityEngine.UI;

namespace Maui
{
	[RequireComponent(typeof(InputField))]
	public class InputFieldBinder : CommandBinder<string>
	{
		[SerializeField] private bool sendOnValueChanged;

		private InputField inputField;

		protected override void Awake()
		{
			base.Awake();

			inputField = GetComponent<InputField>();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			
			inputField.onValueChanged.AddListener(OnValueChangedHandler);
			inputField.onEndEdit.AddListener(OnEndEditHandler);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			
			inputField.onValueChanged.RemoveListener(OnValueChangedHandler);
			inputField.onEndEdit.RemoveListener(OnEndEditHandler);
		}

		protected override void OnCommandCanExecuteChanged(bool newValue)
		{
			inputField.interactable = newValue;
		}

		private void OnValueChangedHandler(string newValue)
		{
			if (sendOnValueChanged)
			{
				ExecuteCommand(newValue);
			}
		}

		private void OnEndEditHandler(string newValue)
		{
			ExecuteCommand(newValue);
		}
	}
}