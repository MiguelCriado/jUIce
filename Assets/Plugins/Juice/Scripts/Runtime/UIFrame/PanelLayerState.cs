using System.Collections.Generic;

namespace Juice
{
	public class PanelLayerState
	{
		public IEnumerable<PanelStateEntry> VisiblePanels { get; }

		public PanelLayerState(IEnumerable<PanelStateEntry> visiblePanels)
		{
			VisiblePanels = visiblePanels;
		}
	}
}
