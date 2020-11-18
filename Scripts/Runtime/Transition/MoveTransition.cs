using System.Threading.Tasks;
using Juice.Tweening;
using UnityEngine;

namespace Juice
{
	public class MoveTransition : Transition
	{
		[SerializeField] private Vector2 anchoredOrigin;
		[SerializeField] private Vector2 anchoredDestiny;
		[SerializeField] private float duration = 0.3f;
		[SerializeField] private Ease ease = Ease.InOutSine;

		public override void PrepareForAnimation(RectTransform target)
		{
			Tween.Kill(target);
			target.anchoredPosition = anchoredOrigin;
		}

		public override async Task Animate(RectTransform target)
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
	}
}