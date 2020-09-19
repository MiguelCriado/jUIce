using System.Threading.Tasks;
using UnityEngine;

namespace Maui
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

		[Header("Widget Animations")]
		[SerializeField] private Transition inTransition;
		[SerializeField] private Transition outTransition;

		private readonly TransitionHandler transitionHandler = new TransitionHandler();
		private RectTransform rectTransform;

		protected virtual void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
		}

		public Task Show()
		{
			return transitionHandler.Show(rectTransform, InTransition);
		}

		public Task Hide()
		{
			return transitionHandler.Hide(rectTransform, OutTransition);
		}
	}
}
