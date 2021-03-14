using TMPro;
using UnityEngine;

namespace Juice
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TextMeshProUGUIBinder : VariableBinder<object>
	{
		protected override string BindingInfoName { get; } = "Text";

		private TextMeshProUGUI textComponent;

		protected override void Awake()
		{
			base.Awake();

			textComponent = GetComponent<TextMeshProUGUI>();
		}

		protected override void Refresh(object value)
		{
			textComponent.text = value != null ? value.ToString() : string.Empty;
		}
	}
}
