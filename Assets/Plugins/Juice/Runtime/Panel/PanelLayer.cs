using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public class PanelLayer : Layer<IPanel, PanelShowSettings, PanelHideSettings>
	{
		internal PanelPriorityLayerList PriorityLayers { get => priorityLayers; set => priorityLayers = value; }

		[SerializeField] private PanelPriorityLayerList priorityLayers = null;

		private readonly Dictionary<Type, PanelStateEntry> visiblePanels = new Dictionary<Type, PanelStateEntry>();

		public PanelLayerState GetCurrentState()
		{
			return new PanelLayerState(visiblePanels.Values.ToArray());
		}

		public void SetState(PanelLayerState state)
		{
			visiblePanels.Clear();
			var visiblePanelsSet = new HashSet<IPanel>();

			foreach (PanelStateEntry current in state.VisiblePanels)
			{
				visiblePanels[current.GetType()] = current;
				visiblePanelsSet.Add(current.Panel);
				current.Panel.SetViewModel(current.Settings.ViewModel);
			}

			foreach (var kvp in registeredViews)
			{
				if (visiblePanelsSet.Contains(kvp.Value))
				{
					kvp.Value.Show(Transition.Null);
				}
				else
				{
					kvp.Value.Hide(Transition.Null);
					kvp.Value.SetViewModel(default);
				}
			}
		}

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

		public bool IsPanelVisible<T>()
		{
			bool result = false;

			if (registeredViews.TryGetValue(typeof(T), out IPanel panel))
			{
				result = panel.IsVisible;
			}

			return result;
		}

		protected override async Task ShowView(IPanel view, PanelShowSettings settings)
		{
			visiblePanels[view.GetType()] = new PanelStateEntry(view, settings);
			PanelPriority finalPriority = settings.Priority ?? view.Priority;
			ReparentToParaLayer(finalPriority, ((Component)view).transform);
			view.SetViewModel(settings.ViewModel);

			await view.Show(settings.ShowTransition);
		}

		protected override async Task HideView(IPanel view, PanelHideSettings settings)
		{
			visiblePanels.Remove(view.GetType());

			await view.Hide(settings?.HideTransition);

			view.SetViewModel(default);
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
