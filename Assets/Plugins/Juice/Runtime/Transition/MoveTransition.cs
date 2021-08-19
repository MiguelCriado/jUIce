using System.Threading.Tasks;
using Juice.Tweening;
using UnityEngine;

namespace Juice
{
	public class MoveTransition : ComponentTransition
	{
		[SerializeField] private Vector2 anchoredOrigin;
		[SerializeField] private Vector2 anchoredDestiny;
		[SerializeField] private float duration = 0.3f;
		[SerializeField] private Ease ease = Ease.InOutSine;

		protected override void PrepareInternal(RectTransform target)
		{
			Tween.Kill(target);
			target.anchoredPosition = anchoredOrigin;
		}

		protected override async Task AnimateInternal(RectTransform target)
		{
			bool isTweenDone = false;

			target.TweenAnchoredPosition(anchoredDestiny, duration)
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
