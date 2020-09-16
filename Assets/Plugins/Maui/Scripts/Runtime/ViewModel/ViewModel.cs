namespace Maui
{
	public abstract class ViewModel : IViewModel
	{
		public bool IsEnabled => isEnabled;
		
		private bool isEnabled;

		public void Enable()
		{
			isEnabled = true;
			LifecycleUtils.OnUpdate += Update;
		}

		public void Disable()
		{
			LifecycleUtils.OnUpdate -= Update;
			isEnabled = false;
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