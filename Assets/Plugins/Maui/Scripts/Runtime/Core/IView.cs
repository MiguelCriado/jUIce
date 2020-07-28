using System.Threading.Tasks;

namespace Maui
{
	public delegate void ViewEventHandler(IView controller);

	public interface IView
	{
		event ViewEventHandler InTransitionFinished;
		event ViewEventHandler OutTransitionFinished;
		event ViewEventHandler CloseRequested;
		event ViewEventHandler ViewDestroyed;

		bool IsVisible { get; }
		
		Task Show(IViewModel viewModel);

		Task Hide(bool animate = true);
	}
}
