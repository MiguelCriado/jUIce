namespace Juice
{
	public abstract class ViewModel : IViewModel
	{
		public bool IsEnabled { get; private set; }

		public void Enable()
		{
			IsEnabled = true;
			LifecycleUtils.OnUpdate += Update;
		}

		public void Disable()
		{
			LifecycleUtils.OnUpdate -= Update;
			IsEnabled = false;
		}

		protected virtual void OnEnable()
		{
			
		}

		protected virtual void OnDisable()
		{
			
		}

		protected virtual void Update()
		{
			
		}
	}
}