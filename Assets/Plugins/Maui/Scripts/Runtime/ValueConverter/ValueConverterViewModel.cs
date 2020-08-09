namespace Maui
{
	public class ValueConverterViewModel<T> : IViewModel
	{
		public IReadOnlyObservableVariable<T> ConvertedValue => convertedValue;

		private readonly IReadOnlyObservableVariable<T> convertedValue;

		public ValueConverterViewModel(IReadOnlyObservableVariable<T> convertedValue)
		{
			this.convertedValue = convertedValue;
		}
	}
}