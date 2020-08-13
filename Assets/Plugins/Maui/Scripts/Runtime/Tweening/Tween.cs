using UnityEngine;

namespace Maui.Tweening
{
	public static class Tween
	{
		public static Tweener<float> To(Tweener<float>.Getter getter, Tweener<float>.Setter setter, float finalValue, float duration)
		{
			return new FloatTweener(getter, setter, finalValue, duration);
		}

		public static Tweener<Vector2> To(Tweener<Vector2>.Getter getter, Tweener<Vector2>.Setter setter, Vector2 finalValue, float duration)
		{
			return new Vector2Tweener(getter, setter, finalValue, duration);
		}
		
		public static Tweener<Vector3> To(Tweener<Vector3>.Getter getter, Tweener<Vector3>.Setter setter, Vector3 finalValue, float duration)
		{
			return new Vector3Tweener(getter, setter, finalValue, duration);
		}
		
		public static Tweener<Color> To(Tweener<Color>.Getter getter, Tweener<Color>.Setter setter, Color finalValue, float duration)
		{
			return new ColorTweener(getter, setter, finalValue, duration);
		}
	}
}