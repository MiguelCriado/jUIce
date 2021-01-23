using UnityEngine;

namespace Juice
{
	public abstract class Window : Window<NullViewModel>
	{

	}

	public abstract class Window<T> : View<T>, IWindow
		where T : IViewModel
	{
		public WindowPriority WindowPriority => windowQueuePriority;
		public bool HideOnForegroundLost => hideOnForegroundLost;
		public bool IsPopup => isPopup;
		public bool CloseOnShadowClick => closeOnShadowClick;

		[Header("Window Properties")]
		[SerializeField] private bool isPopup;
		[SerializeField] private WindowPriority windowQueuePriority = WindowPriority.ForceForeground;
		[SerializeField] private bool hideOnForegroundLost = true;
		[DrawIf(nameof(isPopup), true)]
		[SerializeField] private bool closeOnShadowClick = true;

		protected override void OnShowing()
		{
			base.OnShowing();

			transform.SetAsLastSibling();
		}
	}
}
