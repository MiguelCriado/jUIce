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

		[Header("Transitions")]
		[SerializeField] private Transition inTransition;
		[SerializeField] private Transition outTransition;

		private readonly TransitionHandler transitionHandler = new TransitionHandler();
		private RectTransform rectTransform;
		private bool isInitialized;

		protected virtual void Awake()
		{
			Initialize();
		}

		public virtual async Task Show(Transition overrideTransition = null)
		{
			Initialize();
			Showing?.Invoke(this);

			Transition transition = overrideTransition ? overrideTransition : InTransition;

			await transitionHandler.Show(rectTransform, transition);

			Shown?.Invoke(this);
		}

		public virtual async Task Hide(Transition overrideTransition = null)
		{
			Initialize();
			Hiding?.Invoke(this);

			Transition transition = overrideTransition ? overrideTransition : OutTransition;

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
