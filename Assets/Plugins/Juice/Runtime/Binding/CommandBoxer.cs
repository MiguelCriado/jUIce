namespace Juice
{
	public class CommandBoxer<TExposed, TBoxed> : IObservableCommand<TExposed> where TBoxed : struct, TExposed
	{
#pragma warning disable 67
		public event ObservableCommandEventHandler<TExposed> ExecuteRequested;
#pragma warning restore 67

		public IObservableVariable<bool> CanExecute => boxedCommand.CanExecute;

		private readonly IObservableCommand<TBoxed> boxedCommand;

		public CommandBoxer(IObservableCommand<TBoxed> boxedCommand)
		{
			this.boxedCommand = boxedCommand;
		}

		public bool Execute(TExposed parameter)
		{
			return boxedCommand.Execute((TBoxed)parameter);
		}
	}
}
