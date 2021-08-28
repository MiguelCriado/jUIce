using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public abstract class CollectionBindingAsyncProcessor<TFrom, TTo> : IBindingProcessor
	{
		public IViewModel ViewModel { get; }

		protected readonly CollectionBinding<TFrom> collectionBinding;
		protected readonly ObservableCollection<TTo> processedCollection;

		private readonly Queue<Task> operations;
		private CancellationTokenSource cancellationTokenSource;

		protected CollectionBindingAsyncProcessor(BindingInfo bindingInfo, Component context)
		{
			processedCollection = new ObservableCollection<TTo>();
			ViewModel = new OperatorCollectionViewModel<TTo>(processedCollection);
			collectionBinding = new CollectionBinding<TFrom>(bindingInfo, context);
			collectionBinding.Property.Reset += OnReset;
			collectionBinding.Property.ItemAdded += OnItemAdded;
			collectionBinding.Property.ItemReplaced += OnItemReplaced;
			collectionBinding.Property.ItemRemoved += OnItemRemoved;
			collectionBinding.Property.ItemMoved += OnItemMoved;

			operations = new Queue<Task>();
		}

		public virtual void Bind()
		{
			cancellationTokenSource = new CancellationTokenSource();

			collectionBinding.Bind();
		}

		public virtual void Unbind()
		{
			cancellationTokenSource.Cancel();

			collectionBinding.Unbind();
		}

		protected abstract Task<TTo> ProcessValueAsync(TFrom newValue, TFrom oldValue, bool isNewItem);

		protected virtual async void OnReset()
		{
			await RunOperation(ResetCollection());
		}

		protected virtual async void OnItemAdded(int index, TFrom value)
		{
			await RunOperation(InsertValue(index, value));
		}

		protected virtual async void OnItemReplaced(int index, TFrom oldValue, TFrom newValue)
		{
			await RunOperation(ReplaceValue(index, oldValue, newValue));
		}

		protected virtual async void OnItemRemoved(int index, TFrom value)
		{
			await RunOperation(RemoveValue(index, value));
		}

		protected virtual async void OnItemMoved(int oldIndex, int newIndex, TFrom value)
		{
			await RunOperation(MoveValue(oldIndex, newIndex, value));
		}

		private Task ResetCollection()
		{
			processedCollection.Clear();

			return Task.CompletedTask;
		}

		private async Task InsertValue(int index, TFrom value)
		{
			TTo result = await ProcessValueAsync(value, default, true);

			processedCollection.Insert(index, result);
		}

		private async Task ReplaceValue(int index, TFrom oldValue, TFrom newValue)
		{
			processedCollection[index] = await ProcessValueAsync(newValue, oldValue, false);
		}

		private Task RemoveValue(int index, TFrom value)
		{
			processedCollection.RemoveAt(index);

			return Task.CompletedTask;
		}

		private Task MoveValue(int oldIndex, int newIndex, TFrom value)
		{
			processedCollection.Move(oldIndex, newIndex);

			return Task.CompletedTask;
		}

		private async Task RunOperation(Task op)
		{
			operations.Enqueue(op);

			await RunOperations();
		}

		private async Task RunOperations()
		{
			while (!cancellationTokenSource.IsCancellationRequested && operations.Count > 0)
			{
				await operations.Dequeue();
			}
		}
	}
}