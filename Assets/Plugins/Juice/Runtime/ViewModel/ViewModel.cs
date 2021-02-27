namespace Juice
{
	public abstract class ViewModel : IViewModel
	{
		public bool IsEnabled { get; private set; }

		public void Enable()
		{
			IsEnabled = true;
			OnEnable();
		}

		public void Disable()
		{
			IsEnabled = false;
			OnDisable();
		}

		protected virtual void OnEnable()
		{

		}

		protected virtual void OnDisable()
		{

		}
	}
}
