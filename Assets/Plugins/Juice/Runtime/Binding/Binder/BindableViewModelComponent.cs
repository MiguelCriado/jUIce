using System;
using Juice.Utils;
using UnityEngine;

namespace Juice
{
	public class BindableViewModelComponent : ViewModelComponent, IViewModelInjector
	{
		public Type InjectionType => viewModelType.Type;
		public override Type ExpectedType => viewModelType.Type;
		public ViewModelComponent Target => this;

		[TypeConstraint(typeof(IBindableViewModel<>), true)]
		[SerializeField] protected SerializableType viewModelType = new SerializableType();
		[SerializeField] private BindingInfo bindingInfo;

		private VariableBinding<object> binding;
		private IBindableViewModel<object> bindableViewModel;

		protected override void OnValidate()
		{
			base.OnValidate();

			if (viewModelType != null && viewModelType.Type != null)
			{
				var genericType = FindGenericType();
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
			binding.Property.Changed += OnValueChanged;
		}

		protected virtual void OnEnable()
		{
			binding.Bind();
		}

		protected virtual void OnDisable()
		{
			binding.Unbind();
			bindableViewModel?.SetData(null);
		}

		private Type FindGenericType()
		{
			Type result = null;

			using (var enumerator = viewModelType.Type.GetGenericInterfacesTowardsRoot().GetEnumerator())
			{
				while (result == null && enumerator.MoveNext())
				{
					Type[] args = enumerator.Current?.GetGenericArguments();

					if (args != null
					    && args.Length == 1
					    && typeof(IBindableViewModel<>).MakeGenericType(args).IsAssignableFrom(enumerator.Current))
					{
						result = enumerator.Current;
					}
				}
			}

			return result;
		}

		private void OnValueChanged(object data)
		{
			if (ExpectedType != null)
			{
				if (bindableViewModel == null)
				{
					object viewModel = Activator.CreateInstance(ExpectedType);
					bindableViewModel = (IBindableViewModel<object>)viewModel;
					ViewModel = bindableViewModel;
				}

				bindableViewModel.SetData(data);
			}
			else
			{
				Debug.LogError("Expected Type must be set", this);
			}
		}
	}
}
