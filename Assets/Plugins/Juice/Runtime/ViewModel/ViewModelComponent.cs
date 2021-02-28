using System;
using UnityEngine;

namespace Juice
{
	public class ViewModelComponent : MonoBehaviour
	{
		public delegate void ViewModelChangeEventHandler(ViewModelComponent source, IViewModel lastViewModel, IViewModel newViewModel);

		public event ViewModelChangeEventHandler ViewModelChanged;

		public virtual Type ExpectedType => expectedType.Type;

		public IViewModel ViewModel
		{
			get => viewModel;
			set
			{
				IViewModel lastViewModel = viewModel;
				viewModel?.Disable();
				viewModel = value;
				viewModel?.Enable();
				OnViewModelChanged(lastViewModel, viewModel);
			}
		}

		public string Id => id;

		[TypeConstraint(typeof(IViewModel))]
		[SerializeField, HideInInspector] protected SerializableType expectedType;
		[SerializeField, DisableAtRuntime] private string id;

		private IViewModel viewModel;

		protected virtual void Reset()
		{
			ResetId();
		}

		protected virtual void OnValidate()
		{
			if (string.IsNullOrEmpty(id) || IsIdAvailable(id) == false)
			{
				ResetId();
			}
		}

		protected virtual void OnViewModelChanged(IViewModel lastViewModel, IViewModel newViewModel)
		{
			ViewModelChanged?.Invoke(this, lastViewModel, newViewModel);
		}

		private void ResetId()
		{
			string candidate = GetNewId();

			while (IsIdAvailable(candidate) == false)
			{
				candidate = GetNewId();
			}

			id = candidate;
		}

		private string GetNewId()
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
