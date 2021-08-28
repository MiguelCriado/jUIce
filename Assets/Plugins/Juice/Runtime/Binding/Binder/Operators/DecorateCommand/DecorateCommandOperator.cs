using System;
using UnityEngine;

namespace Juice
{
	public abstract class DecorateCommandOperator<T> : Operator
	{
		[SerializeField] private BindingInfo command = BindingInfo.Command<T>();
		[SerializeField] private ConstantBindingInfo<T> decoration = new ConstantBindingInfo<T>();
		
		private ObservableCommand exposedCommand;
		private CommandBinding<T> commandBinding;
		private VariableBinding<T> decorationBinding;

		protected override void Awake()
		{
			base.Awake();

			exposedCommand = new ObservableCommand();
			exposedCommand.ExecuteRequested += OnExposedCommandExecuteRequested;
			ViewModel = new CommandViewModel(exposedCommand);

			commandBinding = RegisterCommand<T>(command).OnCanExecuteChanged(OnCommandCanExecuteChanged).GetBinding();
			decorationBinding = RegisterVariable<T>(decoration).GetBinding();
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			OnCommandCanExecuteChanged(commandBinding.Property.CanExecute.Value);
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			OnCommandCanExecuteChanged(false);
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
