namespace Juice
{
	public class CommandBoxer<TExposed, TBoxed> : IObservableCommand<TExposed> where TBoxed : struct, TExposed
	{
		public event ObservableCommandEventHandler<TExposed> ExecuteRequested;

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
