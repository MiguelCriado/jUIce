namespace Muui
{
	public abstract class BaseWindowController : BaseWindowController<WindowProperties>
	{

	}

	public abstract class BaseWindowController<T> : BaseScreenController<T>, IWindowController
		where T : IWindowProperties
	{
		public bool HideOnForegroundLost { get => CurrentProperties.HideOnForegroundLost; }
		public bool IsPopup { get => CurrentProperties.IsPopup; }
		public WindowPriority WindowPriority { get => CurrentProperties.WindowQueuePriority; }

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

		protected override void OnShow()
		{
			base.OnShow();

			transform.SetAsLastSibling();
		}
	}
}
