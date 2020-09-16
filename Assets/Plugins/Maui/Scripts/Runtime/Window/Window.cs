using System.Threading.Tasks;
using UnityEngine;

namespace Maui
{
	public abstract class Window : Window<IViewModel>
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
		[SerializeField] private WindowPriority windowQueuePriority = WindowPriority.ForceForeground;
		[SerializeField] private bool hideOnForegroundLost = true;
		[SerializeField] private bool isPopup;
		[SerializeField] private bool closeOnShadowClick = true;

		public Task Show(IViewModel viewModel)
		{
			return base.Show((T) viewModel);
		}
		
		protected override void OnShowing()
		{
			base.OnShowing();

			transform.SetAsLastSibling();
		}
	}
}
