using System;
using UnityEngine;

namespace Maui
{
	public class ViewModelComponent : MonoBehaviour
	{
		public event EventHandler<IViewModel> ViewModelChanged;

		public Type ExpectedType => expectedType.Type;
		public IViewModel ViewModel => viewModel;

		[TypeConstraint(typeof(IViewModel))]
		[SerializeField] private SerializableType expectedType;
		[SerializeField] private IViewModel viewModel;

		public void Set(IViewModel viewModel)
		{
			this.viewModel = viewModel;
			OnViewModelChanged();
		}

		protected virtual void OnViewModelChanged()
		{
			ViewModelChanged?.Invoke(this, ViewModel);
		}
	}
}
