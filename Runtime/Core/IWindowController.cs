namespace Muui
{
	public interface IWindowController : IScreenController
	{
		bool HideOnForegroundLost { get; }
		bool IsPopup { get; }
		WindowPriority WindowPriority { get; }
	}
}
