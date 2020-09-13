using System.Collections.Generic;
using UnityEngine;

namespace Maui.Tweening
{
	public static class Tween
	{
		private static readonly HashSet<Tweener> AliveTweeners = new HashSet<Tweener>();
		private static readonly List<Tweener> TweenersToRemove = new List<Tweener>();

		static Tween()
		{
			LifecycleUtils.OnUpdate += Update;
		}

		public static Tweener<float> To(Tweener<float>.Getter getter, Tweener<float>.Setter setter, float finalValue, float duration)
		{
			Tweener<float> result =  new FloatTweener(getter, setter, finalValue, duration);
			RegisterTweenener(result);
			return result;
		}

		public static Tweener<Vector2> To(Tweener<Vector2>.Getter getter, Tweener<Vector2>.Setter setter, Vector2 finalValue, float duration)
		{
			Tweener<Vector2> result = new Vector2Tweener(getter, setter, finalValue, duration);
			RegisterTweenener(result);
			return result;
		}
		
		public static Tweener<Vector3> To(Tweener<Vector3>.Getter getter, Tweener<Vector3>.Setter setter, Vector3 finalValue, float duration)
		{
			Tweener<Vector3> result = new Vector3Tweener(getter, setter, finalValue, duration);
			RegisterTweenener(result);
			return result;
		}
		
		public static Tweener<Color> To(Tweener<Color>.Getter getter, Tweener<Color>.Setter setter, Color finalValue, float duration)
		{
			Tweener<Color> result = new ColorTweener(getter, setter, finalValue, duration);
			RegisterTweenener(result);
			return result;
		}

		private static void Update()
		{
			foreach (Tweener current in AliveTweeners)
			{
				current.Update();
			}

			foreach (Tweener current in TweenersToRemove)
			{
				AliveTweeners.Remove(current);
			}
			
			TweenersToRemove.Clear();
		}
		
		private static void RegisterTweenener(Tweener tweener)
		{
			AliveTweeners.Add(tweener);
			tweener.Completed += () => TweenCompletedHandler(tweener);
		}
		
		private static void TweenCompletedHandler(Tweener tweener)
		{
			TweenersToRemove.Add(tweener);
		}
	}
}