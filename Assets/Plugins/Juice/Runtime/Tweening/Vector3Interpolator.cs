using UnityEngine;

namespace Juice.Tweening
{
	public class Vector3Interpolator : Interpolator<Vector3>
	{
		protected override Vector3 Evaluate(Vector3 a, Vector3 b, float t, EaseDelegate easeFunction)
		{
			return new Vector3(
				easeFunction(a.x, b.x, t),
				easeFunction(a.y, b.y, t),
				easeFunction(a.z, b.z, t));
		}
	}
}