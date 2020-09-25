using System.Collections.Generic;
using UnityEngine;

namespace Maui.Tweening
{
	public static class Tween
	{
		private static readonly HashSet<Tweener> AliveTweeners = new HashSet<Tweener>();
		private static readonly HashSet<Tweener> TweenersToRemove = new HashSet<Tweener>();
		private static readonly Dictionary<object, HashSet<Tweener>> TweenersById = new Dictionary<object, HashSet<Tweener>>();
		
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
		
		public static int Kill(object tweenId)
		{
			HashSet<Tweener> idSet = GetIdSet(tweenId);
			int result = idSet.Count;

			foreach (Tweener current in idSet)
			{
				current.Kill();
			}

			return result;
		}

		private static void Update()
		{
			RemoveUselessTweeners();
			UpdateAliveTweeners();
		}

		private static void UpdateAliveTweeners()
		{
			foreach (Tweener current in AliveTweeners)
			{
				current.Update();
			}
		}

		private static void RemoveUselessTweeners()
		{
			foreach (Tweener current in TweenersToRemove)
			{
				UnregisterTweener(current);
			}

			TweenersToRemove.Clear();
		}

		private static void RegisterTweenener(Tweener tweener)
		{
			tweener.Play();
			AliveTweeners.Add(tweener);
			RegisterTweenerId(tweener, tweener.Id);
			tweener.IdChanged += OnTweenChangedId;
			tweener.Completed += OnTweenCompleted;
			tweener.Killed += OnTweenKilled;
		}

		private static void UnregisterTweener(Tweener tweener)
		{
			AliveTweeners.Remove(tweener);
			UnregisterTweenerId(tweener, tweener.Id);
			tweener.IdChanged -= OnTweenChangedId;
			tweener.Completed -= OnTweenCompleted;
			tweener.Killed -= OnTweenKilled;
		}
		
		private static void RegisterTweenerId(Tweener tweener, object id)
		{
			HashSet<Tweener> idSet = GetIdSet(id);
			idSet.Add(tweener);
		}

		private static void UnregisterTweenerId(Tweener tweener, object id)
		{
			HashSet<Tweener> idSet = GetIdSet(id);
			idSet.Remove(tweener);
			
			if (idSet.Count <= 0)
			{
				TweenersById.Remove(id);
			}
		}
		
		private static HashSet<Tweener> GetIdSet(object id)
		{
			if (TweenersById.TryGetValue(id, out var result) == false)
			{
				result = new HashSet<Tweener>();
				TweenersById.Add(id, result);
			}

			return result;
		}

		private static void OnTweenChangedId(Tweener tweener, object lastId, object newId)
		{
			UnregisterTweenerId(tweener, lastId);
			RegisterTweenerId(tweener, newId);
		}

		private static void OnTweenCompleted(Tweener tweener)
		{
			TweenersToRemove.Add(tweener);
		}

		private static void OnTweenKilled(Tweener tweener)
		{
			TweenersToRemove.Add(tweener);
		}
	}
}