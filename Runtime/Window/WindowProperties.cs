using System;
using UnityEngine;

namespace Muui
{
	[Serializable]
	public class WindowProperties : IWindowProperties
	{
		public WindowPriority WindowQueuePriority
		{
			get => windowQueuePriority;
			set => windowQueuePriority = value;
		}

		public bool HideOnForegroundLost
		{
			get => hideOnForegroundLost;
			set => hideOnForegroundLost = value;
		}

		public bool IsPopup
		{
			get => isPopup;
			set => isPopup = value;
		}

		public bool SupressPrefabProperties { get; set; }

		[SerializeField] private WindowPriority windowQueuePriority = WindowPriority.ForceForeground;
		[SerializeField] private bool hideOnForegroundLost = true;
		[SerializeField] private bool isPopup;

		public WindowProperties(bool supressPrefabPropertes = false)
		{
			WindowQueuePriority = WindowPriority.ForceForeground;
			HideOnForegroundLost = false;
			SupressPrefabProperties = supressPrefabPropertes;
		}

		public WindowProperties(WindowPriority priority, bool hideOnForegroundLost = false, bool supressPrefabProperties = false)
		{
			WindowQueuePriority = priority;
			HideOnForegroundLost = hideOnForegroundLost;
			SupressPrefabProperties = supressPrefabProperties;
		}
	}
}
