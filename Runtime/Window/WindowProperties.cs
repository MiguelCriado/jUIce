using System;
using System.Net.Configuration;
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

		public bool CloseOnShadowClick
		{
			get => closeOnShadowClick;
			set => closeOnShadowClick = value;
		}

		public bool SupressPrefabProperties { get; set; }

		[SerializeField] private WindowPriority windowQueuePriority = WindowPriority.ForceForeground;
		[SerializeField] private bool hideOnForegroundLost = true;
		[SerializeField] private bool isPopup;
		[SerializeField] private bool closeOnShadowClick = true;

		public WindowProperties(bool supressPrefabPropertes = false)
		{
			WindowQueuePriority = WindowPriority.ForceForeground;
			HideOnForegroundLost = false;
			SupressPrefabProperties = supressPrefabPropertes;
			CloseOnShadowClick = true;
		}

		public WindowProperties(WindowPriority priority, bool hideOnForegroundLost = false, bool supressPrefabProperties = false, bool closeOnShadowClick = true)
		{
			WindowQueuePriority = priority;
			HideOnForegroundLost = hideOnForegroundLost;
			SupressPrefabProperties = supressPrefabProperties;
			CloseOnShadowClick = closeOnShadowClick;
		}
	}
}
