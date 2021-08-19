using System;
using System.Linq;
using UnityEngine;

namespace Juice
{
	public abstract class SubCollectionOperator<T> : Operator
	{
		protected abstract BindingInfo Collection { get; }

		[SerializeField] private ConstantBindingInfo<int> startAt = new ConstantBindingInfo<int>();
		[SerializeField] private ConstantBindingInfo<int> count = new ConstantBindingInfo<int>();

		private CollectionBinding<T> collectionBinding;
		private VariableBinding<int> startAtBinding;
		private VariableBinding<int> countBinding;
		private ObservableCollection<T> exposedVariable;

		protected override Type GetInjectionType()
		{
			return typeof(OperatorCollectionViewModel<T>);
		}

		protected override void Awake()
		{
			base.Awake();

			exposedVariable = new ObservableCollection<T>();
			ViewModel = new OperatorCollectionViewModel<T>(exposedVariable);

			collectionBinding = RegisterCollection<T>(Collection)
				.OnChanged(OnCollectionChanged)
				.GetBinding();

			startAtBinding = RegisterVariable<int>(startAt)
				.OnChanged(OnStartAtChanged)
				.GetBinding();

			countBinding = RegisterVariable<int>(count)
				.OnChanged(OnCountChanged)
				.GetBinding();
		}

		private void OnCollectionChanged()
		{
			Refresh();
		}

		private void OnStartAtChanged(int value)
		{
			Refresh();
		}

		private void OnCountChanged(int value)
		{
			Refresh();
		}

		private void Refresh()
		{
			if (collectionBinding.IsBound)
			{
				exposedVariable.Set(collectionBinding.Property
					.Skip(startAtBinding.Property.Value)
					.Take(countBinding.Property.Value));
			}
		}
	}
}
