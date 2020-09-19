using System.Threading.Tasks;
using Maui.Tweening;
using Maui.Utils;
using UnityEngine;

namespace Maui
{
	public class FadeTransition : Transition
	{
		public enum FadeType
		{
			In,
			Out
		}

		[SerializeField] private FadeType fadeType = FadeType.In;
		[SerializeField] private float duration = 0.3f;
		[SerializeField] private Ease ease = Ease.InOutSine;
		
		
		public override void PrepareForAnimation(RectTransform target)
		{
			if (fadeType == FadeType.In)
			{
				target.GetOrAddComponent<CanvasGroup>().alpha = 0;
			}
			else
			{
				target.GetOrAddComponent<CanvasGroup>().alpha = 1;
			}
		}

		public override async Task Animate(RectTransform target)
		{
			float valueTarget = fadeType == FadeType.In ? 1 : 0;
			CanvasGroup canvasGroup = target.GetOrAddComponent<CanvasGroup>();
			bool isTweenDone = false;

			Tween.To(() => 
					canvasGroup.alpha,
					x => canvasGroup.alpha = x,
					valueTarget,
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