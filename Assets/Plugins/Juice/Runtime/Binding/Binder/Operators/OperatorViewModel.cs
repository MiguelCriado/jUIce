namespace Juice
{
	public abstract class OperatorViewModel : IViewModel
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
