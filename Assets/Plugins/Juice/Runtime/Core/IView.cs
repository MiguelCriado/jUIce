namespace Juice
{
	public delegate void ViewEventHandler(IView controller);

	public interface IView : ITransitionable
	{
		event ViewEventHandler CloseRequested;
		event ViewEventHandler ViewDestroyed;

		bool AllowsInteraction { get; set; }

		void SetViewModel(IViewModel viewModel);
	}
}
