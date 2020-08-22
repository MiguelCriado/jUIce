using System;
using UnityEngine;

namespace Maui.Tweening
{
	public abstract class Tweener : ITweener
	{
		public event Action Completed;
		
		public bool IsPlaying { get; protected set; }

		internal abstract void Update();
		
		protected virtual void OnComplete()
		{
			Completed?.Invoke();
		}
	}
	
	public abstract class Tweener<T> : Tweener
	{
		public delegate T Getter();
		public delegate void Setter(T value);
		
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
		}

		public Tweener<T> SetEase(Ease ease)
		{
			Interpolator.SetEase(ease);
			return this;
		}

		internal override void Update()
		{
			if (IsPlaying)
			{
				float elapsedTime = Time.realtimeSinceStartup - initialTime;

				if (elapsedTime <= duration)
				{
					float t = elapsedTime / duration;
					T newValue = Interpolator.Evaluate(initialValue, finalValue, t);
					setter(newValue);
				}
				else
				{
					IsPlaying = false;
					OnComplete();
				}
			}
		}
		
		protected override void OnComplete()
		{
			setter(finalValue);
			
			base.OnComplete();
		}
	}
}