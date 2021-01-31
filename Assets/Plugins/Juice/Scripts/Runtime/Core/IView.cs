using System.Threading.Tasks;

namespace Juice
{
	public delegate void ViewEventHandler(IView controller);

	public interface IView
	{
		event ViewEventHandler CloseRequested;
		event ViewEventHandler ViewDestroyed;

		bool IsVisible { get; }
		bool AllowsInteraction { get; set; }

		void SetViewModel(IViewModel viewModel);
		Task Show(Transition overrideTransition = null);
		Task Hide(Transition overrideTransition = null);
	}
}
