using UnityEngine;

namespace Maui.Tweening
{
	public class FloatInterpolator : Interpolator<float>
	{
		const float C1 = 1.70158f;
		const float C2 = C1 * 1.525f;
		const float C3 = C1 + 1;
		const float C4 = (2 * Mathf.PI) / 3;
		const float C5 = (2 * Mathf.PI) / 4.5f;
		
		const float N1 = 7.5625f;
		const float D1 = 2.75f;
	
		protected override float Linear(float a, float b, float t)
		{
			return Lerp(a, b, t);
		}

		protected override float InSine(float a, float b, float t)
		{
			return Lerp(a, b, 1 - Mathf.Cos((t * Mathf.PI) / 2f));
		}

		protected override float OutSine(float a, float b, float t)
		{
			return Lerp(a, b, Mathf.Sin((t * Mathf.PI) / 2f));
		}

		protected override float InOutSine(float a, float b, float t)
		{
			return Lerp(a, b, -(Mathf.Cos(t * Mathf.PI) - 1) / 2f);
		}

		protected override float InQuad(float a, float b, float t)
		{
			return Lerp(a, b, t * t);
		}

		protected override float OutQuad(float a, float b, float t)
		{
			return Lerp(a, b, 1 - (1 - t) * (1 - t));
		}

		protected override float InOutQuad(float a, float b, float t)
		{
			return Lerp(a, b, t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2);
		}

		protected override float InCubic(float a, float b, float t)
		{
			return Lerp(a, b, t * t * t);
		}

		protected override float OutCubic(float a, float b, float t)
		{
			return Lerp(a, b, 1 - Mathf.Pow(1 - t, 3));
		}

		protected override float InOutCubic(float a, float b, float t)
		{
			return Lerp(a, b, t < 0.5f ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2);
		}

		protected override float InQuart(float a, float b, float t)
		{
			return Lerp(a, b, t * t * t * t);
		}

		protected override float OutQuart(float a, float b, float t)
		{
			return Lerp(a, b, 1 - Mathf.Pow(1 - t, 4));
		}

		protected override float InOutQuart(float a, float b, float t)
		{
			return Lerp(a, b, t < 0.5f ? 8 * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 4) / 2);
		}

		protected override float InQuint(float a, float b, float t)
		{
			return Lerp(a, b, t * t * t * t * t);
		}

		protected override float OutQuint(float a, float b, float t)
		{
			return Lerp(a, b, 1 - Mathf.Pow(1 - t, 5));
		}

		protected override float InOutQuint(float a, float b, float t)
		{
			return Lerp(a, b, t < 0.5f ? 16 * t * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 5) / 2);
		}

		protected override float InExpo(float a, float b, float t)
		{
			return Lerp(a, b, t == 0 ? 0 : Mathf.Pow(2, 10 * t - 10));
		}

		protected override float OutExpo(float a, float b, float t)
		{
			return Lerp(a, b, t == 1 ? 1 : 1 - Mathf.Pow(2, -10 * t));
		}

		protected override float InOutExpo(float a, float b, float t)
		{
			return Lerp(a, b, t == 0 ? 0 : t == 1 ? 1 : t < 0.5f ? Mathf.Pow(2, 20 * t - 10) / 2 : (2 - Mathf.Pow(2, -20 * t + 10)) / 2);
		}

		protected override float InCirc(float a, float b, float t)
		{
			return Lerp(a, b, 1 - Mathf.Sqrt(1 - Mathf.Pow(t, 2)));
		}

		protected override float OutCirc(float a, float b, float t)
		{
			return Lerp(a, b, Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2)));
		}

		protected override float InOutCirc(float a, float b, float t)
		{
			return Lerp(a, b, t < 0.5f ? (1 - Mathf.Sqrt(1 -Mathf.Pow(2 * t, 2))) / 2 : (Mathf.Sqrt(1 - Mathf.Pow(-2 * t + 2, 2)) + 1) / 2);
		}

		protected override float InBack(float a, float b, float t)
		{
			return Lerp(a, b, C3 * t * t * t - C1 * t * t);
		}

		protected override float OutBack(float a, float b, float t)
		{
			return Lerp(a, b, 1 + C3 * Mathf.Pow(t - 1, 3) + C1 * Mathf.Pow(t - 1, 2));
		}

		protected override float InOutBack(float a, float b, float t)
		{
			return Lerp(a, b, t < 0.5f ? (Mathf.Pow(2 * t, 2) * ((C2 + 1) * 2 * t - C2)) / 2 : (Mathf.Pow(2 * t - 2, 2) * ((C2 + 1) * (t * 2 - 2) + C2) + 2) / 2);
		}

		protected override float InElastic(float a, float b, float t)
		{
			return Lerp(a, b, t == 0 ? 0 : t == 1 ? 1 : -Mathf.Pow(2, 10 * t - 10) * Mathf.Sin((t * 10 - 10.75f) * C4));
		}

		protected override float OutElastic(float a, float b, float t)
		{
			return Lerp(a, b, t == 0 ? 0 : t == 1 ? 1 : Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * C4) + 1);
		}

		protected override float InOutElastic(float a, float b, float t)
		{
			return Lerp(a, b, t == 0 ? 0 : t == 1 ? 1 : t < 0.5f ? -(Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 11.125f) * C5)) / 2 : (Mathf.Pow(2, -20 * t + 10) * Mathf.Sin((20 * t - 11.125f) * C5)) / + 1);
		}

		protected override float InBounce(float a, float b, float t)
		{
			return Lerp(a, b, 1- RawOutBounce(1 - t));
		}

		protected override float OutBounce(float a, float b, float t)
		{
			return Lerp(a, b, RawOutBounce(t));
		}

		protected override float InOutBounce(float a, float b, float t)
		{
			return Lerp(a, b, t < 0.5f ? (1 - RawOutBounce(1 - 2 * t)) / 2 : (1 + RawOutBounce(2 * t - 1)) / 2);
		}

		private static float RawOutBounce(float t)
		{
			if (t < 1 / D1) 
			{
				return N1 * t * t;
			}
			
			if (t < 2 / D1) 
			{
				return N1 * (t -= 1.5f / D1) * t + 0.75f;
			}

			if (t < 2.5f / D1) 
			{
				return N1 * (t -= 2.25f / D1) * t + 0.9375f;
			}

			return N1 * (t -= 2.625f / D1) * t + 0.984375f;
		}

		private static float Lerp(float a, float b, float t)
		{
			return a + (b - a) * t;
		}
	}
}