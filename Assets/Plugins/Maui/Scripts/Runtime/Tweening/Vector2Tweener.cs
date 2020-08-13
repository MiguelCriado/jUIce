using UnityEngine;

namespace Maui.Tweening
{
	public class Vector2Tweener : Tweener<Vector2>
	{
		protected override Interpolator<Vector2> Interpolator => interpolator;
		
		private Vector2Interpolator interpolator = new Vector2Interpolator();

		public Vector2Tweener(Getter getter, Setter setter, Vector2 finalValue, float duration)
			: base(getter, setter, finalValue, duration)
		{
			
		}
	}
}