using UnityEngine;

namespace Juice.Tweening
{
	public static class TransformExtensions
	{
		public static Tweener<Vector3> TweenLocalPosition(this Transform transform, Vector3 finalValue, float duration)
		{
			Tweener<Vector3> result = Tween.To(
					() => transform.localPosition,
					x => transform.localPosition = x,
					finalValue,
					duration)
				.SetId(transform);
			return result;
		}

		public static Tweener<Vector3> TweenLocalScale(this Transform transform, Vector3 finalValue, float duration)
		{
			Tweener<Vector3> result = Tween.To(
					() => transform.localScale,
					x => transform.localScale = x,
					finalValue,
					duration)
				.SetId(transform);
			return result;
		}
	}
}