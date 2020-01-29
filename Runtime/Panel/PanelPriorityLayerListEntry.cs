using System;
using UnityEngine;

namespace Muui
{
	[Serializable]
	public class PanelPriorityLayerListEntry
	{
		public PanelPriority Priority
		{
			get => priority;
			set => priority = value;
		}

		public Transform TargetParent
		{
			get => targetParent;
			set => targetParent = value;
		}

		[SerializeField]private PanelPriority priority;
		[SerializeField] private Transform targetParent;

		public PanelPriorityLayerListEntry(PanelPriority priority, Transform targetParent)
		{
			this.priority = priority;
			this.targetParent = targetParent;
		}
	}
}
