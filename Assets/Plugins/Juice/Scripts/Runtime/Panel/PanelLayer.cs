using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public class PanelLayer : Layer<IPanel, PanelShowSettings, PanelHideSettings>
	{
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

		protected override async Task ShowView(IPanel view, PanelShowSettings settings)
		{
			view.SetViewModel(settings.ViewModel);

			await view.Show(settings.ShowTransition);
		}

		protected override async Task HideView(IPanel view, PanelHideSettings settings)
		{
			await view.Hide(settings?.Transition);
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
