using System.Threading.Tasks;

namespace Muui
{
	public struct WindowHistoryEntry
	{
		public readonly IWindowController Screen;
		public readonly IWindowProperties Properties;

		public WindowHistoryEntry(IWindowController screen, IWindowProperties properties)
		{
			Screen = screen;
			Properties = properties;
		}

		public Task Show()
		{
			return Screen.Show(Properties);
		}
	}
}
