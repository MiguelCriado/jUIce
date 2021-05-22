using Juice.Utils;

namespace Juice
{
	public class SingleSignalBus : Singleton<SingleSignalBus>
	{
		public SignalBus DefaultSignalBus { get; } = new SignalBus();
	}
}
