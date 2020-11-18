using System;

namespace Juice.Utils
{
	public class BindingInfoTrackerProxy
	{
		public static event Action RefreshBindingInfoRequested;

		public static void RefreshBindingInfo()
		{
			RefreshBindingInfoRequested?.Invoke();
		}
	}
}