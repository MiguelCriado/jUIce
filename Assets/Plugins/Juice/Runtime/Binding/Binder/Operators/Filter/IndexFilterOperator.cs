using System;
using UnityEngine;

namespace Juice
{
	public abstract class IndexFilterOperator<T> : Operator
	{
		[SerializeField] private ConstantBindingInfo<int> index = new ConstantBindingInfo<int>();
		[SerializeField] private BindingInfo collection = BindingInfo.Collection<T>();
		private int BoundIndex => indexBinding.IsBound ? indexBinding.Property.GetValue(-1) : -1;

		private CollectionBinding<T> collectionBinding;
		private VariableBinding<int> indexBinding;
		private ObservableVariable<T> exposedVariable;

		protected override Type GetInjectionType()
		{
			return typeof(OperatorVariableViewModel<T>);
		}

		protected override void Awake()
		{
			base.Awake();

			exposedVariable = new ObservableVariable<T>();
			ViewModel = new OperatorVariableViewModel<T>(exposedVariable);

			indexBinding = RegisterVariable<int>(index)
				.OnChanged(OnIndexChanged)
				.OnCleared(OnIndexCleared)
				.GetBinding();

			collectionBinding = RegisterCollection<T>(collection)
				.OnItemAdded(OnItemAdded)
				.OnItemRemoved(OnItemRemoved)
				.OnItemReplaced(OnItemReplaced)
				.OnItemMoved(OnItemMoved)
				.OnReset(OnReset)
				.GetBinding();
		}

		private void OnIndexChanged(int value)
		{
			Evaluate();
		}
		
		private void OnIndexCleared()
		{
			exposedVariable.Clear();
		}
		
		private void Evaluate()
		{
			if (BoundIndex >= 0 && BoundIndex < collectionBinding.Property.Count)
			{
				RefreshExposedValue(collectionBinding.Property[BoundIndex]);
			}
			else
			{
				exposedVariable.Clear();
			}
		}

		private void RefreshExposedValue(T value)
		{
			exposedVariable.Value = value;
		}

		private void OnItemAdded(int index, T value)
		{
			if (index == BoundIndex)
			{
				RefreshExposedValue(value);
			}
		}

		private void OnItemRemoved(int index, T value)
		{
			if (index == BoundIndex)
			{
				exposedVariable.Clear();
			}
		}

		private void OnItemReplaced(int index, T lastValue, T newValue)
		{
			if (index == BoundIndex)
			{
				RefreshExposedValue(newValue);
			}
		}

		private void OnItemMoved(int lastIndex, int newIndex, T value)
		{
			Evaluate();
		}

		private void OnReset()
		{
			exposedVariable.Clear();
		}
	}
}