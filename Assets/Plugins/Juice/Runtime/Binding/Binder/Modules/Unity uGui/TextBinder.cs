using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	[RequireComponent(typeof(Text))]
	public class TextBinder : VariableBinder<object>
	{
		private Text text;
		
		protected override void Awake()
		{
			base.Awake();
			
			text = GetComponent<Text>();
		}

		protected override void Refresh(object value)
		{
			text.text = value != null ? value.ToString() : string.Empty;
		}
	}
}
