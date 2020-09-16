using System;
using Maui.Utils;
using UnityEngine;

namespace Maui
{
	public class BindableViewModelComponent : ViewModelComponent, IViewModelInjector
	{
		public Type InjectionType => expectedViewModelType.Type;
		public override Type ExpectedType => expectedViewModelType.Type;
		public ViewModelComponent Target => this;
		
		[TypeConstraint(typeof(BindableViewModel<>), true)]
		[SerializeField] protected SerializableType expectedViewModelType = new SerializableType();
		[SerializeField] private BindingInfo bindingInfo;

		private VariableBinding<object> binding;

		protected override void OnValidate()
		{
			base.OnValidate();

			if (expectedViewModelType != null && expectedViewModelType.Type != null)
			{
				Type genericType = expectedViewModelType.Type.GetGenericTypeTowardsRoot();
				Type dataType = genericType.GenericTypeArguments[0];
				Type bindingType = typeof(IReadOnlyObservableVariable<>).MakeGenericType(dataType);

				if (bindingInfo.Type != bindingType)
				{
					bindingInfo = new BindingInfo(bindingType);
					BindingInfoTrackerProxy.RefreshBindingInfo();
				}
			}
		}

		protected virtual void Awake()
		{
			binding = new VariableBinding<object>(bindingInfo, this);
			binding.Property.Changed += SetData;
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
			if (ExpectedType != null)
			{
				object viewModel = Activator.CreateInstance(ExpectedType, data);
				ViewModel = (IViewModel)viewModel;
			}
			else
			{
				Debug.LogError("Expected Type must be set", this);
			}
		}
	}
}