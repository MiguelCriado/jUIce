using UnityEngine;

namespace Maui
{
	public abstract class CommandBinder : MonoBehaviour
	{
		[SerializeField] private BindingInfo bindingInfo = new BindingInfo(typeof(object));

		protected bool CanExecute => binding.Property.CanExecute.Value;

		private CommandBinding binding;

		protected virtual void Awake()
		{
			binding = new CommandBinding(bindingInfo, this);
			binding.Property.CanExecute.Changed += OnCommandCanExecuteChanged;
		}
		
		protected virtual void OnEnable()
		{
			binding.Bind();
		}

		protected virtual void OnDisable()
		{
			binding.Unbind();
		}

		protected void ExecuteCommand()
		{
			binding.Property.Execute();
		}
		
		protected abstract void OnCommandCanExecuteChanged(bool newValue);
	}
	
	public abstract class CommandBinder<T> : MonoBehaviour, IBinder<T>
	{
		[SerializeField] private BindingInfo bindingInfo = new BindingInfo(typeof(IObservableCommand<T>));

		protected bool CanExecute => binding.Property.CanExecute.Value;

		private CommandBinding<T> binding;

		protected virtual void Awake()
		{
			binding = new CommandBinding<T>(bindingInfo, this);
			binding.Property.CanExecute.Changed += OnCommandCanExecuteChanged;
		}
		
		protected void OnEnable()
		{
			binding.Bind();
		}

		protected void OnDisable()
		{
			binding.Unbind();
		}

		protected void ExecuteCommand(T value)
		{
			binding.Property.Execute(value);
		}
		
		protected abstract void OnCommandCanExecuteChanged(bool newValue);
	}
}