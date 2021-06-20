using System;
using System.Reflection;
using Juice.Utils;
using UnityEngine;

namespace Juice
{
	public class BindableViewModelComponent : ViewModelComponent, IViewModelInjector
	{
		private static readonly object[] DataArray = new object[1];

		public Type InjectionType => expectedViewModelType.Type;
		public override Type ExpectedType => expectedViewModelType.Type;
		public ViewModelComponent Target => this;

		[TypeConstraint(typeof(IBindableViewModel<>), true)]
		[SerializeField] protected SerializableType expectedViewModelType = new SerializableType();
		[SerializeField] private BindingInfo bindingInfo;

		private VariableBinding<object> binding;
		private MethodInfo setMethod;

		protected override void OnValidate()
		{
			base.OnValidate();

			if (Application.isPlaying == false
			    && expectedViewModelType != null
			    && expectedViewModelType.Type != null)
			{
				Type genericType = FindBindableInterface(ExpectedType);
				Type dataType = genericType.GenericTypeArguments[0];
				Type bindingType = typeof(IReadOnlyObservableVariable<>).MakeGenericType(dataType);

				if (bindingInfo.Type != bindingType)
				{
					bindingInfo = new BindingInfo(bindingType);
					BindingInfoTrackerProxy.RefreshBindingInfo();
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();

			if (ExpectedType != null)
			{
				object viewModel = Activator.CreateInstance(ExpectedType);
				setMethod = ExpectedType.GetMethod(nameof(IBindableViewModel<object>.Set));
				ViewModel = (IViewModel)viewModel;

				binding = new VariableBinding<object>(bindingInfo, this);
				binding.Property.Changed += SetData;
			}
			else
			{
				Debug.LogError("Expected Type must be set", this);
			}
		}

		protected virtual void OnEnable()
		{
			binding.Bind();
		}

		protected virtual void OnDisable()
		{
			binding.Unbind();
		}

		public void SetData(object data)
		{
			if (ViewModel != null)
			{
				DataArray[0] = data;
				setMethod.Invoke(ViewModel, DataArray);
			}
		}

		private static Type FindBindableInterface(Type type)
		{
			Type result = null;

			if (type != null)
			{
				TypeFilter bindableFilter = BindableFilter;
				object filterCriteria = typeof(IBindableViewModel<>);
				Type[] interfaces = type.FindInterfaces(bindableFilter, filterCriteria);

				if (interfaces.Length > 0)
				{
					result = interfaces[0];
				}
				else
				{
					result = FindBindableInterface(type.BaseType);
				}
			}

			return result;
		}

		private static bool BindableFilter(Type typeObject, object criteria)
		{
			return typeObject.IsGenericType && typeObject.GetGenericTypeDefinition() == (Type)criteria;
		}
	}
}
