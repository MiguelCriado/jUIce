using TMPro;
using UnityEngine;

namespace Maui
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TextMeshProUGUIBinder : VariableBinder<object>
	{
		private TextMeshProUGUI text;

		protected override void Awake()
		{
			base.Awake();
			
			text = GetComponent<TextMeshProUGUI>();
		}

		protected override void Refresh(object value)
		{
			text.text = value != null ? value.ToString() : string.Empty;
		}
	}
}