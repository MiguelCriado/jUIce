using System;
using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
	[RequireComponent(typeof(ViewModelComponent), typeof(RectTransform))]
	public abstract class View<T> : Widget, IView, IViewModelInjector
		where T : IViewModel
	{
		public event ViewEventHandler CloseRequested;
		public event ViewEventHandler ViewDestroyed;

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
