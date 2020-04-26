using System.Threading.Tasks;

namespace Muui
{
	public delegate void ScreenControllerDelegate(IScreenController controller);

	public interface IScreenController
	{
		event ScreenControllerDelegate InTransitionFinished;
		event ScreenControllerDelegate OutTransitionFinished;
		event ScreenControllerDelegate CloseRequested;
		event ScreenControllerDelegate ScreenDestroyed;

		bool IsVisible { get; }

		Task Show(IScreenProperties properties = null);
		Task Hide(bool animate = true);
	}
}
