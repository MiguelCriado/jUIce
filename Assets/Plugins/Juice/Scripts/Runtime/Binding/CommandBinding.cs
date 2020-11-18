using System;
using Juice.Utils;
using UnityEngine;

namespace Juice
{
	public class CommandBinding : Binding
	{
		public override bool IsBound => boundProperty != null;
		public IObservableCommand Property => exposedProperty;
		
		private readonly ObservableVariable<bool> canExecuteSource;
		private readonly ObservableCommand exposedProperty;
		private IObservableCommand boundProperty;

		public CommandBinding(BindingInfo bindingInfo, Component context) : base(bindingInfo, context)
		{
			canExecuteSource = new ObservableVariable<bool>(false);
			exposedProperty = new ObservableCommand(canExecuteSource, OnExecuteRequested);
		}
		
		protected override Type GetBindingType()
		{
			return typeof(IObservableCommand);
		}

		protected override void BindProperty(object property)
		{
			boundProperty = property as IObservableCommand;

			if (boundProperty != null)
			{
				boundProperty.CanExecute.Changed += OnCanExecuteChanged;
				RaiseFirstNotification();
			}
			else
			{
				Debug.LogError($"Property type ({property.GetType()}) different from expected type {typeof(IObservableCommand)}", context);
			}
		}

		protected override void UnbindProperty()
		{
			if (boundProperty != null)
			{
				boundProperty.CanExecute.Changed -= OnCanExecuteChanged;
				boundProperty = null;
				canExecuteSource.Value = false;
			}
		}
		
		private void RaiseFirstNotification()
		{
			if (boundProperty.CanExecute.HasValue && boundProperty.CanExecute.Value == canExecuteSource.Value)
			{
				canExecuteSource.ForceChangedNotification();
			}
			else
			{
				canExecuteSource.Value = boundProperty.CanExecute.GetValue(false);
			}
		}

		private void OnExecuteRequested()
		{
			boundProperty?.Execute();
		}
		
		private void OnCanExecuteChanged(bool newValue)
		{
			canExecuteSource.Value = newValue;
		}
	}
	
	public class CommandBinding<T> : Binding
	{
		public override bool IsBound => boundProperty != null;
		public IObservableCommand<T> Property => exposedProperty;
		
		private readonly ObservableVariable<bool> canExecuteSource;
		private readonly ObservableCommand<T> exposedProperty;
		private IObservableCommand<T> boundProperty;

		public CommandBinding(BindingInfo bindingInfo, Component context) : base(bindingInfo, context)
		{
			canExecuteSource = new ObservableVariable<bool>(false);
			exposedProperty = new ObservableCommand<T>(canExecuteSource, OnExecuteRequested);
		}

		protected override Type GetBindingType()
		{
			return typeof(IObservableCommand<T>);
		}

		protected override void BindProperty(object property)
		{
			boundProperty = property as IObservableCommand<T>;

			if (boundProperty == null)
			{
				boundProperty = BoxCommand(property);
			}
			
			if (boundProperty != null)
			{
				boundProperty.CanExecute.Changed += OnCanExecuteChanged;
				RaiseFirstNotification();
			}
			else
			{
				Debug.LogError($"Property type ({property.GetType()}) cannot be bound as {typeof(IObservableCommand<T>)}", context);
			}
		}

		protected override void UnbindProperty()
		{
			if (boundProperty != null)
			{
				boundProperty.CanExecute.Changed -= OnCanExecuteChanged;
				boundProperty = null;
				canExecuteSource.Value = false;
			}
		}

		private static IObservableCommand<T> BoxCommand(object commandToBox)
		{
			IObservableCommand<T> result = null;
			
			Type commandGenericType = commandToBox.GetType().GetGenericTypeTowardsRoot();

			if (commandGenericType != null)
			{
				Type actualType = typeof(T);
				Type boxedType = commandGenericType.GenericTypeArguments[0];
				Type activationType = typeof(CommandBoxer<,>).MakeGenericType(actualType, boxedType);
				result = Activator.CreateInstance(activationType, commandToBox) as IObservableCommand<T>;
			}

			return result;
		}
		
		private void RaiseFirstNotification()
		{
			if (boundProperty.CanExecute.HasValue && boundProperty.CanExecute.Value == canExecuteSource.Value)
			{
				canExecuteSource.ForceChangedNotification();
			}
			else
			{
				canExecuteSource.Value = boundProperty.CanExecute.GetValue(false);
			}
		}
		
		private void OnExecuteRequested(T value)
		{
			boundProperty?.Execute(value);
		}
		
		private void OnCanExecuteChanged(bool newValue)
		{
			canExecuteSource.Value = newValue;
		}
	}
}