using System;
using System.Collections.Generic;
using Juice.Utils;

namespace Juice.Editor
{
	public static class BindingInfoTracker
	{
		private static readonly List<WeakReference<BindingInfoDrawer>> aliveDrawers = new List<WeakReference<BindingInfoDrawer>>();

		static BindingInfoTracker()
		{
			BindingInfoTrackerProxy.RefreshBindingInfoRequested += RefreshBindingInfoDrawers;
		}
		
		public static void Register(BindingInfoDrawer drawer)
		{
			aliveDrawers.Add(new WeakReference<BindingInfoDrawer>(drawer));
		}

		public static void Unregister(BindingInfoDrawer drawer)
		{
			bool found = false;
			int i = 0; 

			while (found == false && i < aliveDrawers.Count)
			{
				if (aliveDrawers[i].TryGetTarget(out BindingInfoDrawer current) && current == drawer)
				{
					found = true;
					aliveDrawers.RemoveAt(i);
				}

				i++;
			}
		}

		public static void RefreshBindingInfoDrawers()
		{
			foreach (var current in aliveDrawers)
			{
				if (current.TryGetTarget(out BindingInfoDrawer target))
				{
					target.SetDirty();
				}
			}
		}
	}
}