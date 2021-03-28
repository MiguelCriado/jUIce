using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Juice
{
	[Serializable]
	public class WidgetEvents
	{
		public UnityEvent OnShowing = new UnityEvent();
		public UnityEvent OnShown = new UnityEvent();
		public UnityEvent OnHiding = new UnityEvent();
		public UnityEvent OnHidden = new UnityEvent();
	}

	[RequireComponent(typeof(RectTransform))]
	public class Widget : MonoBehaviour, ITransitionable
	{
		public event TransitionableEventHandler Showing;
		public event TransitionableEventHandler Shown;
		public event TransitionableEventHandler Hiding;
		public event TransitionableEventHandler Hidden;

		public bool IsVisible => transitionHandler.IsVisible;

		public ComponentTransition ShowTransition
		{
			get => showTransition;
			set => showTransition = value;
		}

		public ComponentTransition HideTransition
		{
			get => hideTransition;
			set => hideTransition = value;
		}

		[Header("Transitions")]
		[SerializeField] private ComponentTransition showTransition = default;
		[SerializeField] private ComponentTransition hideTransition = default;
		[SerializeField] private WidgetEvents transitionEvents = new WidgetEvents();

		private readonly TransitionHandler transitionHandler = new TransitionHandler();
		private RectTransform rectTransform;
		private bool isInitialized;

		protected virtual void Awake()
		{
			Initialize();
		}

		public virtual async Task Show(ITransition overrideTransition = null)
		{
			Initialize();
			OnShowing();

			ITransition transition = overrideTransition ?? ShowTransition;

			await transitionHandler.Show(rectTransform, transition);

			OnShown();
		}

		public virtual async Task Hide(ITransition overrideTransition = null)
		{
			Initialize();
			OnHiding();

			ITransition transition = overrideTransition ?? HideTransition;

			await transitionHandler.Hide(rectTransform, transition);

			OnHidden();
		}

		public void ShowWidget()
		{
			Show().RunAndForget();
		}

		public void HideWidget()
		{
			Hide().RunAndForget();
		}

		protected virtual void OnShowing()
		{
			Showing?.Invoke(this);
			transitionEvents.OnShowing.Invoke();
		}

		protected virtual void OnShown()
		{
			Shown?.Invoke(this);
			transitionEvents.OnShown.Invoke();
		}

		protected virtual void OnHiding()
		{
			Hiding?.Invoke(this);
			transitionEvents.OnHiding.Invoke();
		}

		protected virtual void OnHidden()
		{
			Hidden?.Invoke(this);
			transitionEvents.OnHidden.Invoke();
		}

		private void Initialize()
		{
			if (isInitialized == false)
			{
				rectTransform = GetComponent<RectTransform>();
				isInitialized = true;
			}
		}
	}
}
