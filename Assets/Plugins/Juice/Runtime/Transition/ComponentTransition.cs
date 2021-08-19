using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public abstract class ComponentTransition : MonoBehaviour, ITransition
	{
		public RectTransform OverrideTarget => overrideTarget;
		
		[SerializeField] private RectTransform overrideTarget;

		public void Prepare(RectTransform target)
		{
			PrepareInternal(GetFinalTarget(target));
		}

		public Task Animate(RectTransform target)
		{
			return AnimateInternal(GetFinalTarget(target));
		}

		public void Cleanup(RectTransform target)
		{
			CleanupInternal(GetFinalTarget(target));
		}

		protected abstract void PrepareInternal(RectTransform target);

		protected abstract Task AnimateInternal(RectTransform target);

		protected abstract void CleanupInternal(RectTransform target);

		private RectTransform GetFinalTarget(RectTransform target)
		{
			return overrideTarget ? overrideTarget : target;
		}
	}
}
