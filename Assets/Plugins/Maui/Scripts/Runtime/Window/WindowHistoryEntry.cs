using System.Threading.Tasks;

namespace Maui
{
	public struct WindowHistoryEntry
	{
		public readonly IWindowController Screen;
		public readonly IViewModel ViewModel;

		public WindowHistoryEntry(IWindowController screen, IViewModel viewModel)
		{
			Screen = screen;
			ViewModel = viewModel;
		}

		public Task Show()
		{
			return Screen.Show(ViewModel);
		}
	}
}
