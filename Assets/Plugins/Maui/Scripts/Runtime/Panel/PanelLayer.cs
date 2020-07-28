using System.Threading.Tasks;
using UnityEngine;

namespace Maui
{
	public class PanelLayer : BaseLayer<IPanel>
	{
		internal PanelPriorityLayerList PriorityLayers { get => priorityLayers; set => priorityLayers = value; }

		[SerializeField] private PanelPriorityLayerList priorityLayers = null;

		public override void ReparentView(IView view, Transform viewTransform)
		{
			IPanel panel = view as IPanel;

			if (panel != null)
			{
				ReparentToParaLayer(panel.Priority, viewTransform);
			}
			else
			{
				base.ReparentView(view, viewTransform);
			}
		}

		public override Task ShowView(IPanel view)
		{
			return view.Show(null);
		}

		public override Task ShowView<T>(IPanel view, T viewModel) 
		{
			return view.Show(viewModel);
		}

		public override Task HideView(IPanel view)
		{
			return view.Hide();
		}

		public bool IsPanelVisible<T>()
		{
			bool result = false;
			IPanel panel;

			if (registeredViews.TryGetValue(typeof(T), out panel))
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
