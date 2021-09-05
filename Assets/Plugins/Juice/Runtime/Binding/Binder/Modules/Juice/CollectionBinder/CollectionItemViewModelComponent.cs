using System;
using System.Reflection;
using UnityEngine;

namespace Juice
{
	public class CollectionItemViewModelComponent : ViewModelComponent, IViewModelInjector
	{
		private static readonly object[] DataArray = new object[1];

		public Type InjectionType => expectedViewModelType.Type;
		public override Type ExpectedType => expectedViewModelType.Type;
		public ViewModelComponent Target => this;

		[TypeConstraint(typeof(IBindableViewModel<>), true)]
		[SerializeField] protected SerializableType expectedViewModelType;
		
		private MethodInfo setMethod;

		public void SetData(object data)
		{
			if (ViewModel == null)
			{
				if (ExpectedType != null)
				{
					object viewModel = Activator.CreateInstance(ExpectedType);
					setMethod = ExpectedType.GetMethod(nameof(IBindableViewModel<object>.Set));
					SetData((IViewModel)viewModel, data);
					ViewModel = (IViewModel)viewModel;
				}
				else
				{
					Debug.LogError("Expected Type must be set", this);
				}
			}
			else
			{
				SetData(ViewModel, data);
			}
		}
		
		private void SetData(IViewModel viewModel, object data)
		{
			if (setMethod != null)
			{
				DataArray[0] = data;
				setMethod.Invoke(viewModel, DataArray);
			}
		}
	}
}
