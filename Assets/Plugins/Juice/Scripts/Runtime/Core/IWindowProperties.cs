namespace Juice
{
	public interface IWindowProperties : IViewProperties
	{
		WindowPriority WindowQueuePriority { get; set; }
		bool HideOnForegroundLost { get; set; }
		bool IsPopup { get; set; }
		bool CloseOnShadowClick { get; set; }
		bool SupressPrefabProperties { get; set; }
	}
}
