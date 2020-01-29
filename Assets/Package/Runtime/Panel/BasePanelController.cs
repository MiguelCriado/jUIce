namespace Muui
{
	public abstract class BasePanelController : BasePanelController<PanelProperties>
	{

	}

	public abstract class BasePanelController<T> : BaseScreenController<T>, IPanelController
		where T : IPanelProperties
	{
		public PanelPriority Priority
		{
			get
			{
				if (Properties != null)
				{
					return Properties.Priority;
				}
				else
				{
					return PanelPriority.None;
				}
			}
		}

		protected sealed override void SetProperties(T properties)
		{
			base.SetProperties(properties);
		}
	}
}
