namespace Juice
{
	public interface IPanel : IView
	{
		PanelPriority Priority { get; }
	}
}
