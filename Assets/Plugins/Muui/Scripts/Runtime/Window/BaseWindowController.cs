namespace Maui
{
	public abstract class BaseWindowController : BaseWindowController<WindowProperties>
	{

	}

	public abstract class BaseWindowController<T> : BaseScreenController<T>, IWindowController
		where T : IWindowProperties
	{
		public bool HideOnForegroundLost => CurrentProperties.HideOnForegroundLost;
		public bool IsPopup => CurrentProperties.IsPopup;
		public bool CloseOnShadowClick => CurrentProperties.CloseOnShadowClick;
		public WindowPriority WindowPriority => CurrentProperties.WindowQueuePriority;

		protected override void SetProperties(T properties)
		{
			if (properties != null)
			{
				if (properties.SupressPrefabProperties == false)
				{
					properties.HideOnForegroundLost = CurrentProperties.HideOnForegroundLost;
					properties.WindowQueuePriority = CurrentProperties.WindowQueuePriority;
					properties.IsPopup = CurrentProperties.IsPopup;
				}

				CurrentProperties = properties;
			}
		}

		protected override void OnShowing()
		{
			base.OnShowing();

			transform.SetAsLastSibling();
		}
	}
}
