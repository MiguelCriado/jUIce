namespace Maui
{
	public class CommandBoxer<T, U> : IObservableCommand<T> where U : struct, T
	{
		public event ObservableCommandEventHandler<T> ExecuteRequested;

		public IObservableVariable<bool> CanExecute => boxedCommand.CanExecute;

		private readonly IObservableCommand<U> boxedCommand;

		public CommandBoxer(IObservableCommand<U> boxedCommand)
		{
			this.boxedCommand = boxedCommand;
		}
		
		public bool Execute(T parameter)
		{
			return boxedCommand.Execute((U)parameter);
		}
	}
}