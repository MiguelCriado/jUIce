using UnityEngine;

namespace Maui.Tweening
{
	public class Vector3Tweener : Tweener<Vector3>
	{
		protected override Interpolator<Vector3> Interpolator => interpolator;
		
		private readonly Vector3Interpolator interpolator = new Vector3Interpolator();

		internal Vector3Tweener(Getter getter, Setter setter, Vector3 finalValue, float duration)
			: base(getter, setter, finalValue, duration)
		{
			
		}
	}
}