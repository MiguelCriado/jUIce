using System;
using UnityEngine;

namespace Maui
{
	public class CommandConversionHandler<TFrom, TTo> : ConversionHandler<TFrom, TTo>
	{
		public override IViewModel ViewModel { get; }

		private readonly CommandBinding<TTo> commandBinding;

		public CommandConversionHandler(BindingInfo bindingInfo, Component context, Func<TFrom, TTo> conversionFunction)
			: base(bindingInfo, context, conversionFunction)
		{
			commandBinding = new CommandBinding<TTo>(bindingInfo, context);
			ObservableCommand<TFrom> convertedCommand = new ObservableCommand<TFrom>(commandBinding.Property.CanExecute);
			ViewModel = new OperatorBinderCommandViewModel<TFrom>(convertedCommand);
			convertedCommand.ExecuteRequested += ConvertedCommandExecuteRequestedHandler;
		}

		private void ConvertedCommandExecuteRequestedHandler(TFrom parameter)
		{
			commandBinding.Property.Execute(conversionFunction(parameter));
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