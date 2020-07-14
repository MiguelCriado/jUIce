using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Maui
{
	[RequireComponent(typeof(Text))]
	public class TextBinder : Binder<string>
	{
		private Text text;

		[SerializeField] private List<BindingInfo> bindingList;

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
