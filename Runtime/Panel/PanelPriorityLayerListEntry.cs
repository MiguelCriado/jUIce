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

		public Transform TargetTransform
		{
			get => targetTransform;
			set => targetTransform = value;
		}

		[SerializeField] private PanelPriority priority;
		[SerializeField] private Transform targetTransform;

		public PanelPriorityLayerListEntry(PanelPriority priority, Transform targetTransform)
		{
			this.priority = priority;
			this.targetTransform = targetTransform;
		}
	}
}
