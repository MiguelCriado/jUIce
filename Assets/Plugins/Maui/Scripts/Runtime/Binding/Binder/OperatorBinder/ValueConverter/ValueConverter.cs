using System;
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
	
	public abstract class ValueConverter<TFrom, TTo> : ViewModelComponent, IViewModelInjector
	{
		public Type InjectionType => GetInjectionType();
		public ViewModelComponent Target => this;

		[SerializeField, DisableAtRuntime] private BindingType bindingType;
		[SerializeField] private BindingInfo fromBinding = new BindingInfo(typeof(IReadOnlyObservableVariable<TFrom>));

		private ConversionHandler<TFrom, TTo> conversionHandler;

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
			ViewModel = conversionHandler.ViewModel;
		}

		protected virtual void OnEnable()
		{
			conversionHandler.Bind();
		}

		protected virtual void OnDisable()
		{
			conversionHandler.Unbind();
		}

		protected abstract TTo Convert(TFrom value);
		
		private Type GetInjectionType()
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
		
		private void Initialize()
		{
			switch (bindingType)
			{
				case BindingType.Variable: 
					conversionHandler = new VariableConversionHandler<TFrom, TTo>(fromBinding, this, Convert);
					break;
				case BindingType.Collection:
					conversionHandler = new CollectionConversionHandler<TFrom, TTo>(fromBinding, this, Convert);
					break;
				case BindingType.Command:
					conversionHandler = new CommandConversionHandler<TFrom, TTo>(fromBinding, this, Convert);
					break;
				case BindingType.Event:
					conversionHandler = new EventConversionHandler<TFrom, TTo>(fromBinding, this, Convert);
					break;
			}
		}
		
		private void SanitizeTypes()
		{
			if (bindingType == BindingType.Variable && fromBinding.Type != typeof(IReadOnlyObservableVariable<TFrom>))
			{
				fromBinding = new BindingInfo(typeof(IReadOnlyObservableVariable<TFrom>));
				expectedType = new SerializableType(typeof(OperatorBinderVariableViewModel<TTo>));
			}

			if (bindingType == BindingType.Collection && fromBinding.Type != typeof(IReadOnlyObservableCollection<TFrom>))
			{
				fromBinding = new BindingInfo(typeof(IReadOnlyObservableCollection<TFrom>));
				expectedType = new SerializableType(typeof(OperatorBinderCollectionViewModel<TTo>));
			}

			if (bindingType == BindingType.Command && fromBinding.Type != typeof(IObservableCommand<TTo>))
			{
				fromBinding = new BindingInfo(typeof(IObservableCommand<TTo>));
				expectedType = new SerializableType(typeof(OperatorBinderCommandViewModel<TFrom>));
			}
			
			if (bindingType == BindingType.Event && fromBinding.Type != typeof(IObservableEvent<TFrom>))
			{
				fromBinding = new BindingInfo(typeof(IObservableEvent<TFrom>));
				expectedType = new SerializableType(typeof(OperatorBinderEventViewModel<TTo>));
			}
		}
	}
}