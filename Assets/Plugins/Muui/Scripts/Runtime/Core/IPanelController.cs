namespace Muui
{
	public interface IPanelController : IScreenController
	{
		PanelPriority Priority { get; }
	}
}
