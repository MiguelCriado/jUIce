using UnityEngine;

namespace Juice.Tweening
{
	public static class RectTransformExtensions
	{
		public static Tweener<Vector2> TweenAnchoredPosition(this RectTransform rectTransform, Vector2 finalValue, float duration)
		{
			Tweener<Vector2> result = Tween.To(
					() => rectTransform.anchoredPosition,
					x => rectTransform.anchoredPosition = x,
					finalValue,
					duration)
				.SetId(rectTransform);
			return result;
		}
	}
}