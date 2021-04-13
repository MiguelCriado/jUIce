using Juice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Juice
{
	[RequireComponent(typeof(Button))]
	public class ButtonBinder : CommandBinder
	{
		protected override string BindingInfoName { get; } = "OnClick Command";

		private Button button;

		protected override void Awake()
		{
			base.Awake();

			button = GetComponent<Button>();
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			button.onClick.AddListener(OnButtonClicked);
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			button.onClick.RemoveListener(OnButtonClicked);
		}

		protected override void OnCommandCanExecuteChanged(bool newValue)
		{
			button.interactable = newValue;
		}

#if UNITY_EDITOR
		[MenuItem("CONTEXT/Button/Add Binder")]
		private static void AddBinder(MenuCommand command)
		{
			Button context = (Button) command.context;
			context.GetOrAddComponent<ButtonBinder>();
		}
#endif

		private void OnButtonClicked()
		{
			ExecuteCommand();
		}
	}
}
