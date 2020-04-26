namespace Muui
{
	public class ObservableCommand : IObservableCommand
	{
		public event ObservableCommandDelegate ExecuteRequested;

		public IReadOnlyObservableVariable<bool> CanExecute { get; }

		public ObservableCommand(IObservableVariable<bool> canExecuteSource)
		{
			CanExecute = canExecuteSource;
		}

		public ObservableCommand() : this(new ObservableVariable<bool>(true))
		{

		}

		public ObservableCommand(ObservableCommandDelegate onRequestCallback) : this()
		{
			ExecuteRequested = onRequestCallback;
		}

		public ObservableCommand(IObservableVariable<bool> canExecuteSource, ObservableCommandDelegate onRequestCallback) : this(canExecuteSource)
		{
			ExecuteRequested = onRequestCallback;
		}

		public bool Execute()
		{
			bool result = false;

			if (CanExecute.Value)
			{
				OnExecuteRequested();
				result = true;
			}

			return result;
		}

		protected virtual void OnExecuteRequested()
		{
			ExecuteRequested?.Invoke();
		}
	}

	public class ObservableCommand<T> : IObservableCommand<T>
	{
		public event ObservableCommandDelegate<T> ExecuteRequested;

		public IReadOnlyObservableVariable<bool> CanExecute { get; }

		public ObservableCommand(IObservableVariable<bool> canExecuteSource)
		{
			CanExecute = canExecuteSource;
		}

		public ObservableCommand() : this(new ObservableVariable<bool>(true))
		{

		}

		public ObservableCommand(ObservableCommandDelegate<T> onRequestCallback) : this()
		{
			ExecuteRequested = onRequestCallback;
		}

		public ObservableCommand(IObservableVariable<bool> canExecuteSource, ObservableCommandDelegate<T> onRequestCallback) : this(canExecuteSource)
		{
			ExecuteRequested = onRequestCallback;
		}

		public bool Execute(T parameter)
		{
			bool result = false;

			if (CanExecute.Value)
			{
				OnExecuteRequested(parameter);
				result = true;
			}

			return result;
		}

		protected virtual void OnExecuteRequested(T parameter)
		{
			ExecuteRequested?.Invoke(parameter);
		}
	}
}
