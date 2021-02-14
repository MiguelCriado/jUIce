using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public class SequentialTransition : ComponentTransition
	{
		[SerializeField] private List<ComponentTransition> transitions;
		
		public override void Prepare(RectTransform target)
		{
			foreach (ComponentTransition current in transitions)
			{
				current.Prepare(target);
			}
		}

		public override async Task Animate(RectTransform target)
		{
			foreach (ComponentTransition current in transitions)
			{
				await current.Animate(target);
			}
		}
	}
}