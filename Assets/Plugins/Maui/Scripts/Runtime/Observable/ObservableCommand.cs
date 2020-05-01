namespace Maui
{
	public class ObservableCommand : IObservableCommand
	{
		public event ObservableCommandEventHandler ExecuteRequested;

		public IReadOnlyObservableVariable<bool> CanExecute { get; }

		public ObservableCommand(IObservableVariable<bool> canExecuteSource)
		{
			CanExecute = canExecuteSource;
		}

		public ObservableCommand() : this(new ObservableVariable<bool>(true))
		{

		}

		public ObservableCommand(ObservableCommandEventHandler onRequestCallback) : this()
		{
			ExecuteRequested = onRequestCallback;
		}

		public ObservableCommand(IObservableVariable<bool> canExecuteSource, ObservableCommandEventHandler onRequestCallback) : this(canExecuteSource)
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
		public event ObservableCommandEventHandler<T> ExecuteRequested;

		public IReadOnlyObservableVariable<bool> CanExecute { get; }

		public ObservableCommand(IObservableVariable<bool> canExecuteSource)
		{
			CanExecute = canExecuteSource;
		}

		public ObservableCommand() : this(new ObservableVariable<bool>(true))
		{

		}

		public ObservableCommand(ObservableCommandEventHandler<T> onRequestCallback) : this()
		{
			ExecuteRequested = onRequestCallback;
		}

		public ObservableCommand(IObservableVariable<bool> canExecuteSource, ObservableCommandEventHandler<T> onRequestCallback) : this(canExecuteSource)
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
