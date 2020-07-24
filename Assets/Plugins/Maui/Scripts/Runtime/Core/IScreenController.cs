using System.Threading.Tasks;

namespace Maui
{
	public delegate void ScreenControllerEventHandler(IScreenController controller);

	public interface IScreenController
	{
		event ScreenControllerEventHandler InTransitionFinished;
		event ScreenControllerEventHandler OutTransitionFinished;
		event ScreenControllerEventHandler CloseRequested;
		event ScreenControllerEventHandler ScreenDestroyed;

		bool IsVisible { get; }
		
		Task Show(IViewModel viewModel);

		Task Hide(bool animate = true);
	}
}
