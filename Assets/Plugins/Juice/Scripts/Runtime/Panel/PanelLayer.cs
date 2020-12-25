using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public class PanelLayer : Layer<IPanel, PanelOptions>
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

		protected override async Task ShowView<T>(IPanel view, T viewModel, PanelOptions overrideOptions)
		{
			PanelOpening?.Invoke(view);
			
			await view.Show(viewModel);
			
			PanelOpened?.Invoke(view);
		}

		public override async Task HideView(IPanel view, PanelOptions overrideOptions = default)
		{
			PanelClosing?.Invoke(view);
			
			await view.Hide(overrideOptions?.Transition);
			
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
			Transform parentTransform;

			if (priorityLayers.ParaLayerLookup.TryGetValue(priority, out parentTransform) == false)
			{
				parentTransform = transform;
			}

			viewTransform.SetParent(parentTransform, false);
		}
	}
}
