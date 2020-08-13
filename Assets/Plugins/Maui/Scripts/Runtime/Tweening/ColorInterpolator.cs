using UnityEngine;

namespace Maui.Tweening
{
	public class ColorInterpolator : Interpolator<Color>
	{
		private readonly FloatInterpolator floatInterpolator = new FloatInterpolator();

		protected override void OnSetEase(Ease ease)
		{
			base.OnSetEase(ease);
			floatInterpolator.SetEase(ease);
		}

		protected override Color Linear(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InSine(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color OutSine(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InOutSine(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InQuad(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color OutQuad(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InOutQuad(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InCubic(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color OutCubic(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InOutCubic(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InQuart(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color OutQuart(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InOutQuart(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InQuint(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color OutQuint(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InOutQuint(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InExpo(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color OutExpo(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InOutExpo(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InCirc(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color OutCirc(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InOutCirc(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InBack(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color OutBack(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InOutBack(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InElastic(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color OutElastic(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InOutElastic(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InBounce(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color OutBounce(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		protected override Color InOutBounce(Color a, Color b, float t)
		{
			return Interpolate(a, b, t);
		}

		private Color Interpolate(Color a, Color b, float t)
		{
			return new Color(
				floatInterpolator.Evaluate(a.r, b.r, t),
				floatInterpolator.Evaluate(a.g, b.g, t),
				floatInterpolator.Evaluate(a.b, b.b, t),
				floatInterpolator.Evaluate(a.a, b.a, t));
		}
	}
}