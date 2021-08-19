using System.Threading.Tasks;
using Juice.Tweening;
using UnityEngine;

namespace Juice
{
	public class ScaleTransition : ComponentTransition
	{
		[SerializeField] private Vector3 origin;
		[SerializeField] private Vector3 destiny;
		[SerializeField] private float duration;
		[SerializeField] private Ease ease;

		protected override void PrepareInternal(RectTransform target)
		{
			Tween.Kill(target);
			target.localScale = origin;
		}

		protected override async Task AnimateInternal(RectTransform target)
		{
			bool isTweenDone = false;

			target.TweenLocalScale(destiny, duration)
				.SetEase(ease)
				.Completed += t => isTweenDone = true;

			while (isTweenDone == false)
			{
				await Task.Yield();
			}
		}

		protected override void CleanupInternal(RectTransform target)
		{
			
		}
	}
}