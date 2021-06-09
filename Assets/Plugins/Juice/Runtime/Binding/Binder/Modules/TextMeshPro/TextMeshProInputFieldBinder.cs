using Juice.Utils;
using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(TMP_InputField))]
	public class TextMeshProInputFieldBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo onValueChanged = BindingInfo.Command<string>();
		[SerializeField] private BindingInfo text = BindingInfo.Variable<object>();
		[SerializeField] private bool sendOnValueChanged;

		private TMP_InputField inputField;
		private CommandBinding<string> onValueChangedBinding;

		protected override void Awake()
		{
			base.Awake();

			inputField = GetComponent<TMP_InputField>();

			onValueChangedBinding = RegisterCommand<string>(onValueChanged)
				.OnCanExecuteChanged(OnCommandCanExecuteChanged)
				.GetBinding();
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

#if UNITY_EDITOR
		[MenuItem("CONTEXT/TMP_InputField/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			TMP_InputField context = (TMP_InputField) command.context;
			context.GetOrAddComponent<TextMeshProInputFieldBinder>();
		}
#endif

		private void OnCommandCanExecuteChanged(bool newValue)
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

		private void ExecuteCommand(string newValue)
		{
			onValueChangedBinding.Property.Execute(newValue);
		}
	}
}
