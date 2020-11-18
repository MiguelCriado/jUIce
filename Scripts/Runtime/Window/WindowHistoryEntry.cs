using System.Threading.Tasks;

namespace Juice
{
	public readonly struct WindowHistoryEntry
	{
		public readonly IWindow View;
		public readonly IViewModel ViewModel;
		public readonly WindowOptions OverrideOptions;

		public WindowHistoryEntry(IWindow view, IViewModel viewModel, WindowOptions overrideOptions)
		{
			View = view;
			ViewModel = viewModel;
			OverrideOptions = overrideOptions;
		}

		public Task Show()
		{
			return View.Show(ViewModel, OverrideOptions?.InTransition);
		}
	}
}
