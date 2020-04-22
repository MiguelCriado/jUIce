namespace Muui
{
	public class ObservableCommand : IObservableCommand
	{
		public event ObservableCommandDelegate OnRequestExecute;

		public IReadOnlyObservableVariable<bool> CanExecute => canExecute;

		private readonly ObservableVariable<bool> canExecute;

		public ObservableCommand()
		{
			canExecute = new ObservableVariable<bool>(true);
		}

		public ObservableCommand(IObservableVariable<bool> canExecuteSource)
		{
			canExecute = new ObservableVariable<bool>(canExecuteSource.Value);
			canExecuteSource.OnChange += OnCanExecuteChanges;
		}

		public bool Execute()
		{
			bool result = false;

			if (canExecute.Value)
			{
				OnRequestExecute?.Invoke();
				result = true;
			}

			return result;
		}

		private void OnCanExecuteChanges(bool value)
		{
			canExecute.Value = value;
		}
	}

	public class ObservableCommand<T> : IObservableCommand<T>
	{
		public event ObservableCommandDelegate<T> OnRequestExecute;

		public IReadOnlyObservableVariable<bool> CanExecute => canExecute;

		private readonly ObservableVariable<bool> canExecute;

		public ObservableCommand()
		{
			canExecute = new ObservableVariable<bool>(true);
		}

		public ObservableCommand(IObservableVariable<bool> canExecuteSource)
		{
			canExecute = new ObservableVariable<bool>(canExecuteSource.Value);
			canExecuteSource.OnChange += OnCanExecuteChanges;
		}

		public bool Execute(T parameter)
		{
			bool result = false;

			if (canExecute.Value)
			{
				OnRequestExecute?.Invoke(parameter);
				result = true;
			}

			return result;
		}

		private void OnCanExecuteChanges(bool value)
		{
			canExecute.Value = value;
		}
	}
}
