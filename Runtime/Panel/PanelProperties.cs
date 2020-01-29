using System;
using UnityEngine;

namespace Muui
{
	[Serializable]
	public class PanelProperties : IPanelProperties
	{
		public PanelPriority Priority
		{
			get => priority;
			set => priority = value;
		}

		[SerializeField] private PanelPriority priority;
	}
}
