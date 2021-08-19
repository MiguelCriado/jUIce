using System;
using System.Collections.Generic;
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

	public abstract class Window<T> : View<T>, IWindow where T : IViewModel, new()
	{
		public delegate void FocusEventHandler(IWindow window);

		public event FocusEventHandler FocusGained;
		public event FocusEventHandler FocusLost;

		public bool HasFocus { get; private set; }
		public WindowPriority WindowPriority => windowQueuePriority;
		public bool HideOnForegroundLost => hideOnForegroundLost;
		public bool IsPopup
		{
			get => isPopup;
			set => isPopup = value;
		}
		public bool CloseOnShadowClick => closeOnShadowClick;

		[Header("Window Properties")]
		[SerializeField] private bool isPopup;
		[SerializeField] private WindowPriority windowQueuePriority = WindowPriority.ForceForeground;
		[SerializeField] private bool hideOnForegroundLost = true;
		[DrawIf(nameof(isPopup), true)]
		[SerializeField] private bool closeOnShadowClick = true;
		[SerializeField] private WindowEvents windowEvents = new WindowEvents();

		private WindowLayer currentLayer;
		private Dictionary<string, object> payload;

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (currentLayer)
			{
				currentLayer.CurrentWindowChanged -= OnLayerWindowChanged;
			}
		}

		public override void SetViewModel(IViewModel viewModel)
		{
			base.SetViewModel(viewModel);

			if (viewModel == null)
			{
				SetViewModel(new T());
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
		
		public IViewModel GetNewViewModel()
		{
			return new T();
		}
		
		public void SetPayload(Dictionary<string, object> payload)
		{
			this.payload = payload;
		}

		public bool GetFromPayload<TValue>(string key, out TValue value)
		{
			bool found = false;
			value = default;

			if (payload != null && payload.TryGetValue(key, out object rawValue))
			{
				value = (TValue) rawValue;
				found = true;
			}

			return found;
		}

		public bool RemoveFromPayload(string key)
		{
			bool removed = false;

			if (payload != null)
			{
				removed = payload.Remove(key);
			}

			return removed;
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
			HasFocus = ReferenceEquals(newWindow, this);

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
