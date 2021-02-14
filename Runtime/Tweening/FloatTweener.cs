namespace Juice.Tweening
{
	public class FloatTweener : Tweener<float>
	{
		protected override Interpolator<float> Interpolator => interpolator;
		
		private readonly FloatInterpolator interpolator = new FloatInterpolator();
		
		internal FloatTweener(Getter getter, Setter setter, float finalValue, float duration)
			: base(getter, setter, finalValue, duration)
		{
		}
	}
}