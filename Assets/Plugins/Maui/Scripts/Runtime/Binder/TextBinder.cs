using UnityEngine;
using UnityEngine.UI;

namespace Maui
{
	[RequireComponent(typeof(Text))]
	public class TextBinder : VariableBinder<string>
	{
		private Text text;
		
		protected override void Awake()
		{
			base.Awake();
			
			text = GetComponent<Text>();
		}

		protected override void Refresh(string value)
		{
			text.text = value;
		}
	}
}
