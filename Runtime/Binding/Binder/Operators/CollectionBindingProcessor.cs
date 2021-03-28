using UnityEngine;

namespace Juice
{
	public abstract class CollectionBindingProcessor<TFrom, TTo> : IBindingProcessor
	{
		public IViewModel ViewModel { get; }

		protected readonly CollectionBinding<TFrom> collectionBinding;
		protected readonly ObservableCollection<TTo> processedCollection;

		protected CollectionBindingProcessor(BindingInfo bindingInfo, Component context)
		{
			processedCollection = new ObservableCollection<TTo>();
			ViewModel = new OperatorCollectionViewModel<TTo>(processedCollection);
			collectionBinding = new CollectionBinding<TFrom>(bindingInfo, context);
			collectionBinding.Property.Reset += BoundCollectionResetHandler;
			collectionBinding.Property.ItemAdded += BoundCollectionItemAddedHandler;
			collectionBinding.Property.ItemReplaced += BoundCollectionItemReplacedHandler;
			collectionBinding.Property.ItemRemoved += BoundCollectionItemRemovedHandler;
			collectionBinding.Property.ItemMoved += BoundCollectionItemMovedHandler;
		}

		public virtual void Bind()
		{
			collectionBinding.Bind();
		}

		public virtual void Unbind()
		{
			collectionBinding.Unbind();
		}

		protected abstract TTo ProcessValue(TFrom newValue, TFrom oldValue, bool isNewItem);
		
		protected virtual void BoundCollectionResetHandler()
		{
			processedCollection.Clear();
		}
		
		protected virtual void BoundCollectionItemAddedHandler(int index, TFrom value)
		{
			processedCollection.Insert(index, ProcessValue(value, default, true));
		}
		
		protected virtual void BoundCollectionItemReplacedHandler(int index, TFrom oldValue, TFrom newValue)
		{
			processedCollection[index] = ProcessValue(newValue, oldValue, false);
		}
		
		protected virtual void BoundCollectionItemRemovedHandler(int index, TFrom value)
		{
			processedCollection.RemoveAt(index);
		}
		
		protected virtual void BoundCollectionItemMovedHandler(int oldIndex, int newIndex, TFrom value)
		{
			processedCollection.Move(oldIndex, newIndex);
		}
	}
}