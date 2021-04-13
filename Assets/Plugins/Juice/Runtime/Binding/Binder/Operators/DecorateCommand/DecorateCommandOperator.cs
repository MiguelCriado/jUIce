using System;
using UnityEngine;

namespace Juice
{
	public abstract class DecorateCommandOperator<T> : Operator
	{
		protected abstract ConstantBindingInfo<T> DecorationBindingInfo { get; }

		[SerializeField] private BindingInfo commandBindingInfo = new BindingInfo(typeof(IObservableCommand<T>));

		private ObservableCommand exposedCommand;
		private CommandBinding<T> commandBinding;
		private VariableBinding<T> decorationBinding;
		
		protected virtual void Awake()
		{
			exposedCommand = new ObservableCommand();
			exposedCommand.ExecuteRequested += OnExposedCommandExecuteRequested;
			ViewModel = new CommandViewModel(exposedCommand);
			
			commandBinding = new CommandBinding<T>(commandBindingInfo, this);
			commandBinding.Property.CanExecute.Changed += OnCommandCanExecuteChanged;
			
			decorationBinding = new VariableBinding<T>(DecorationBindingInfo, this);
		}

		protected virtual void OnEnable()
		{
			commandBinding.Bind();
			decorationBinding.Bind();
			
			OnCommandCanExecuteChanged(commandBinding.Property.CanExecute.Value);
		}

		protected virtual void OnDisable()
		{
			OnCommandCanExecuteChanged(false);
			
			commandBinding.Unbind();
			decorationBinding.Unbind();
		}

		protected override Type GetInjectionType()
		{
			return typeof(CommandViewModel);
		}
		
		private void OnExposedCommandExecuteRequested()
		{
			commandBinding.Property.Execute(decorationBinding.Property.GetValue(default));
		}
		
		private void OnCommandCanExecuteChanged(bool newValue)
		{
			exposedCommand.CanExecute.Value = newValue;
		}
	}
}