using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public class SequentialTransition : Transition
	{
		[SerializeField] private List<Transition> transitions;
		
		public override void Prepare(RectTransform target)
		{
			foreach (Transition current in transitions)
			{
				current.Prepare(target);
			}
		}

		public override async Task Animate(RectTransform target)
		{
			foreach (Transition current in transitions)
			{
				await current.Animate(target);
			}
		}
	}
}