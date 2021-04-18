using System;
using Juice.Utils;
using UnityEngine;

namespace Juice
{
	public enum BindingType
	{
		Variable,
		Collection,
		Command,
		Event
	}

	public abstract class ProcessorOperator<TFrom, TTo> : Operator
	{
		protected virtual BindingType[] AllowedTypes => null; // All types allowed

		[AllowedBindingTypes(nameof(AllowedTypes))]
		[SerializeField, DisableAtRuntime] private BindingType bindingType;
		[SerializeField] private BindingInfo fromBinding = new BindingInfo(typeof(IReadOnlyObservableVariable<TFrom>));

		private IBindingProcessor bindingProcessor;

		protected override void Reset()
		{
			base.Reset();

			bindingType = BindingType.Variable;
			fromBinding = new BindingInfo(typeof(IReadOnlyObservableVariable<TFrom>));
			expectedType = new SerializableType(typeof(OperatorVariableViewModel<TTo>));
		}

		protected override void OnValidate()
		{
			base.OnValidate();

			SanitizeTypes();
		}

		protected override void Awake()
		{
			base.Awake();

			Initialize();
			ViewModel = bindingProcessor.ViewModel;
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			bindingProcessor.Bind();
		}

		protected new virtual void OnDisable()
		{
			base.OnDisable();

			bindingProcessor.Unbind();
		}

		protected abstract IBindingProcessor GetBindingProcessor(BindingType bindingType, BindingInfo fromBinding);

		protected override Type GetInjectionType()
		{
			switch (bindingType)
			{
				default:
				case BindingType.Variable : return typeof(OperatorVariableViewModel<TTo>);
				case BindingType.Collection: return typeof(OperatorCollectionViewModel<TTo>);
				case BindingType.Command: return typeof(OperatorCommandViewModel<TFrom>);
				case BindingType.Event: return typeof(OperatorEventViewModel<TTo>);
			}
		}

		private void SanitizeTypes()
		{
			bool didTypeChange = false;

			if (bindingType == BindingType.Variable && fromBinding.Type != typeof(IReadOnlyObservableVariable<TFrom>))
			{
				fromBinding = new BindingInfo(typeof(IReadOnlyObservableVariable<TFrom>));
				expectedType = new SerializableType(typeof(OperatorVariableViewModel<TTo>));
				didTypeChange = true;
			}

			if (bindingType == BindingType.Collection && fromBinding.Type != typeof(IReadOnlyObservableCollection<TFrom>))
			{
				fromBinding = new BindingInfo(typeof(IReadOnlyObservableCollection<TFrom>));
				expectedType = new SerializableType(typeof(OperatorCollectionViewModel<TTo>));
				didTypeChange = true;
			}

			if (bindingType == BindingType.Command && fromBinding.Type != typeof(IObservableCommand<TTo>))
			{
				fromBinding = new BindingInfo(typeof(IObservableCommand<TTo>));
				expectedType = new SerializableType(typeof(OperatorCommandViewModel<TFrom>));
				didTypeChange = true;
			}

			if (bindingType == BindingType.Event && fromBinding.Type != typeof(IObservableEvent<TFrom>))
			{
				fromBinding = new BindingInfo(typeof(IObservableEvent<TFrom>));
				expectedType = new SerializableType(typeof(OperatorEventViewModel<TTo>));
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
