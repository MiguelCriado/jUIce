using System.Collections.Generic;
using Juice.Utils;
using UnityEngine;

namespace Juice
{
	public class WidgetBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo show = BindingInfo.Variable<bool>();
		[SerializeField] private List<Widget> targets;

		protected override void Awake()
		{
			base.Awake();

			RegisterVariable<bool>(show)
				.OnChanged(Refresh);
		}

		private void Refresh(bool value)
		{
			if (value)
			{
				ShowTargets();
			}
			else
			{
				HideTargets();
			}
		}

		private void ShowTargets()
		{
			foreach (Widget current in targets)
			{
				if (current)
				{
					current.Show().RunAndForget();
				}
			}
		}

		private void HideTargets()
		{
			foreach (Widget current in targets)
			{
				if (current)
				{
					current.Hide().RunAndForget();
				}
			}
		}
	}
}
