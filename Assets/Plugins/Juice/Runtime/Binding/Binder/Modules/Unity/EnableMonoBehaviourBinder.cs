using System.Collections.Generic;
using UnityEngine;

namespace Juice
{
	public class EnableMonoBehaviourBinder : ComponentBinder
	{
		[SerializeField, Rename(nameof(BindingInfoName))]
		private BindingInfo bindingInfo = BindingInfo.Variable<bool>();
		[SerializeField] private List<MonoBehaviour> targets;

		private string BindingInfoName { get; } = "Enabled";

		protected override void Awake()
		{
			base.Awake();

			RegisterVariable<bool>(bindingInfo).OnChanged(Refresh);
		}

		private void Refresh(bool value)
		{
			foreach (MonoBehaviour current in targets)
			{
				if (current)
				{
					current.enabled = value;
				}
			}
		}
	}
}
