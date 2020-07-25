namespace Maui
{
	public abstract class ViewModel<T> : IViewModel
	{
		public ViewModel(T data)
		{
			Initialize(data);
		}

		public void SetData(object data)
		{
			if (data is T dataAsType)
			{
				Initialize(dataAsType);
			}
		}

		protected abstract void Initialize(T data);
	}
}