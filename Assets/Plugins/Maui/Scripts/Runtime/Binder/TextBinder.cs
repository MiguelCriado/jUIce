using UnityEngine;
using UnityEngine.UI;

namespace Maui
{
	[RequireComponent(typeof(Text))]
	public class TextBinder : Binder<string>
	{
		private Text text;

		private void Awake()
		{
			text = GetComponent<Text>();
		}

		protected override void Refresh(string value)
		{
			this.

			text.text = value;
		}
	}
}
