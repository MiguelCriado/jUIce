using System;
using UnityEngine;

namespace Juice
{
	public class CollectionItemViewModelComponent : ViewModelComponent, IViewModelInjector
	{
		public Type InjectionType => expectedViewModelType.Type;
		public override Type ExpectedType => expectedViewModelType.Type;
		public ViewModelComponent Target => this;

		[TypeConstraint(typeof(IBindableViewModel<>), true)]
		[SerializeField] protected SerializableType expectedViewModelType;

		private IBindableViewModel<object> bindableViewModel;

		public void SetData(object data)
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
