using System.Threading.Tasks;
using UnityEngine;

namespace Maui
{
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

		public Task Show()
		{
			return transitionHandler.Show(gameObject, InTransition);
		}

		public Task Hide()
		{
			return transitionHandler.Hide(gameObject, OutTransition);
		}
	}
}
