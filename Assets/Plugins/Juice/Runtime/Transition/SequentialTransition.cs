using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public class SequentialTransition : ComponentTransition
	{
		[SerializeField] private List<ComponentTransition> transitions;

		protected override void PrepareInternal(RectTransform target)
		{
			foreach (ComponentTransition current in transitions)
			{
				current.Prepare(target);
			}
		}

		protected override async Task AnimateInternal(RectTransform target)
		{
			foreach (ComponentTransition current in transitions)
			{
				await current.Animate(target);
			}
		}

		protected override void CleanupInternal(RectTransform target)
		{
			foreach (ComponentTransition current in transitions)
			{
				current.Cleanup(target);
			}
		}
	}
}