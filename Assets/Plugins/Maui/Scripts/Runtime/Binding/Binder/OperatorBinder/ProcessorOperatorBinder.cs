using System;
using Maui.Utils;
using UnityEngine;

namespace Maui
{
	public enum BindingType 
	{
		Variable,
		Collection,
		Command,
		Event
	}
	
	public abstract class ProcessorOperatorBinder<TFrom, TTo> : OperatorBinder
	{
		[SerializeField, DisableAtRuntime] private BindingType bindingType;
		[SerializeField] private BindingInfo fromBinding = new BindingInfo(typeof(IReadOnlyObservableVariable<TFrom>));

		private IBindingProcessor bindingProcessor;
		
		protected override void Reset()
		{
			base.Reset();
			
			bindingType = BindingType.Variable;
			fromBinding = new BindingInfo(typeof(IReadOnlyObservableVariable<TFrom>));
			expectedType = new SerializableType(typeof(OperatorBinderVariableViewModel<TTo>));
		}
		
		protected override void OnValidate()
		{
			base.OnValidate();

			SanitizeTypes();
		}

		protected virtual void Awake()
		{
			Initialize();
			ViewModel = bindingProcessor.ViewModel;
		}

		protected virtual void OnEnable()
		{
			bindingProcessor.Bind();
		}
		
		protected virtual void OnDisable()
		{
			bindingProcessor.Unbind();
		}

		protected abstract IBindingProcessor GetBindingProcessor(BindingType bindingType, BindingInfo fromBinding);
		
		protected override Type GetInjectionType()
		{
			switch (bindingType)
			{
				default:
				case BindingType.Variable : return typeof(OperatorBinderVariableViewModel<TTo>);
				case BindingType.Collection: return typeof(OperatorBinderCollectionViewModel<TTo>);
				case BindingType.Command: return typeof(OperatorBinderCommandViewModel<TFrom>);
				case BindingType.Event: return typeof(OperatorBinderEventViewModel<TTo>);
			}
		}
		
		private void SanitizeTypes()
		{
			bool didTypeChange = false;
			
			if (bindingType == BindingType.Variable && fromBinding.Type != typeof(IReadOnlyObservableVariable<TFrom>))
			{
				fromBinding = new BindingInfo(typeof(IReadOnlyObservableVariable<TFrom>));
				expectedType = new SerializableType(typeof(OperatorBinderVariableViewModel<TTo>));
				didTypeChange = true;
			}

			if (bindingType == BindingType.Collection && fromBinding.Type != typeof(IReadOnlyObservableCollection<TFrom>))
			{
				fromBinding = new BindingInfo(typeof(IReadOnlyObservableCollection<TFrom>));
				expectedType = new SerializableType(typeof(OperatorBinderCollectionViewModel<TTo>));
				didTypeChange = true;
			}

			if (bindingType == BindingType.Command && fromBinding.Type != typeof(IObservableCommand<TTo>))
			{
				fromBinding = new BindingInfo(typeof(IObservableCommand<TTo>));
				expectedType = new SerializableType(typeof(OperatorBinderCommandViewModel<TFrom>));
				didTypeChange = true;
			}
			
			if (bindingType == BindingType.Event && fromBinding.Type != typeof(IObservableEvent<TFrom>))
			{
				fromBinding = new BindingInfo(typeof(IObservableEvent<TFrom>));
				expectedType = new SerializableType(typeof(OperatorBinderEventViewModel<TTo>));
				didTypeChange = true;
			}

			if (didTypeChange)
			{
				BindingInfoTrackerProxy.RefreshBindingInfo();
			}
		}
		
		private void Initialize()
		{
			bindingProcessor = GetBindingProcessor(bindingType, fromBinding);
		}
	}
}