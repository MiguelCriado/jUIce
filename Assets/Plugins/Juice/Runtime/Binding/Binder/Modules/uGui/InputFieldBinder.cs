using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(InputField))]
	public class InputFieldBinder : CommandBinder<string>
	{
		[SerializeField] private BindingInfo text = new BindingInfo(typeof(IReadOnlyObservableVariable<object>));
		[SerializeField] private bool sendOnValueChanged;

		protected override string BindingInfoName { get; } = "OnValueChanged Command";

		private InputField inputField;

		protected override void Awake()
		{
			base.Awake();

			inputField = GetComponent<InputField>();

			RegisterVariable<object>(text).OnChanged(OnTextBindingPropertyChanged);
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
			inputField.onEndEdit.AddListener(OnInputFieldEditionEnded);
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			inputField.onValueChanged.RemoveListener(OnInputFieldValueChanged);
			inputField.onEndEdit.RemoveListener(OnInputFieldEditionEnded);
		}

		protected override void OnCommandCanExecuteChanged(bool newValue)
		{
			inputField.interactable = newValue;
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/InputField/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			InputField context = (InputField) command.context;
			context.GetOrAddComponent<InputFieldBinder>();
		}
#endif

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
