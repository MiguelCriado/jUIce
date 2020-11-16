using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Maui
{
	public class SequentialTransition : Transition
	{
		[SerializeField] private List<Transition> transitions;
		
		public override void PrepareForAnimation(RectTransform target)
		{
			foreach (Transition current in transitions)
			{
				current.PrepareForAnimation(target);
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