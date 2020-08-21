using System;
using Maui.Utils;
using UnityEngine;

namespace Maui
{
	public class CommandBinding : Binding
	{
		public override bool IsBound => boundProperty != null;
		public IObservableCommand Property => exposedProperty;
		
		private readonly IObservableVariable<bool> canExecuteSource;
		private readonly ObservableCommand exposedProperty;
		private IObservableCommand boundProperty;

		public CommandBinding(BindingInfo bindingInfo, Component context) : base(bindingInfo, context)
		{
			canExecuteSource = new ObservableVariable<bool>(false);
			exposedProperty = new ObservableCommand(canExecuteSource, RequestExecuteHandler);
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
				boundProperty.CanExecute.Changed += CanExecuteChangedHandler;
				canExecuteSource.Value = boundProperty.CanExecute.Value;
			}
			else
			{
				Debug.LogError($"Property type ({property.GetType()}) different from expected type \"{typeof(IObservableCommand)}");
			}
		}

		protected override void UnbindProperty()
		{
			if (boundProperty != null)
			{
				boundProperty.CanExecute.Changed -= CanExecuteChangedHandler;
				canExecuteSource.Value = false;
			}
		}

		private void RequestExecuteHandler()
		{
			boundProperty?.Execute();
		}
		
		private void CanExecuteChangedHandler(bool newValue)
		{
			canExecuteSource.Value = newValue;
		}
	}
	
	public class CommandBinding<T> : Binding
	{
		public override bool IsBound => boundProperty != null;
		public IObservableCommand<T> Property => exposedProperty;
		
		private readonly IObservableVariable<bool> canExecuteSource;
		private readonly ObservableCommand<T> exposedProperty;
		private IObservableCommand<T> boundProperty;

		public CommandBinding(BindingInfo bindingInfo, Component context) : base(bindingInfo, context)
		{
			canExecuteSource = new ObservableVariable<bool>(false);
			exposedProperty = new ObservableCommand<T>(canExecuteSource, RequestExecuteHandler);
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
				boundProperty.CanExecute.Changed += CanExecuteChangedHandler;
				canExecuteSource.Value = boundProperty.CanExecute.Value;
			}
			else
			{
				Debug.LogError($"Property type ({property.GetType()}) cannot be bound as {typeof(IObservableCommand<T>)}");
			}
		}

		protected override void UnbindProperty()
		{
			if (boundProperty != null)
			{
				boundProperty.CanExecute.Changed -= CanExecuteChangedHandler;
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
		
		private void RequestExecuteHandler(T value)
		{
			boundProperty?.Execute(value);
		}
		
		private void CanExecuteChangedHandler(bool newValue)
		{
			canExecuteSource.Value = newValue;
		}
	}
}