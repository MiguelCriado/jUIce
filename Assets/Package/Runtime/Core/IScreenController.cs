using System.Threading.Tasks;

namespace Muui
{
	public delegate void ScreenControllerDelegate(IScreenController controller);

	public interface IScreenController
	{
		event ScreenControllerDelegate OnInTransitionFinished;
		event ScreenControllerDelegate OnOutTransitionFinished;
		event ScreenControllerDelegate OnCloseRequest;
		event ScreenControllerDelegate OnScreenDestroyed;

		bool IsVisible { get; }

		Task Show(IScreenProperties properties = null);
		Task Hide(bool animate = true);
	}
}
