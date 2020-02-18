namespace Muui
{
	public interface IWindowController : IScreenController
	{
		bool HideOnForegroundLost { get; }
		bool IsPopup { get; }
		bool CloseOnShadowClick { get; }
		WindowPriority WindowPriority { get; }
	}
}
