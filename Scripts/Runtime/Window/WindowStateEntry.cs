namespace Juice
{
	public class WindowStateEntry
	{
		public IWindow Window { get; }
		public WindowShowSettings Settings { get; }
		public bool IsVisible { get; }

		public WindowStateEntry(IWindow window, WindowShowSettings settings, bool isVisible)
		{
			Window = window;
			Settings = settings;
			IsVisible = isVisible;
		}
	}
}