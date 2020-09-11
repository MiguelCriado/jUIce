using System;
using UnityEngine;

namespace Maui
{
	public class CollectionBindingProcessor<TFrom, TTo> : BindingProcessor<TFrom, TTo>
	{
		public override IViewModel ViewModel { get; }

		private readonly CollectionBinding<TFrom> collectionBinding;
		private readonly ObservableCollection<TTo> processedCollection;

		public CollectionBindingProcessor(BindingInfo bindingInfo, Component context, Func<TFrom, TTo> processFunction)
			: base(bindingInfo, context, processFunction)
		{
			processedCollection = new ObservableCollection<TTo>();
			ViewModel = new OperatorBinderCollectionViewModel<TTo>(processedCollection);
			collectionBinding = new CollectionBinding<TFrom>(bindingInfo, context);
			collectionBinding.Property.Reset += BoundCollectionResetHandler;
			collectionBinding.Property.ItemAdded += BoundCollectionItemAddedHandler;
			collectionBinding.Property.ItemReplaced += BoundCollectionItemReplacedHandler;
			collectionBinding.Property.ItemRemoved += BoundCollectionItemRemovedHandler;
			collectionBinding.Property.ItemMoved += BoundCollectionItemMovedHandler;
		}

		public override void Bind()
		{
			collectionBinding.Bind();
		}

		public override void Unbind()
		{
			collectionBinding.Unbind();
		}
		
		private void BoundCollectionResetHandler()
		{
			processedCollection.Clear();
		}
		
		private void BoundCollectionItemAddedHandler(int index, TFrom value)
		{
			processedCollection.Insert(index, processFunction(value));
		}
		
		private void BoundCollectionItemReplacedHandler(int index, TFrom oldValue, TFrom newValue)
		{
			processedCollection[index] = processFunction(newValue);
		}
		
		private void BoundCollectionItemRemovedHandler(int index, TFrom value)
		{
			processedCollection.RemoveAt(index);
		}
		
		private void BoundCollectionItemMovedHandler(int oldIndex, int newIndex, TFrom value)
		{
			processedCollection.Move(oldIndex, newIndex);
		}
	}
}