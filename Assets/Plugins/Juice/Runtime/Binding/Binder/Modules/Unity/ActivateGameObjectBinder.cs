using System.Collections.Generic;
using UnityEngine;

namespace Juice
{
	public class ActivateGameObjectBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo setActive = BindingInfo.Variable<bool>();
		[SerializeField] private List<GameObject> targets;

		protected override void Awake()
		{
			base.Awake();

			RegisterVariable<bool>(setActive).OnChanged(Refresh);
		}

		private void Refresh(bool value)
		{
			foreach (GameObject current in targets)
			{
				if (current)
				{
					current.SetActive(value);
				}
			}
		}
	}
}
