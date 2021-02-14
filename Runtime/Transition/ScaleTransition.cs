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

		public override void Prepare(RectTransform target)
		{
			Tween.Kill(target);
			target.localScale = origin;
		}

		public override async Task Animate(RectTransform target)
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
	}
}