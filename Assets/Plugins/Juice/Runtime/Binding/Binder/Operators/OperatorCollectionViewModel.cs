namespace Juice
{
	public class OperatorCollectionViewModel<T> : OperatorViewModel
	{
		public IReadOnlyObservableCollection<T> Value { get; }

		public OperatorCollectionViewModel(IReadOnlyObservableCollection<T> value)
		{
			Value = value;
		}
	}
}
