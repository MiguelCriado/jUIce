using UnityEngine;
using UnityEngine.UI;

namespace Maui
{
	[RequireComponent(typeof(InputField))]
	public class InputFieldBinder : CommandBinder<string>
	{
		[SerializeField] private BindingInfo textValueBinding = new BindingInfo(typeof(IReadOnlyObservableVariable<object>));
		[SerializeField] private bool sendOnValueChanged;

		private VariableBinding<object> textBinding;
		private InputField inputField;

		protected override void Reset()
		{
			base.Reset();
			
			textValueBinding = new BindingInfo(typeof(IReadOnlyObservableVariable<object>));
		}

		protected override void Awake()
		{
			base.Awake();

			textBinding = new VariableBinding<object>(textValueBinding, this);
			textBinding.Property.Changed += ContentBoundPropertyChangedHandler;
			
			inputField = GetComponent<InputField>();
		}

		private void ContentBoundPropertyChangedHandler(object newValue)
		{
			inputField.text = newValue.ToString();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			
			textBinding.Bind();
			
			inputField.onValueChanged.AddListener(OnValueChangedHandler);
			inputField.onEndEdit.AddListener(OnEndEditHandler);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			
			textBinding.Unbind();
			
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