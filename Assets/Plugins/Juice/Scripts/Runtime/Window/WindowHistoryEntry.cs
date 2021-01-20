using System.Threading.Tasks;

namespace Juice
{
	public readonly struct WindowHistoryEntry
	{
		public readonly IWindow View;
		public readonly WindowShowSettings Settings;

		public WindowHistoryEntry(IWindow view, WindowShowSettings settings)
		{
			View = view;
			Settings = settings;
		}

		public Task Show()
		{
			return View.Show(Settings.ViewModel, Settings.InTransition);
		}
	}
}
