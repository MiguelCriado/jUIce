namespace Maui
{
	public class OperatorBinderCollectionViewModel<T> : OperatorBinderViewModel
	{
		public IReadOnlyObservableCollection<T> Value { get; }

		public OperatorBinderCollectionViewModel(IReadOnlyObservableCollection<T> value)
		{
			Value = value;
		}
	}
}