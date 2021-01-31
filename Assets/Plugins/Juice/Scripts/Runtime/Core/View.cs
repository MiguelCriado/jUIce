using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	[RequireComponent(typeof(ViewModelComponent), typeof(RectTransform))]
	public abstract class View<T> : MonoBehaviour, IView, IViewModelInjector
		where T : IViewModel
	{
		public event ViewEventHandler CloseRequested;
		public event ViewEventHandler ViewDestroyed;

		public bool IsVisible => transitionHandler.IsVisible;

		public bool AllowsInteraction
		{
			get => allowsInteraction;
			set => SetAllowsInteraction(value);
		}

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
		private bool allowsInteraction;
		private GraphicRaycaster[] raycasters;

		protected virtual void Reset()
		{
			targetComponent = GetComponentInChildren<ViewModelComponent>();
		}

		protected virtual void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			allowsInteraction = true;
			raycasters = GetComponentsInChildren<GraphicRaycaster>();
		}

		public void SetViewModel(IViewModel viewModel)
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
		}

		public virtual async Task Show(Transition overrideTransition = null)
		{
			if (gameObject.activeSelf == false)
			{
				Transition transition = overrideTransition ? overrideTransition : InTransition;

				await transitionHandler.Show(rectTransform, transition);
			}
		}

		public virtual async Task Hide(Transition overrideTransition = null)
		{
			Transition transition = overrideTransition ? overrideTransition : OutTransition;

			await transitionHandler.Hide(rectTransform, transition);

			SetViewModel(default);
		}

		protected virtual void SetViewModel(T viewModel)
		{
			if (targetComponent)
			{
				targetComponent.ViewModel = viewModel;
			}
		}

		private void SetAllowsInteraction(bool value)
		{
			allowsInteraction = value;

			foreach (var current in raycasters)
			{
				current.enabled = value;
			}
		}
	}
}
