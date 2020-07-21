using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Maui
{
#pragma warning disable 0067
	[RequireComponent(typeof(ViewModelComponent))]
	public abstract class BaseScreenController<T> : MonoBehaviour, IScreenController<T>, IViewModelInjector<T>
		where T : IViewModel
	{
		public event ScreenControllerEventHandler InTransitionFinished;
		public event ScreenControllerEventHandler OutTransitionFinished;
		public event ScreenControllerEventHandler CloseRequested;
		public event ScreenControllerEventHandler ScreenDestroyed;

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

		public ViewModelComponent Target => targetComponent;

		[Header("Target ViewModel Component")]
		[SerializeField] private ViewModelComponent targetComponent;
		[Header("Screen Animations")]
		[SerializeField] private BaseTransition inTransition;
		[SerializeField] private BaseTransition outTransition;

		protected T viewModel;

		protected void Reset()
		{
			targetComponent = GetComponentInChildren<ViewModelComponent>();
		}

		public async Task Show(T viewModel)
		{
			if (viewModel != null)
			{
				SetViewModel(viewModel);
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
		}

		protected virtual void SetViewModel(T viewModel)
		{
			this.viewModel = viewModel;

			if (targetComponent != null)
			{
				targetComponent.Set(viewModel);
			}
		}

		protected virtual void OnShowing()
		{

		}

		protected virtual void OnInTransitionFinished()
		{
			InTransitionFinished?.Invoke((IScreenController)this);
		}

		protected virtual void OnHiding()
		{

		}

		protected virtual void OnOutTransitionFinished()
		{
			OutTransitionFinished?.Invoke((IScreenController)this);
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
