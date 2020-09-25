using UnityEngine;

namespace Maui.Tweening
{
	public abstract class Tweener : ITweener
	{
		public event TweenerIdChangedHandler IdChanged;
		public event TweenerLifecycleEventHandler Completed;
		public event TweenerLifecycleEventHandler Killed;

		public object Id
		{
			get => id;
			set
			{
				object lastId = id;
				id = value;
				OnIdChanged(lastId, id);
			}
		}
		
		public bool IsPlaying { get; protected set; }

		private object id;

		public Tweener SetId(object id)
		{
			Id = id;
			return this;
		}

		public abstract void Play();
		public abstract void Stop();

		public void Kill()
		{
			Stop();
			OnKill();
		}
		
		internal abstract void Update();

		protected virtual void OnIdChanged(object lastId, object newId)
		{
			IdChanged?.Invoke(this, lastId, newId);
		}
		
		protected virtual void OnComplete()
		{
			Completed?.Invoke(this);
		}

		protected virtual void OnKill()
		{
			Killed?.Invoke(this);
		}
	}
	
	public abstract class Tweener<T> : Tweener
	{
		public delegate T Getter();
		public delegate void Setter(T value);
		
		protected abstract Interpolator<T> Interpolator { get; }

		private Getter getter;
		private Setter setter;
		private T initialValue;
		private T finalValue;
		private float duration;
		private float initialTime;

		internal Tweener(Getter getter, Setter setter, T finalValue, float duration)
		{
			this.getter = getter;
			this.setter = setter;
			this.finalValue = finalValue;
			this.duration = duration;
		}

		public override void Play()
		{
			initialValue = getter();
			initialTime = Time.realtimeSinceStartup;
			IsPlaying = true;
		}

		public override void Stop()
		{
			IsPlaying = false;
		}

		internal void Setup(Getter getter, Setter setter, T finalValue, float duration)
		{
			this.getter = getter;
			this.setter = setter;
			this.finalValue = finalValue;
			this.duration = duration;
		}

		public Tweener<T> SetEase(Ease ease)
		{
			Interpolator.SetEase(ease);
			return this;
		}

		public new Tweener<T> SetId(object id)
		{
			base.SetId(id);
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