namespace Maui
{
	public abstract class ViewModel<T> : IViewModel
	{
		public ViewModel(T data)
		{
			SetData(data);
		}

		public void SetData(object data)
		{
			if (data is T dataAsType)
			{
				SetData(dataAsType);
			}
		}

		protected abstract void SetData(T data);
	}
}