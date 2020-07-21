using System.Threading.Tasks;
using UnityEngine;

namespace Maui
{
	public class PanelLayer : BaseLayer<IPanelController>
	{
		internal PanelPriorityLayerList PriorityLayers { get => priorityLayers; set => priorityLayers = value; }

		[SerializeField] private PanelPriorityLayerList priorityLayers = null;

		public override void ReparentScreen(IScreenController controller, Transform screenTransform)
		{
			IPanelController panelController = controller as IPanelController;

			if (panelController != null)
			{
				ReparentToParaLayer(panelController.Priority, screenTransform);
			}
			else
			{
				base.ReparentScreen(controller, screenTransform);
			}
		}

		public override Task ShowScreen(IPanelController screen)
		{
			return screen.Show(null);
		}

		public override Task ShowScreen<T>(IPanelController screen, T viewModel) 
		{
			return screen.Show(viewModel);
		}

		public override Task HideScreen(IPanelController screen)
		{
			return screen.Hide();
		}

		public bool IsPanelVisible<T>()
		{
			bool result = false;
			IPanelController panel;

			if (registeredScreens.TryGetValue(typeof(T), out panel))
			{
				result = panel.IsVisible;
			}

			return result;
		}

		private void ReparentToParaLayer(PanelPriority priority, Transform screenTransform)
		{
			Transform parentTransform;

			if (priorityLayers.ParaLayerLookup.TryGetValue(priority, out parentTransform) == false)
			{
				parentTransform = transform;
			}

			screenTransform.SetParent(parentTransform, false);
		}
	}
}
