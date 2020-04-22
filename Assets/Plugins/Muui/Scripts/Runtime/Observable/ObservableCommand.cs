namespace Muui
{
	public class ObservableCommand : IObservableCommand
	{
		public event ObservableCommandDelegate OnRequestExecute;

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
			OnRequestExecute = onRequestCallback;
		}

		public ObservableCommand(IObservableVariable<bool> canExecuteSource, ObservableCommandDelegate onRequestCallback) : this(canExecuteSource)
		{
			OnRequestExecute = onRequestCallback;
		}

		public bool Execute()
		{
			bool result = false;

			if (CanExecute.Value)
			{
				OnRequestExecute?.Invoke();
				result = true;
			}

			return result;
		}
	}

	public class ObservableCommand<T> : IObservableCommand<T>
	{
		public event ObservableCommandDelegate<T> OnRequestExecute;

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
			OnRequestExecute = onRequestCallback;
		}

		public ObservableCommand(IObservableVariable<bool> canExecuteSource, ObservableCommandDelegate<T> onRequestCallback) : this(canExecuteSource)
		{
			OnRequestExecute = onRequestCallback;
		}

		public bool Execute(T parameter)
		{
			bool result = false;

			if (CanExecute.Value)
			{
				OnRequestExecute?.Invoke(parameter);
				result = true;
			}

			return result;
		}
	}
}
