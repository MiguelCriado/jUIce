using System.Threading.Tasks;
using ICSharpCode.NRefactory.Ast;
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
		private bool isInitialized;

		protected virtual void Awake()
		{
			Initialize();
		}

		public Task Show()
		{
			Initialize();
			return transitionHandler.Show(rectTransform, InTransition);
		}

		public Task Hide()
		{
			Initialize();
			return transitionHandler.Hide(rectTransform, OutTransition);
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
