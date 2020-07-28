using System.Threading.Tasks;

namespace Maui
{
	public struct WindowHistoryEntry
	{
		public readonly IWindow View;
		public readonly IViewModel ViewModel;

		public WindowHistoryEntry(IWindow view, IViewModel viewModel)
		{
			View = view;
			ViewModel = viewModel;
		}

		public Task Show()
		{
			return View.Show(ViewModel);
		}
	}
}
