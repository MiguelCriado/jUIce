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

		public string Id => id;

		[TypeConstraint(typeof(IViewModel))]
		[SerializeField] [HideInInspector] protected SerializableType expectedType;
		[SerializeField] [HideInInspector] private string id;

		private IViewModel viewModel;

		protected virtual void Reset()
		{
			ResetId();
		}

		protected void OnValidate()
		{
			if (string.IsNullOrEmpty(id) || IsIdAvailable(id) == false)
			{
				ResetId();
			}
		}

		protected virtual void OnViewModelChanged()
		{
			ViewModelChanged?.Invoke(this, ViewModel);
		}

		private void ResetId()
		{
			string candidate = NewId();

			while (IsIdAvailable(candidate) == false)
			{
				candidate = NewId();
			}

			id = candidate;
		}

		private string NewId()
		{
			return Guid.NewGuid().ToString().Substring(0, 4);
		}

		private bool IsIdAvailable(string candidate)
		{
			ViewModelComponent[] components = GetComponentsInChildren<ViewModelComponent>(true);
			bool result = IsIdAvailable(candidate, components);
			components = GetComponentsInParent<ViewModelComponent>();
			result &= IsIdAvailable(candidate, components);
			return result;
		}

		private bool IsIdAvailable(string candidate, ViewModelComponent[] components)
		{
			bool result = true;
			int i = 0;

			while (result == true && i < components.Length)
			{
				ViewModelComponent current = components[i];

				if (current != this)
				{
					result &= string.Equals(candidate, current.Id) == false;

					if (result == false)
					{
						Debug.LogError($"Id \"{candidate}\" already taken by {current.name}", current);
					}
				}

				i++;
			}

			return result;
		}
	}
}
