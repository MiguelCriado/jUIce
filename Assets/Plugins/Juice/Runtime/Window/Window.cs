using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Juice
{
	public abstract class Window : Window<NullViewModel>
	{

	}

	[Serializable]
	public class WindowEvents
	{
		public UnityEvent OnFocusGained = new UnityEvent();
		public UnityEvent OnFocusLost = new UnityEvent();
	}

	public abstract class Window<T> : View<T>, IWindow where T : IViewModel
	{
		public delegate void FocusEventHandler(IWindow window);

		public event FocusEventHandler FocusGained;
		public event FocusEventHandler FocusLost;

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
		[SerializeField] private WindowEvents windowEvents = new WindowEvents();

		private WindowLayer currentLayer;

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (currentLayer)
			{
				currentLayer.CurrentWindowChanged -= OnLayerWindowChanged;
			}
		}

		public void SetLayer(WindowLayer layer)
		{
			if (currentLayer)
			{
				currentLayer.CurrentWindowChanged -= OnLayerWindowChanged;
			}

			if (layer)
			{
				layer.CurrentWindowChanged += OnLayerWindowChanged;
			}

			currentLayer = layer;
		}

		public override Task Show(ITransition overrideTransition = null)
		{
			transform.SetAsLastSibling();

			return base.Show(overrideTransition);
		}

		protected virtual void OnFocusGained()
		{
			FocusGained?.Invoke(this);
			windowEvents.OnFocusGained.Invoke();
		}

		protected virtual void OnFocusLost()
		{
			FocusLost?.Invoke(this);
			windowEvents.OnFocusLost.Invoke();
		}

		private void OnLayerWindowChanged(IWindow oldWindow, IWindow newWindow, bool fromBack)
		{
			if (oldWindow != newWindow)
			{
				if (ReferenceEquals(oldWindow, this))
				{
					OnFocusLost();
				}
				else if (ReferenceEquals(newWindow, this))
				{
					OnFocusGained();
				}
			}
		}
	}
}
