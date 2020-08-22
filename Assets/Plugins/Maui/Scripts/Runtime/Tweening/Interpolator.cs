namespace Maui.Tweening
{
	public abstract class Interpolator<T> : IInterpolator<T>
	{
		private delegate T InterpolationFunction(T min, T max, float t);

		private InterpolationFunction interpolationFunction;

		public Interpolator() : this(Ease.InOutSine)
		{
			
		}
		
		public Interpolator(Ease ease)
		{
			SetEase(ease);
		}
		
		public void SetEase(Ease ease)
		{
			switch (ease)
			{
				case Ease.Linear : interpolationFunction = Linear; break;
				case Ease.InSine : interpolationFunction = InSine; break;
				case Ease.OutSine: interpolationFunction = OutSine; break;
				case Ease.InOutSine: interpolationFunction = InOutSine; break;
				case Ease.InQuad: interpolationFunction = InQuad; break;
				case Ease.OutQuad: interpolationFunction = OutQuad; break;
				case Ease.InOutQuad: interpolationFunction = InOutQuad; break;
				case Ease.InCubic : interpolationFunction = InCubic; break;
				case Ease.OutCubic : interpolationFunction = OutCubic; break;
				case Ease.InOutCubic : interpolationFunction = InOutCubic; break;
				case Ease.InQuart : interpolationFunction = InQuart; break;
				case Ease.OutQuart : interpolationFunction = OutQuart; break;
				case Ease.InOutQuart : interpolationFunction = InOutQuart; break;
				case Ease.InQuint : interpolationFunction = InQuint; break;
				case Ease.OutQuint : interpolationFunction = OutQuint; break;
				case Ease.InOutQuint : interpolationFunction = InOutQuint; break;
				case Ease.InExpo : interpolationFunction = InExpo; break;
				case Ease.OutExpo : interpolationFunction = OutExpo; break;
				case Ease.InOutExpo : interpolationFunction = InOutExpo; break;
				case Ease.InCirc : interpolationFunction = InCirc; break;
				case Ease.OutCirc : interpolationFunction = OutCirc; break;
				case Ease.InOutCirc : interpolationFunction = InOutCirc; break;
				case Ease.InBack : interpolationFunction = InBack; break;
				case Ease.OutBack : interpolationFunction = OutBack; break;
				case Ease.InOutBack : interpolationFunction = InOutBack; break;
				case Ease.InElastic : interpolationFunction = InElastic; break;
				case Ease.OutElastic : interpolationFunction = OutElastic; break;
				case Ease.InOutElastic : interpolationFunction = InOutElastic; break;
				case Ease.InBounce : interpolationFunction = InBounce; break;
				case Ease.OutBounce : interpolationFunction = OutBounce; break;
				case Ease.InOutBounce : interpolationFunction = InOutBounce; break;
			}
			
			OnSetEase(ease);
		}

		public T Evaluate(T a, T b, float t)
		{
			return interpolationFunction(a, b, t);
		}
		
		protected virtual void OnSetEase(Ease ease)
		{
			
		}

		protected abstract T Linear(T a, T b, float t);
		protected abstract T InSine(T a, T b, float t);
		protected abstract T OutSine(T a, T b, float t);
		protected abstract T InOutSine(T a, T b, float t);
		protected abstract T InQuad(T a, T b, float t);
		protected abstract T OutQuad(T a, T b, float t);
		protected abstract T InOutQuad(T a, T b, float t);
		protected abstract T InCubic(T a, T b, float t);
		protected abstract T OutCubic(T a, T b, float t);
		protected abstract T InOutCubic(T a, T b, float t);
		protected abstract T InQuart(T a, T b, float t);
		protected abstract T OutQuart(T a, T b, float t);
		protected abstract T InOutQuart(T a, T b, float t);
		protected abstract T InQuint(T a, T b, float t);
		protected abstract T OutQuint(T a, T b, float t);
		protected abstract T InOutQuint(T a, T b, float t);
		protected abstract T InExpo(T a, T b, float t);
		protected abstract T OutExpo(T a, T b, float t);
		protected abstract T InOutExpo(T a, T b, float t);
		protected abstract T InCirc(T a, T b, float t);
		protected abstract T OutCirc(T a, T b, float t);
		protected abstract T InOutCirc(T a, T b, float t);
		protected abstract T InBack(T a, T b, float t);
		protected abstract T OutBack(T a, T b, float t);
		protected abstract T InOutBack(T a, T b, float t);
		protected abstract T InElastic(T a, T b, float t);
		protected abstract T OutElastic(T a, T b, float t);
		protected abstract T InOutElastic(T a, T b, float t);
		protected abstract T InBounce(T a, T b, float t);
		protected abstract T OutBounce(T a, T b, float t);
		protected abstract T InOutBounce(T a, T b, float t);
	}
}