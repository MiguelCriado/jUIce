namespace Juice
{
	public class PanelStateEntry
	{
		public IPanel Panel { get; }
		public PanelShowSettings Settings { get; }

		public PanelStateEntry(IPanel panel, PanelShowSettings settings)
		{
			Panel = panel;
			Settings = settings;
		}
	}
}