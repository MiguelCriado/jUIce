using System;
using UnityEngine;

namespace Maui.Tweening
{
	public abstract class Tweener<T>
	{
		public event Action Completed;
		
		public delegate T Getter();
		public delegate void Setter(T value);
		
		public bool IsPlaying { get; private set; }
		protected abstract Interpolator<T> Interpolator { get; }
		
		private Setter setter;
		private T initialValue;
		private T finalValue;
		private float duration;
		private float initialTime;

		internal Tweener(Getter getter, Setter setter, T finalValue, float duration)
		{
			this.setter = setter;
			initialValue = getter();
			this.finalValue = finalValue;
			this.duration = duration;
			initialTime = Time.realtimeSinceStartup;
			IsPlaying = true;
			LifecycleUtils.OnUpdate += Update;
		}

		public Tweener<T> SetEase(Ease ease)
		{
			Interpolator.SetEase(ease);
			return this;
		}

		protected virtual void OnComplete()
		{
			setter(finalValue);
			
			Completed?.Invoke();
		}

		private void Update()
		{
			float elapsedTime = Time.realtimeSinceStartup - initialTime;

			if (elapsedTime <= duration)
			{
				float t = duration / elapsedTime;
				T newValue = Interpolator.Evaluate(initialValue, finalValue, t);
				setter(newValue);
			}
			else
			{
				LifecycleUtils.OnUpdate -= Update;
				
				OnComplete();
			}
		}
	}
}