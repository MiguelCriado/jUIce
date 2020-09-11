using System;
using UnityEngine;

namespace Maui
{
	public class CommandBindingProcessor<TFrom, TTo> : BindingProcessor<TFrom, TTo>
	{
		public override IViewModel ViewModel { get; }

		private readonly CommandBinding<TTo> commandBinding;

		public CommandBindingProcessor(BindingInfo bindingInfo, Component context, Func<TFrom, TTo> processFunction)
			: base(bindingInfo, context, processFunction)
		{
			commandBinding = new CommandBinding<TTo>(bindingInfo, context);
			ObservableCommand<TFrom> convertedCommand = new ObservableCommand<TFrom>(commandBinding.Property.CanExecute);
			ViewModel = new OperatorBinderCommandViewModel<TFrom>(convertedCommand);
			convertedCommand.ExecuteRequested += ProcessedCommandExecuteRequestedHandler;
		}

		private void ProcessedCommandExecuteRequestedHandler(TFrom parameter)
		{
			commandBinding.Property.Execute(processFunction(parameter));
		}

		public override void Bind()
		{
			commandBinding.Bind();
		}

		public override void Unbind()
		{
			commandBinding.Unbind();
		}
	}
}