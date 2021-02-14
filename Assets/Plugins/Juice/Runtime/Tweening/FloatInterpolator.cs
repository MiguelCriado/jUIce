namespace Juice.Tweening
{
	public class FloatInterpolator : Interpolator<float>
	{
		protected override float Evaluate(float a, float b, float t, EaseDelegate easeFunction)
		{
			return easeFunction(a, b, t);
		}
	}
}