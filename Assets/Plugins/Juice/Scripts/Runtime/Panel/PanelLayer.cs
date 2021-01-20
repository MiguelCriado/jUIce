using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public class PanelLayer : Layer<IPanel, PanelShowSettings, PanelHideSettings>
	{
		public delegate void PanelOperationHandler(IPanel panel);

		public event PanelOperationHandler PanelOpening;
		public event PanelOperationHandler PanelOpened;
		public event PanelOperationHandler PanelClosing;
		public event PanelOperationHandler PanelClosed;

		internal PanelPriorityLayerList PriorityLayers { get => priorityLayers; set => priorityLayers = value; }

		[SerializeField] private PanelPriorityLayerList priorityLayers = null;

		public override void ReparentView(IView view, Transform viewTransform)
		{
			if (view is IPanel panel)
			{
				ReparentToParaLayer(panel.Priority, viewTransform);
			}
			else
			{
				base.ReparentView(view, viewTransform);
			}
		}

		protected override void ShowView(IPanel view, PanelShowSettings settings)
		{
			PanelOpening?.Invoke(view);

			view.Show(settings.ViewModel, settings.InTransition).RunAndForget();

			PanelOpened?.Invoke(view);
		}

		protected override async Task HideView(IPanel view, PanelHideSettings settings)
		{
			PanelClosing?.Invoke(view);

			await view.Hide(settings?.OutTransition);

			PanelClosed?.Invoke(view);
		}

		public bool IsPanelVisible<T>()
		{
			bool result = false;

			if (registeredViews.TryGetValue(typeof(T), out IPanel panel))
			{
				result = panel.IsVisible;
			}

			return result;
		}

		private void ReparentToParaLayer(PanelPriority priority, Transform viewTransform)
		{
			if (priorityLayers.ParaLayerLookup.TryGetValue(priority, out Transform parentTransform) == false)
			{
				parentTransform = transform;
			}

			viewTransform.SetParent(parentTransform, false);
		}
	}
}
