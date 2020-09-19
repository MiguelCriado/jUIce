using System.Threading.Tasks;
using Maui.Tweening;
using UnityEngine;

namespace Maui
{
	public class ScaleTransition : Transition
	{
		[SerializeField] private Vector3 origin;
		[SerializeField] private Vector3 destiny;
		[SerializeField] private float duration;
		[SerializeField] private Ease ease;

		public override void PrepareForAnimation(RectTransform target)
		{
			target.localScale = origin;
		}

		public override async Task Animate(RectTransform target)
		{
			bool isTweenDone = false;

			Tween.To(
					() => target.localScale,
					x => target.localScale = x,
					destiny,
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