using UnityEngine;

namespace Juice.Tweening
{
	public class ColorTweener : Tweener<Color>
	{
		protected override Interpolator<Color> Interpolator => interpolator;

		private readonly ColorInterpolator interpolator = new ColorInterpolator();

		internal ColorTweener(Getter getter, Setter setter, Color finalValue, float duration)
			: base(getter, setter, finalValue, duration)
		{
			
		}
	}
}