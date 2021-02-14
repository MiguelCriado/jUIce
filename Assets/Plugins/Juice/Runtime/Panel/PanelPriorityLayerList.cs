using System;
using System.Collections.Generic;
using UnityEngine;

namespace Juice
{
	[Serializable]
	public class PanelPriorityLayerList
	{
		[SerializeField] private List<PanelPriorityLayerListEntry> paralayers = null;

		private Dictionary<PanelPriority, Transform> lookup;

		public Dictionary<PanelPriority, Transform> ParaLayerLookup
		{
			get
			{
				if (lookup == null || lookup.Count == 0)
				{
					CacheLookup();
				}

				return lookup;
			}
		}

		public PanelPriorityLayerList(List<PanelPriorityLayerListEntry> entries)
		{
			paralayers = entries;
		}

		private void CacheLookup()
		{
			lookup = new Dictionary<PanelPriority, Transform>();

			foreach (var current in paralayers)
			{
				lookup.Add(current.Priority, current.TargetTransform);
			}
		}
	}
}
