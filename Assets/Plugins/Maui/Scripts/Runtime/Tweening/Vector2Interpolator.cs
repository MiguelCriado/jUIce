using UnityEngine;

namespace Maui.Tweening
{
	public class Vector2Interpolator : Interpolator<Vector2>
	{
		protected override Vector2 Evaluate(Vector2 a, Vector2 b, float t, EaseDelegate easeFunction)
		{
			return new Vector2(
				easeFunction(a.x, b.x, t),
				easeFunction(a.y, b.y, t));
		}
	}
}