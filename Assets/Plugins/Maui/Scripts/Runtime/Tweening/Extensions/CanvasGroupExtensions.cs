using UnityEngine;

namespace Maui.Tweening
{
	public static class CanvasGroupExtensions
	{
		public static Tweener<float> Fade(this CanvasGroup canvasGroup, float finalValue, float duration)
		{
			Tweener<float> result = Tween.To(
					() => canvasGroup.alpha,
					x => canvasGroup.alpha = x,
					finalValue,
					duration)
				.SetId(canvasGroup);
			return result;
		}
	}
}