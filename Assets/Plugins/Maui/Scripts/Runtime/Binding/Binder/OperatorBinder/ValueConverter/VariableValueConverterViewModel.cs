namespace Maui
{
	public class VariableValueConverterViewModel<T> : IViewModel
	{
		public IReadOnlyObservableVariable<T> ConvertedValue { get; }

		public VariableValueConverterViewModel(IReadOnlyObservableVariable<T> convertedValue)
		{
			ConvertedValue = convertedValue;
		}
	}
}