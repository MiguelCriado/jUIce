using UnityEngine;

namespace Maui.Tweening
{
	public class Vector3Interpolator : Interpolator<Vector3>
	{
		private readonly FloatInterpolator floatInterpolator = new FloatInterpolator();

		protected override void OnSetEase(Ease ease)
		{
			base.OnSetEase(ease);
			floatInterpolator.SetEase(ease);
		}

		protected override Vector3 Linear(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InSine(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 OutSine(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InOutSine(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InQuad(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 OutQuad(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InOutQuad(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InCubic(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 OutCubic(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InOutCubic(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InQuart(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 OutQuart(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InOutQuart(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InQuint(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 OutQuint(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InOutQuint(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InExpo(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 OutExpo(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InOutExpo(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InCirc(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 OutCirc(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InOutCirc(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InBack(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 OutBack(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InOutBack(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InElastic(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 OutElastic(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InOutElastic(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InBounce(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 OutBounce(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Vector3 InOutBounce(Vector3 a, Vector3 b, float t)
		{
			return Interpolate(a, b, t);
		}

		private Vector3 Interpolate(Vector3 a, Vector3 b, float t)
		{
			return new Vector3(
				floatInterpolator.Evaluate(a.x, b.x, t),
				floatInterpolator.Evaluate(a.y, b.y, t),
				floatInterpolator.Evaluate(a.z, b.z, t));
		}
	}
}