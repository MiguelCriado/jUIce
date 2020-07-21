using System;
using UnityEngine;

namespace Maui
{
	public class ViewModelComponent : MonoBehaviour
	{
		public event EventHandler<IViewModel> ViewModelChanged;

		public Type ExpectedType => expectedType.Type;
		public IViewModel ViewModel { get; private set; }

		[TypeConstraint(typeof(IViewModel))]
		[SerializeField] private SerializableType expectedType;

		public void Set(IViewModel viewModel)
		{
			ViewModel = viewModel;
			OnViewModelChanged();
		}

		protected virtual void OnViewModelChanged()
		{
			ViewModelChanged?.Invoke(this, ViewModel);
		}
	}
}
