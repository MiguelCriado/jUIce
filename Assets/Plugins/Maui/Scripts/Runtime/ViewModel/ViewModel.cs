using System;

namespace Maui
{
	public abstract class ViewModel : IViewModel, IDisposable
	{
		private Action updateMethod;
		
		~ViewModel()
		{
			Dispose(false);
		}
		
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void RegisterUpdateMethod(Action methodCallback)
		{
			UnsubscribeUpdateMethod(updateMethod);
			updateMethod = methodCallback;
			LifecycleUtils.OnUpdate += updateMethod;
		}
		
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				UnsubscribeUpdateMethod(updateMethod);
			}
		}

		private void UnsubscribeUpdateMethod(Action methodCallback)
		{
			if (methodCallback != null)
			{
				LifecycleUtils.OnUpdate -= methodCallback;
			}
		}
	}
}