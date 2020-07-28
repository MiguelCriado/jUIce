using System;
using UnityEngine;

namespace Maui
{
	public class ViewModelComponent : MonoBehaviour
	{
		public event EventHandler<IViewModel> ViewModelChanged;

		public Type ExpectedType => expectedType.Type;

		public IViewModel ViewModel
		{
			get => viewModel;
			set
			{
				viewModel = value;
				OnViewModelChanged();
			}
		}

		[TypeConstraint(typeof(IViewModel))]
		[SerializeField] private SerializableType expectedType;

		private IViewModel viewModel;

		protected virtual void OnViewModelChanged()
		{
			ViewModelChanged?.Invoke(this, ViewModel);
		}
	}
}
