using System.Threading.Tasks;
using Maui.Tweening;
using UnityEngine;

namespace Maui
{
	public class MoveTransition : Transition
	{
		[SerializeField] private Vector2 anchoredOrigin;
		[SerializeField] private Vector2 anchoredDestiny;
		[SerializeField] private float duration = 0.3f;
		[SerializeField] private Ease ease = Ease.InOutSine;

		public override void PrepareForAnimation(RectTransform target)
		{
			target.anchoredPosition = anchoredOrigin;
		}

		public override async Task Animate(RectTransform target)
		{
			bool isTweenDone = false;

			Tween.To(
					() => target.anchoredPosition,
					x => target.anchoredPosition = x,
					anchoredDestiny,
					duration)
				.SetEase(ease)
				.Completed += () => isTweenDone = true;

			while (isTweenDone == false)
			{
				await Task.Yield();
			}
		}
	}
}