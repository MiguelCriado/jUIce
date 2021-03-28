namespace Juice
{
	public interface IWindow : IView
	{
		bool HideOnForegroundLost { get; }
		bool IsPopup { get; }
		bool CloseOnShadowClick { get; }
		WindowPriority WindowPriority { get; }

		void SetLayer(WindowLayer layer);
	}
}
