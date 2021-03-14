using UnityEngine;

namespace Juice
{
	public abstract class CommandBinder : MonoBehaviour
	{
		[SerializeField, Rename(nameof(BindingInfoName))]
		private BindingInfo bindingInfo = new BindingInfo(typeof(IObservableCommand));

		protected virtual string BindingInfoName { get; } = nameof(bindingInfo);
		protected bool CanExecute => binding.Property.CanExecute.Value;

		private CommandBinding binding;

		protected virtual void Reset()
		{
			bindingInfo = new BindingInfo(typeof(IObservableCommand));
		}

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
		[SerializeField, Rename(nameof(BindingInfoName))]
		private BindingInfo bindingInfo = new BindingInfo(typeof(IObservableCommand<T>));

		protected virtual string BindingInfoName { get; } = nameof(bindingInfo);
		protected bool CanExecute => binding.Property.CanExecute.Value;

		private CommandBinding<T> binding;

		protected virtual void Reset()
		{
			bindingInfo = new BindingInfo(typeof(IObservableCommand<T>));
		}

		protected virtual void Awake()
		{
			binding = new CommandBinding<T>(bindingInfo, this);
			binding.Property.CanExecute.Changed += OnCommandCanExecuteChanged;
		}

		protected virtual void OnEnable()
		{
			binding.Bind();

			OnCommandCanExecuteChanged(binding.Property.CanExecute.Value);
		}

		protected virtual void OnDisable()
		{
			OnCommandCanExecuteChanged(false);

			binding.Unbind();
		}

		protected void ExecuteCommand(T value)
		{
			binding.Property.Execute(value);
		}

		protected abstract void OnCommandCanExecuteChanged(bool newValue);
	}
}
