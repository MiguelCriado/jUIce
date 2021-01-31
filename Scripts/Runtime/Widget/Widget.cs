using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	[RequireComponent(typeof(RectTransform))]
	public class Widget : MonoBehaviour, ITransitionable
	{
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

			Transition transition = overrideTransition ? overrideTransition : InTransition;

			await transitionHandler.Show(rectTransform, transition);
		}

		public virtual async Task Hide(Transition overrideTransition = null)
		{
			Initialize();

			Transition transition = overrideTransition ? overrideTransition : OutTransition;

			await transitionHandler.Hide(rectTransform, transition);
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
