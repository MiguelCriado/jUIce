using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Juice
{
	public abstract class CollectionBatcherOperator<T> : Operator
	{
		private class ActionBatch
		{
			public bool IsDone => lastProcessedIndex >= actions.Count - 1;
			public DateTime InitialBuildingTime { get; }

			private readonly List<Action> actions;
			private readonly float timeBetweenActions;

			private DateTime initialExecutionTime;
			private int lastProcessedIndex;

			public ActionBatch(float timeBetweenActions)
			{
				InitialBuildingTime = DateTime.UtcNow;
				actions = new List<Action>();
				initialExecutionTime = DateTime.MinValue;
				lastProcessedIndex = -1;

				this.timeBetweenActions = timeBetweenActions;
			}

			public void Add(Action action)
			{
				actions.Add(action);
			}

			public void Update()
			{
				if (initialExecutionTime == DateTime.MinValue)
				{
					initialExecutionTime = DateTime.UtcNow;
				}

				if (!IsDone && GetIndexForCurrentTime() > lastProcessedIndex)
				{
					int currentIndex = lastProcessedIndex + 1;
					actions[currentIndex].Invoke();
					lastProcessedIndex++;
				}
			}

			private int GetIndexForCurrentTime()
			{
				return Mathf.FloorToInt((float)(DateTime.UtcNow - initialExecutionTime).TotalSeconds / timeBetweenActions);
			}
		}

		protected abstract BindingInfo Collection { get; }

		[SerializeField] private ConstantBindingInfo<float> batchThreshold = new ConstantBindingInfo<float>();
		[SerializeField] private ConstantBindingInfo<float> timeBetweenItems = new ConstantBindingInfo<float>();

		private float BatchThreshold => batchThresholdBinding.Property.GetValue(0);
		private float TimeBetweenItems => timeBetweenItemsBinding.Property.GetValue(0);

		private readonly List<ActionBatch> batches = new List<ActionBatch>();

		private VariableBinding<float> batchThresholdBinding;
		private VariableBinding<float> timeBetweenItemsBinding;
		private ObservableCollection<T> exposedCollection;


		protected override void Awake()
		{
			base.Awake();

			exposedCollection = new ObservableCollection<T>();
			ViewModel = new OperatorCollectionViewModel<T>(exposedCollection);

			batchThresholdBinding = RegisterVariable<float>(batchThreshold).GetBinding();
			timeBetweenItemsBinding = RegisterVariable<float>(timeBetweenItems).GetBinding();

			RegisterCollection<T>(Collection)
				.OnItemAdded(OnItemAdded)
				.OnItemRemoved(OnItemRemoved)
				.OnItemReplaced(OnItemReplaced)
				.OnItemMoved(OnItemMoved)
				.OnReset(OnCleared);
		}
		
		protected override void OnDisable()
		{
			base.OnDisable();

			batches.Clear();
			exposedCollection.Clear();
		}

		protected virtual void Update()
		{
			ActionBatch currentBatch = GetCurrentBatch();

			if (currentBatch != null)
			{
				currentBatch.Update();

				if (currentBatch.IsDone)
				{
					batches.Remove(currentBatch);
				}
			}
		}

		protected override Type GetInjectionType()
		{
			return typeof(OperatorCollectionViewModel<T>);
		}

		private void OnItemAdded(int index, T value)
		{
			ProcessAction(() => AddItem(index, value));
		}

		private void OnItemRemoved(int index, T value)
		{
			ProcessAction(() => RemoveItem(index));
		}

		private void OnItemReplaced(int index, T oldValue, T newValue)
		{
			ProcessAction(() => ReplaceItem(index, newValue));
		}

		private void OnItemMoved(int oldIndex, int newIndex, T value)
		{
			ProcessAction(() => MoveItem(oldIndex, newIndex));
		}

		private void OnCleared()
		{
			ProcessAction(Clear);
		}

		private void ProcessAction(Action action)
		{
			if (ShouldStartBatch())
			{
				ActionBatch newBatch = new ActionBatch(TimeBetweenItems);
				batches.Add(newBatch);
			}

			GetLastBatch().Add(action);
		}

		private ActionBatch GetCurrentBatch()
		{
			ActionBatch result = null;

			if (batches.Count > 0)
			{
				result = batches.First();
			}

			return result;
		}

		private ActionBatch GetLastBatch()
		{
			ActionBatch result = null;

			if (batches.Count > 0)
			{
				result = batches.Last();
			}

			return result;
		}

		private void AddItem(int index, T value)
		{
			exposedCollection.Insert(index, value);
		}

		private void RemoveItem(int index)
		{
			exposedCollection.RemoveAt(index);
		}

		private void ReplaceItem(int index, T value)
		{
			exposedCollection[index] = value;
		}

		private void MoveItem(int oldIndex, int newIndex)
		{
			exposedCollection.Move(oldIndex, newIndex);
		}

		private void Clear()
		{
			exposedCollection.Clear();
		}

		private bool ShouldStartBatch()
		{
			return batches.Count <= 0
				   || (DateTime.UtcNow - GetLastBatch().InitialBuildingTime).TotalSeconds >= BatchThreshold;
		}
	}
}
