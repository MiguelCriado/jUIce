using System.Threading.Tasks;

namespace Muui
{
	public delegate void ScreenControllerEventHandler(IScreenController controller);

	public interface IScreenController
	{
		event ScreenControllerEventHandler InTransitionFinished;
		event ScreenControllerEventHandler OutTransitionFinished;
		event ScreenControllerEventHandler CloseRequested;
		event ScreenControllerEventHandler ScreenDestroyed;

		bool IsVisible { get; }

		Task Show(IScreenProperties properties = null);
		Task Hide(bool animate = true);
	}
}
