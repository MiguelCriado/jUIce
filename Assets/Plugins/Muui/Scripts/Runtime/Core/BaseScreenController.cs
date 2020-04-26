using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Muui
{
#pragma warning disable 0067
	public abstract class BaseScreenController<T> : MonoBehaviour, IScreenController
		where T : IScreenProperties
	{
		public event ScreenControllerDelegate InTransitionFinished;
		public event ScreenControllerDelegate OutTransitionFinished;
		public event ScreenControllerDelegate CloseRequested;
		public event ScreenControllerDelegate ScreenDestroyed;

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
					SubscribeToProperties();
				}
				else
				{
					Debug.LogError($"Properties passed have wrong type! ({properties.GetType()} instead of {typeof(T)})");
					return;
				}
			}

			OnShowing();

			if (gameObject.activeSelf)
			{
				OnInTransitionFinished();
			}
			else
			{
				await DoAnimation(inTransition, true);

				IsVisible = true;
				OnInTransitionFinished();
			}
		}

		public async Task Hide(bool animate = true)
		{
			OnHiding();

			await DoAnimation(animate ? outTransition : null, false);

			IsVisible = false;
			gameObject.SetActive(false);
			OnOutTransitionFinished();

			CleanUpProperties();
		}

		protected virtual void SubscribeToProperties()
		{

		}

		protected virtual void UnsubscribeFromProperties()
		{

		}

		protected virtual void SetProperties(T properties)
		{
			this.properties = properties;
		}

		protected virtual void OnShowing()
		{

		}

		protected virtual void OnInTransitionFinished()
		{
			InTransitionFinished?.Invoke(this);
		}

		protected virtual void OnHiding()
		{

		}

		protected virtual void OnOutTransitionFinished()
		{
			OutTransitionFinished?.Invoke(this);
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

				try
				{
					await targetTransition.Animate(transform);
				}
				catch (Exception e)
				{
					Debug.LogError(e);
				}
			}
		}
	}
#pragma warning disable 0067
}
