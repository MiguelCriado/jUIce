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
	}
}
