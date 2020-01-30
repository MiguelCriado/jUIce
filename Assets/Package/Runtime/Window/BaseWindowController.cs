namespace Muui
{
	public abstract class BaseWindowController : BaseWindowController<WindowProperties>
	{

	}

	public abstract class BaseWindowController<T> : BaseScreenController<T>, IWindowController
		where T : IWindowProperties
	{
		public bool HideOnForegroundLost { get => Properties.HideOnForegroundLost; }
		public bool IsPopup { get => Properties.IsPopup; }
		public WindowPriority WindowPriority { get => Properties.WindowQueuePriority; }

		protected override void SetProperties(T properties)
		{
			if (properties != null)
			{
				if (properties.SupressPrefabProperties == false)
				{
					properties.HideOnForegroundLost = Properties.HideOnForegroundLost;
					properties.WindowQueuePriority = Properties.WindowQueuePriority;
					properties.IsPopup = Properties.IsPopup;
				}

				Properties = properties;
			}
		}

		protected override void OnShow()
		{
			base.OnShow();

			transform.SetAsLastSibling();
		}
	}
}
