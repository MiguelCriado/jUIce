using UnityEngine;

namespace Maui.Tweening
{
	public class ColorInterpolator : Interpolator<Color>
	{
		protected override Color Evaluate(Color a, Color b, float t, EaseDelegate easeFunction)
		{
			return new Color(
				easeFunction(a.r, b.r, t),
				easeFunction(a.g, b.g, t),
				easeFunction(a.b, b.b, t),
				easeFunction(a.a, b.a, t));
		}
	}
}