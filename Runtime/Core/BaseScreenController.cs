using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Muui
{
	public abstract class BaseScreenController<T> : MonoBehaviour, IScreenController
		where T : IScreenProperties
	{
		public event ScreenControllerDelegate OnInTransitionFinished;
		public event ScreenControllerDelegate OnOutTransitionFinished;
		public event ScreenControllerDelegate OnCloseRequest;
		public event ScreenControllerDelegate OnScreenDestroyed;

		public bool IsVisible { get; private set; }

		public BaseTransition InTransition
		{
			get => inTransition;
			set => inTransition = value;
		}

		public BaseTransition OutTransition
		{
			get => outTransition;
			set => outTransition = value;
		}

		protected T Properties
		{
			get => properties;
			set => properties = value;
		}

		[Header("Screen Animations")]
		[SerializeField] private BaseTransition inTransition;
		[SerializeField] private BaseTransition outTransition;
		[Header("Screen Properties")]
		[SerializeField] private T properties;

		private bool propertiesHaveBeenSet;

		public async Task Show(IScreenProperties properties = null)
		{
			if (properties != null)
			{
				if (properties is T typedProperties)
				{
					CleanUpProperties();
					SetProperties(typedProperties);
				}
				else
				{
					Debug.LogError($"Properties passed have wrong type! ({properties.GetType()} instead of {typeof(T)})");
					return;
				}
			}

			OnShow();

			if (gameObject.activeSelf)
			{
				OnInTransitionFinished?.Invoke(this);
			}
			else
			{
				DoAnimation(inTransition, OnInAnimationFinished, true);

				while (IsVisible == false)
				{
					await Task.Yield();
				}
			}
		}

		public async Task Hide(bool animate = true)
		{
			DoAnimation(animate ? outTransition : null, OnOutAnimationFinished, false);
			CleanUpProperties();

			while (IsVisible)
			{
				await Task.Yield();
			}
		}

		protected virtual void OnPropertiesSet()
		{

		}

		protected virtual void UnsubscribeFromProperties()
		{

		}

		protected virtual void SetProperties(T properties)
		{
			this.properties = properties;
			propertiesHaveBeenSet = true;
			OnPropertiesSet();
		}

		protected virtual void OnShow()
		{

		}

		private void CleanUpProperties()
		{
			if (propertiesHaveBeenSet)
			{
				UnsubscribeFromProperties();
			}

			propertiesHaveBeenSet = false;
		}

		private void DoAnimation(BaseTransition targetTransition, Action callbackWhenFinished, bool isVisible)
		{
			if (targetTransition == null)
			{
				gameObject.SetActive(isVisible);
				callbackWhenFinished?.Invoke();
			}
			else
			{
				if (isVisible && gameObject.activeSelf == false)
				{
					gameObject.SetActive(true);
				}

				targetTransition.Animate(transform, callbackWhenFinished);
			}
		}

		private void OnInAnimationFinished()
		{
			IsVisible = true;
			OnInTransitionFinished?.Invoke(this);
		}

		private void OnOutAnimationFinished()
		{
			IsVisible = false;
			gameObject.SetActive(false);
			OnOutTransitionFinished?.Invoke(this);
		}
	}
}
