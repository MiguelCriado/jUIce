namespace Maui
{
	public class CommandValueConverterViewModel<T> : IViewModel
	{
		public IObservableCommand<T> ConvertedValue { get; }

		public CommandValueConverterViewModel(IObservableCommand<T> convertedValue)
		{
			ConvertedValue = convertedValue;
		}
	}
}