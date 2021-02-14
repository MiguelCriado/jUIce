namespace Juice
{
	public class CommandViewModel : ViewModel
	{
		public IObservableCommand Value { get; }

		public CommandViewModel(IObservableCommand value)
		{
			Value = value;
		}
	}
}