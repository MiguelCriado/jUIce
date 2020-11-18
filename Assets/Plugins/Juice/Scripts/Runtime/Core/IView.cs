using System.Threading.Tasks;

namespace Juice
{
	public delegate void ViewEventHandler(IView controller);

	public interface IView
	{
		event ViewEventHandler InTransitionFinished;
		event ViewEventHandler OutTransitionFinished;
		event ViewEventHandler CloseRequested;
		event ViewEventHandler ViewDestroyed;

		bool IsVisible { get; }
		bool AllowInteraction { get; set; }
		
		Task Show(IViewModel viewModel, Transition overrideTransition = null);

		Task Hide(bool animate = true);
	}
}
