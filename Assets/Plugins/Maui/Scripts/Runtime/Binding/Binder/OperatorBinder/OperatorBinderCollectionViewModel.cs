namespace Maui
{
	public class OperatorBinderCollectionViewModel<T> : IViewModel
	{
		public IReadOnlyObservableCollection<T> Value { get; }

		public OperatorBinderCollectionViewModel(IReadOnlyObservableCollection<T> value)
		{
			Value = value;
		}

		public void Enable()
		{
			
		}

		public void Disable()
		{
			
		}
	}
}