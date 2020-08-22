using UnityEngine;

namespace Maui.Tweening
{
	public class ColorTweener : Tweener<Color>
	{
		protected override Interpolator<Color> Interpolator => interpolator;

		private ColorInterpolator interpolator = new ColorInterpolator();

		public ColorTweener(Getter getter, Setter setter, Color finalValue, float duration)
			: base(getter, setter, finalValue, duration)
		{
			
		}
	}
}