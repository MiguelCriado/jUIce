using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Maui
{
#pragma warning disable 0067
	[RequireComponent(typeof(ViewModelComponent))]
	public abstract class BaseView<T> : MonoBehaviour, IView, IViewModelInjector
		where T : IViewModel
	{
		public event ViewEventHandler InTransitionFinished;
		public event ViewEventHandler OutTransitionFinished;
		public event ViewEventHandler CloseRequested;
		public event ViewEventHandler ViewDestroyed;

		public bool IsVisible { get; private set; }
		public IViewModel ViewModel => viewModel;

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

		public Type InjectionType => typeof(T);
		public ViewModelComponent Target => targetComponent;

		[Header("Target ViewModel Component")]
		[SerializeField] private ViewModelComponent targetComponent;
		[Header("View Animations")]
		[SerializeField] private BaseTransition inTransition;
		[SerializeField] private BaseTransition outTransition;

		protected T viewModel;

		protected void Reset()
		{
			targetComponent = GetComponentInChildren<ViewModelComponent>();
		}

		public async Task Show(IViewModel viewModel)
		{
			if (viewModel != null)
			{
				if (viewModel is T typedViewModel)
				{
					SetViewModel(typedViewModel);
				}
				else
				{
					Debug.LogError($"ViewModel passed have wrong type! ({viewModel.GetType()} instead of {typeof(T)})", this);
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
			SetViewModel(default);
			OnOutTransitionFinished();
		}

		protected virtual void SetViewModel(T viewModel)
		{
			this.viewModel = viewModel;

			if (targetComponent != null)
			{
				targetComponent.ViewModel = viewModel;
			}
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
