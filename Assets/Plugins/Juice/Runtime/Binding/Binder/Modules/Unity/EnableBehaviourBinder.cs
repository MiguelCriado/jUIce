using System.Collections.Generic;
using UnityEngine;

namespace Juice
{
	public class EnableBehaviourBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo isEnabled = BindingInfo.Variable<bool>();
		[SerializeField] private List<Behaviour> targets = default;

		protected override void Awake()
		{
			base.Awake();

			RegisterVariable<bool>(isEnabled).OnChanged(Refresh);
		}

		private void Refresh(bool value)
		{
			foreach (Behaviour current in targets)
			{
				if (current)
				{
					current.enabled = value;
				}
			}
		}
	}
}
