using UnityEngine;

namespace Maui
{
	public abstract class CommandBindingProcessor<TFrom, TTo> : IBindingProcessor
	{
		public IViewModel ViewModel { get; }

		protected readonly CommandBinding<TTo> commandBinding;

		protected CommandBindingProcessor(BindingInfo bindingInfo, Component context)
		{
			commandBinding = new CommandBinding<TTo>(bindingInfo, context);
			ObservableCommand<TFrom> convertedCommand = new ObservableCommand<TFrom>(commandBinding.Property.CanExecute);
			ViewModel = new OperatorBinderCommandViewModel<TFrom>(convertedCommand);
			convertedCommand.ExecuteRequested += ProcessedCommandExecuteRequestedHandler;
		}

		public virtual void Bind()
		{
			commandBinding.Bind();
		}

		public virtual void Unbind()
		{
			commandBinding.Unbind();
		}

		protected abstract TTo ProcessValue(TFrom value);
		
		protected virtual void ProcessedCommandExecuteRequestedHandler(TFrom parameter)
		{
			commandBinding.Property.Execute(ProcessValue(parameter));
		}
	}
}