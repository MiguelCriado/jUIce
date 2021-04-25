using System;
using UnityEngine;

namespace Juice
{
	public abstract class CommandBinder : ComponentBinder
	{
		private event Action ExecuteRequested;

		[SerializeField, Rename(nameof(BindingInfoName))]
		private BindingInfo bindingInfo = new BindingInfo(typeof(IObservableCommand));

		protected virtual string BindingInfoName { get; } = nameof(bindingInfo);

		protected override void Awake()
		{
			base.Awake();

			RegisterCommand(bindingInfo)
				.AddExecuteTrigger(handler => ExecuteRequested += handler)
				.OnCanExecuteChanged(OnCommandCanExecuteChanged);
		}

		protected void ExecuteCommand()
		{
			ExecuteRequested?.Invoke();
		}

		protected abstract void OnCommandCanExecuteChanged(bool newValue);
	}

	public abstract class CommandBinder<T> : ComponentBinder
	{
		private event Action<T> ExecuteRequested;

		[SerializeField, Rename(nameof(BindingInfoName))]
		private BindingInfo bindingInfo = new BindingInfo(typeof(IObservableCommand<T>));

		protected virtual string BindingInfoName { get; } = nameof(bindingInfo);

		protected virtual void Reset()
		{
			bindingInfo = new BindingInfo(typeof(IObservableCommand<T>));
		}

		protected override void Awake()
		{
			base.Awake();

			RegisterCommand<T>(bindingInfo)
				.AddExecuteTrigger(handler => ExecuteRequested += handler)
				.OnCanExecuteChanged(OnCommandCanExecuteChanged);
		}

		protected void ExecuteCommand(T value)
		{
			ExecuteRequested?.Invoke(value);
		}

		protected abstract void OnCommandCanExecuteChanged(bool newValue);
	}
}
