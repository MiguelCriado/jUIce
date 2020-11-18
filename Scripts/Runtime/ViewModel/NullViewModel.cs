namespace Juice
{
	public sealed class NullViewModel : IViewModel
	{
		public bool IsEnabled { get; private set; }

		public void Enable()
		{
			IsEnabled = true;
		}

		public void Disable()
		{
			IsEnabled = false;
		}
	}
}