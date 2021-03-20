using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	[RequireComponent(typeof(InputField))]
	public class InputFieldBinder : CommandBinder<string>
	{
		[SerializeField] private BindingInfo text = new BindingInfo(typeof(IReadOnlyObservableVariable<object>));
		[SerializeField] private bool sendOnValueChanged;

		protected override string BindingInfoName { get; } = "OnValueChanged Command";

		private VariableBinding<object> textBinding;
		private InputField inputField;

		protected override void Reset()
		{
			base.Reset();

			text = new BindingInfo(typeof(IReadOnlyObservableVariable<object>));
		}

		protected override void Awake()
		{
			base.Awake();

			textBinding = new VariableBinding<object>(text, this);
			textBinding.Property.Changed += OnTextBindingPropertyChanged;

			inputField = GetComponent<InputField>();
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			textBinding.Bind();

			inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
			inputField.onEndEdit.AddListener(OnInputFieldEditionEnded);
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			textBinding.Unbind();

			inputField.onValueChanged.RemoveListener(OnInputFieldValueChanged);
			inputField.onEndEdit.RemoveListener(OnInputFieldEditionEnded);
		}

		protected override void OnCommandCanExecuteChanged(bool newValue)
		{
			inputField.interactable = newValue;
		}

		private void OnTextBindingPropertyChanged(object newValue)
		{
			inputField.text = newValue != null ? newValue.ToString() : string.Empty;
		}

		private void OnInputFieldValueChanged(string newValue)
		{
			if (sendOnValueChanged)
			{
				ExecuteCommand(newValue);
			}
		}

		private void OnInputFieldEditionEnded(string newValue)
		{
			ExecuteCommand(newValue);
		}
	}
}
