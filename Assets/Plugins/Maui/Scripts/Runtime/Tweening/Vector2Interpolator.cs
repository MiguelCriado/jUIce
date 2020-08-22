using UnityEngine;

namespace Maui.Tweening
{
	public class Vector2Interpolator : Interpolator<Vector2>
	{
		readonly FloatInterpolator floatInterpolator = new FloatInterpolator();

		protected override void OnSetEase(Ease ease)
		{
			base.OnSetEase(ease);
			floatInterpolator.SetEase(ease);
		}

		protected override Vector2 Linear(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InSine(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 OutSine(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InOutSine(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InQuad(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 OutQuad(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InOutQuad(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InCubic(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 OutCubic(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InOutCubic(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InQuart(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 OutQuart(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InOutQuart(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InQuint(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 OutQuint(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InOutQuint(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InExpo(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 OutExpo(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InOutExpo(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InCirc(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 OutCirc(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InOutCirc(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InBack(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 OutBack(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InOutBack(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InElastic(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 OutElastic(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InOutElastic(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InBounce(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 OutBounce(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector2 InOutBounce(Vector2 a, Vector2 b, float t)
		{
			return Interpolate(a, b, t);
		}

		private Vector2 Interpolate(Vector2 a, Vector2 b, float t)
		{
			return new Vector2(
				floatInterpolator.Evaluate(a.x, b.x, t),
				floatInterpolator.Evaluate(a.y, b.y, t));
		}
	}
}