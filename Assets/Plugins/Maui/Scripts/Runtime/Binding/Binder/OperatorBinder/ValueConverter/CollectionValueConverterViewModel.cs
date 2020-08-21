namespace Maui
{
	public class CollectionValueConverterViewModel<T> : IViewModel
	{
		public IReadOnlyObservableCollection<T> ConvertedValue { get; }

		public CollectionValueConverterViewModel(IReadOnlyObservableCollection<T> convertedValue)
		{
			ConvertedValue = convertedValue;
		}
	}
}