using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Maui
{
	[RequireComponent(typeof(ViewModelComponent), typeof(RectTransform))]
	public abstract class View<T> : MonoBehaviour, IView, IViewModelInjector
		where T : IViewModel
	{
		public event ViewEventHandler InTransitionFinished;
		public event ViewEventHandler OutTransitionFinished;
		public event ViewEventHandler CloseRequested;
		public event ViewEventHandler ViewDestroyed;

		public bool IsVisible => transitionHandler.IsVisible;
		public IViewModel ViewModel => targetComponent != null ? targetComponent.ViewModel : null;

		public Transition InTransition
		{
			get => inTransition;
			set => inTransition = value;
		}

		public Transition OutTransition
		{
			get => outTransition;
			set => outTransition = value;
		}

		public Type InjectionType => typeof(T);
		public ViewModelComponent Target => targetComponent;

		[Header("Target ViewModel Component")]
		[SerializeField] private ViewModelComponent targetComponent;
		[Header("View Animations")]
		[SerializeField] private Transition inTransition;
		[SerializeField] private Transition outTransition;
		
		private readonly TransitionHandler transitionHandler = new TransitionHandler();
		private RectTransform rectTransform;

		protected virtual void Reset()
		{
			targetComponent = GetComponentInChildren<ViewModelComponent>();
		}

		protected virtual void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
		}

		public async Task Show(IViewModel viewModel, Transition overrideTransition = null)
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
				Transition transition = overrideTransition ? overrideTransition : InTransition;
				
				await transitionHandler.Show(rectTransform, transition);
				
				OnInTransitionFinished();
			}
		}

		public async Task Hide(bool animate = true)
		{
			OnHiding();

			await transitionHandler.Hide(rectTransform, animate ? OutTransition : null);

			SetViewModel(default);
			OnOutTransitionFinished();
		}

		protected virtual void SetViewModel(T viewModel)
		{
			if (targetComponent)
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
	}
}
