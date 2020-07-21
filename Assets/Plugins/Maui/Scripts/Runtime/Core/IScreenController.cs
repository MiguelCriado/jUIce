using System.Threading.Tasks;

namespace Maui
{
	public delegate void ScreenControllerEventHandler(IScreenController controller);

	public interface IScreenController : IScreenController<IViewModel>
	{
		
	}
	
	public interface IScreenController<in T>  where T : IViewModel
	{
		event ScreenControllerEventHandler InTransitionFinished;
		event ScreenControllerEventHandler OutTransitionFinished;
		event ScreenControllerEventHandler CloseRequested;
		event ScreenControllerEventHandler ScreenDestroyed;

		bool IsVisible { get; }
		
		Task Show(T viewModel);

		Task Hide(bool animate = true);
	}
}
