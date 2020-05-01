namespace Maui
{
	public interface IPanelController : IScreenController
	{
		PanelPriority Priority { get; }
	}
}
