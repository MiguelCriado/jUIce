using System.Collections.Generic;
using Juice.Utils;
using UnityEngine;

namespace Juice.Tweening
{
	public class Tween : Singleton<Tween>
	{
		private readonly HashSet<Tweener> aliveTweeners = new HashSet<Tweener>();
		private readonly HashSet<Tweener> tweenersToRemove = new HashSet<Tweener>();
		private readonly Dictionary<object, HashSet<Tweener>> tweenersById = new Dictionary<object, HashSet<Tweener>>();

		public static Tweener<float> To(Tweener<float>.Getter getter, Tweener<float>.Setter setter, float finalValue, float duration)
		{
			Tweener<float> result =  new FloatTweener(getter, setter, finalValue, duration);
			Instance.RegisterTweenener(result);
			return result;
		}

		public static Tweener<Vector2> To(Tweener<Vector2>.Getter getter, Tweener<Vector2>.Setter setter, Vector2 finalValue, float duration)
		{
			Tweener<Vector2> result = new Vector2Tweener(getter, setter, finalValue, duration);
			Instance.RegisterTweenener(result);
			return result;
		}

		public static Tweener<Vector3> To(Tweener<Vector3>.Getter getter, Tweener<Vector3>.Setter setter, Vector3 finalValue, float duration)
		{
			Tweener<Vector3> result = new Vector3Tweener(getter, setter, finalValue, duration);
			Instance.RegisterTweenener(result);
			return result;
		}

		public static Tweener<Color> To(Tweener<Color>.Getter getter, Tweener<Color>.Setter setter, Color finalValue, float duration)
		{
			Tweener<Color> result = new ColorTweener(getter, setter, finalValue, duration);
			Instance.RegisterTweenener(result);
			return result;
		}

		public static int Kill(object tweenId)
		{
			HashSet<Tweener> idSet = Instance.GetIdSet(tweenId);
			int result = idSet.Count;

			foreach (Tweener current in idSet)
			{
				current.Kill();
			}

			return result;
		}

		protected virtual void Update()
		{
			RemoveUselessTweeners();
			UpdateAliveTweeners();
		}

		private void UpdateAliveTweeners()
		{
			foreach (Tweener current in aliveTweeners)
			{
				current.Update();
			}
		}

		private void RemoveUselessTweeners()
		{
			foreach (Tweener current in tweenersToRemove)
			{
				UnregisterTweener(current);
			}

			tweenersToRemove.Clear();
		}

		private void RegisterTweenener(Tweener tweener)
		{
			tweener.Play();
			aliveTweeners.Add(tweener);
			RegisterTweenerId(tweener, tweener.Id);
			tweener.IdChanged += OnTweenChangedId;
			tweener.Completed += OnTweenCompleted;
			tweener.Killed += OnTweenKilled;
		}

		private void UnregisterTweener(Tweener tweener)
		{
			aliveTweeners.Remove(tweener);
			UnregisterTweenerId(tweener, tweener.Id);
			tweener.IdChanged -= OnTweenChangedId;
			tweener.Completed -= OnTweenCompleted;
			tweener.Killed -= OnTweenKilled;
		}

		private void RegisterTweenerId(Tweener tweener, object id)
		{
			HashSet<Tweener> idSet = GetIdSet(id);
			idSet.Add(tweener);
		}

		private void UnregisterTweenerId(Tweener tweener, object id)
		{
			HashSet<Tweener> idSet = GetIdSet(id);
			idSet.Remove(tweener);

			if (idSet.Count <= 0)
			{
				tweenersById.Remove(id);
			}
		}

		private HashSet<Tweener> GetIdSet(object id)
		{
			if (tweenersById.TryGetValue(id, out var result) == false)
			{
				result = new HashSet<Tweener>();
				tweenersById.Add(id, result);
			}

			return result;
		}

		private void OnTweenChangedId(Tweener tweener, object lastId, object newId)
		{
			UnregisterTweenerId(tweener, lastId);
			RegisterTweenerId(tweener, newId);
		}

		private void OnTweenCompleted(Tweener tweener)
		{
			tweenersToRemove.Add(tweener);
		}

		private void OnTweenKilled(Tweener tweener)
		{
			tweenersToRemove.Add(tweener);
		}
	}
}
