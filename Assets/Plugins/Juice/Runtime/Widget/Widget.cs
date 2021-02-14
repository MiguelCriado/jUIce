using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
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
		[SerializeField] private ComponentTransition showTransition;
		[SerializeField] private ComponentTransition hideTransition;

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
			Showing?.Invoke(this);

			ITransition transition = overrideTransition ?? ShowTransition;

			await transitionHandler.Show(rectTransform, transition);

			Shown?.Invoke(this);
		}

		public virtual async Task Hide(ITransition overrideTransition = null)
		{
			Initialize();
			Hiding?.Invoke(this);

			ITransition transition = overrideTransition ?? HideTransition;

			await transitionHandler.Hide(rectTransform, transition);

			Hidden?.Invoke(this);
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
