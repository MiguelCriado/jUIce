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
