using System.Threading.Tasks;

namespace Juice
{
	public delegate void TransitionableEventHandler(ITransitionable transitionable);

	public interface ITransitionable
	{
		event TransitionableEventHandler Showing;
		event TransitionableEventHandler Shown;
		event TransitionableEventHandler Hiding;
		event TransitionableEventHandler Hidden;

		bool IsVisible { get; }

		Task Show(Transition overrideTransition = null);
		Task Hide(Transition overrideTransition = null);
	}
}
