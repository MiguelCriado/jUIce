using System;
using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	[RequireComponent(typeof(ViewModelComponent), typeof(RectTransform))]
	public abstract class View<T> : Widget, IView, IViewModelInjector
		where T : IViewModel
	{
		public delegate void ViewModelEventHandler(View<T> source, T lastViewModel, T newViewModel);

		public event ViewEventHandler CloseRequested;
		public event ViewEventHandler ViewDestroyed;
		public event ViewModelEventHandler ViewModelChanged;

		public bool AllowsInteraction
		{
			get => allowsInteraction;
			set => SetAllowsInteraction(value);
		}

		public Type InjectionType => typeof(T);
		public ViewModelComponent Target => targetComponent;

		[Header("Target ViewModel Component")]
		[SerializeField] private ViewModelComponent targetComponent;

		private bool allowsInteraction;
		private GraphicRaycaster[] raycasters;

		protected virtual void Reset()
		{
			targetComponent = GetComponentInChildren<ViewModelComponent>();
		}

		protected override void Awake()
		{
			allowsInteraction = true;
			raycasters = GetComponentsInChildren<GraphicRaycaster>();

			if (Target)
			{
				Target.ViewModelChanged += OnTargetComponentViewModelChanged;
			}
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

		protected virtual void SetViewModel(T viewModel)
		{
			if (targetComponent)
			{
				targetComponent.ViewModel = viewModel;
			}
		}

		protected virtual void OnViewModelChanged(IViewModel lastViewModel, IViewModel newViewModel)
		{
			ViewModelChanged?.Invoke(this, (T)lastViewModel, (T)newViewModel);
		}

		private void SetAllowsInteraction(bool value)
		{
			allowsInteraction = value;

			foreach (var current in raycasters)
			{
				current.enabled = value;
			}
		}

		private void OnTargetComponentViewModelChanged(ViewModelComponent source, IViewModel lastViewModel, IViewModel newViewModel)
		{
			OnViewModelChanged(lastViewModel, newViewModel);
		}
	}
}
