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

		protected T CurrentProperties
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
					propertiesHaveBeenSet = true;
					OnPropertiesSet();
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
				await DoAnimation(inTransition, true);

				IsVisible = true;
				OnInTransitionFinished?.Invoke(this);
			}
		}

		public async Task Hide(bool animate = true)
		{
			await DoAnimation(animate ? outTransition : null, false);

			IsVisible = false;
			gameObject.SetActive(false);
			OnOutTransitionFinished?.Invoke(this);

			CleanUpProperties();
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

		private async Task DoAnimation(BaseTransition targetTransition, bool isVisible)
		{
			if (targetTransition == null)
			{
				gameObject.SetActive(isVisible);
			}
			else
			{
				if (isVisible && gameObject.activeSelf == false)
				{
					gameObject.SetActive(true);
				}

				targetTransition.PrepareForAnimation(transform);

				await targetTransition.Animate(transform);
			}
		}
	}
}
