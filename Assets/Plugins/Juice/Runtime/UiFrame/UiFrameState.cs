namespace Juice
{
	public class UiFrameState
	{
		public WindowLayerState WindowLayerState { get; }
		public PanelLayerState PanelLayerState { get; }

		public UiFrameState(WindowLayerState windowLayerState, PanelLayerState panelLayerState)
		{
			WindowLayerState = windowLayerState;
			PanelLayerState = panelLayerState;
		}
	}
}
