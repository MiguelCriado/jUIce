namespace Maui.Tweening
{
	public abstract class Interpolator<T> : IInterpolator<T>
	{
		private EaseDelegate easeFunction;
		
		public Interpolator() : this(Ease.InOutSine)
		{
			
		}
		
		public Interpolator(Ease ease)
		{
			easeFunction = TweenerCore.GetEaseFunction(ease);
		}
		
		public void SetEase(Ease ease)
		{
			easeFunction = TweenerCore.GetEaseFunction(ease);
		}

		public T Evaluate(T a, T b, float t)
		{
			return Evaluate(a, b, t, easeFunction);
		}

		protected abstract T Evaluate(T a, T b, float t, EaseDelegate easeFunction);
	}
}