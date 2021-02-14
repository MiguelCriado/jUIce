using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public class ParallelTransition : ComponentTransition
	{
		[SerializeField] private List<ComponentTransition> transitions;

		private Task[] tasks;

		protected virtual void Awake()
		{
			tasks = new Task[transitions.Count];
		}

		public override void Prepare(RectTransform target)
		{
			foreach (ComponentTransition current in transitions)
			{
				current.Prepare(target);
			}
		}

		public override async Task Animate(RectTransform target)
		{
			for (int i = 0; i < transitions.Count; i++)
			{
				tasks[i] = transitions[i].Animate(target);
			}

			await Task.WhenAll(tasks);
		}
	}
}
