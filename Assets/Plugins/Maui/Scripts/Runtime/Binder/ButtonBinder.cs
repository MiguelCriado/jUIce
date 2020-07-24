using UnityEngine;
using UnityEngine.UI;

namespace Maui
{
	[RequireComponent((typeof(Button)))]
	public class ButtonBinder : CommandBinder
	{
		private Button button;

		protected override void Awake()
		{
			base.Awake();

			button = GetComponent<Button>();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			
			button.onClick.AddListener(ButtonClickedHandler);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			
			button.onClick.RemoveListener(ButtonClickedHandler);
		}

		protected override void OnCommandCanExecuteChanged(bool newValue)
		{
			button.interactable = newValue;
		}

		private void ButtonClickedHandler()
		{
			ExecuteCommand();
		}
	}
}