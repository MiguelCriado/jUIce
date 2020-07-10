using System;
using UnityEngine;

namespace Maui.Sandbox
{
	public class ViewModelInjector : MonoBehaviour, IViewModelInjector<TestViewModel>
	{
		public Type ViewModelType => typeof(TestViewModel);

		[SerializeField] private ViewModelComponent viewModelComponent;

		private void Awake()
		{
			viewModelComponent.Set(new TestViewModel());
		}
	}
}
