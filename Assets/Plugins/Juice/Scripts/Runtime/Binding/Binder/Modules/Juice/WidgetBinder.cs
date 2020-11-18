using UnityEngine;

namespace Juice
{
	[RequireComponent(typeof(Widget))]
	public class WidgetBinder : VariableBinder<bool>
	{
		private Widget widget;

		protected override void Awake()
		{
			base.Awake();

			widget = GetComponent<Widget>();
		}

		protected override void Refresh(bool value)
		{
			if (value)
			{
				widget.Show();
			}
			else
			{
				widget.Hide();
			}
		}
	}
}