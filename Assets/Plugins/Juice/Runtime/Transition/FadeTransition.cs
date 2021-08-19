using System.Threading.Tasks;
using Juice.Tweening;
using Juice.Utils;
using UnityEngine;

namespace Juice
{
	public class FadeTransition : ComponentTransition
	{
		public enum FadeType
		{
			In,
			Out
		}

		internal FadeType FadeTypeInternal { get => fadeType; set => fadeType = value; }

		[SerializeField] private FadeType fadeType = FadeType.In;
		[SerializeField] private float duration = 0.3f;
		[SerializeField] private Ease ease = Ease.InOutSine;

		protected override void PrepareInternal(RectTransform target)
		{
			CanvasGroup canvasGroup = target.GetOrAddComponent<CanvasGroup>();
			Tween.Kill(canvasGroup);

			if (fadeType == FadeType.In)
			{
				canvasGroup.alpha = 0;
			}
			else
			{
				canvasGroup.alpha = 1;
			}
		}

		protected override async Task AnimateInternal(RectTransform target)
		{
			float valueTarget = fadeType == FadeType.In ? 1 : 0;
			CanvasGroup canvasGroup = target.GetOrAddComponent<CanvasGroup>();
			bool isTweenDone = false;

			canvasGroup.Fade(valueTarget, duration)
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
