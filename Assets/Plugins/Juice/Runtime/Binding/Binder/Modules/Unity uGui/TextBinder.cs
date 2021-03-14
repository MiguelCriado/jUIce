using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	[RequireComponent(typeof(Text))]
	public class TextBinder : VariableBinder<object>
	{
		protected override string BindingInfoName { get; } = "Text";

		private Text textComponent;

		protected override void Awake()
		{
			base.Awake();

			textComponent = GetComponent<Text>();
		}

		protected override void Refresh(object value)
		{
			textComponent.text = value != null ? value.ToString() : string.Empty;
		}
	}
}
