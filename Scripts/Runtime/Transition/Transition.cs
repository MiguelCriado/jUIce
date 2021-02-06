using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public abstract class Transition : MonoBehaviour, ITransition
	{
		private class NullTransition : ITransition
		{
			public void Prepare(RectTransform target)
			{

			}

			public Task Animate(RectTransform target)
			{
				return Task.CompletedTask;
			}

			public void Cleanup(RectTransform target)
			{

			}
		}

		public static readonly ITransition Null = new NullTransition();

		public virtual void Prepare(RectTransform target)
		{

		}

		public abstract Task Animate(RectTransform target);

		public virtual void Cleanup(RectTransform target)
		{

		}
	}
}
