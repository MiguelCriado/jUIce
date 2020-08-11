namespace Maui
{
	public class ValueConverterViewModel<T> : IViewModel
	{
		public IReadOnlyObservableVariable<T> ConvertedValue { get; }

		public ValueConverterViewModel(IReadOnlyObservableVariable<T> convertedValue)
		{
			ConvertedValue = convertedValue;
		}
	}
}