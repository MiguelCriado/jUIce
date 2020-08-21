using System;
using UnityEngine;

namespace Maui
{
	public class CollectionConversionHandler<TFrom, TTo> : ConversionHandler<TFrom, TTo>
	{
		public override IViewModel ViewModel { get; }

		private readonly CollectionBinding<TFrom> collectionBinding;
		private readonly ObservableCollection<TTo> convertedCollection;

		public CollectionConversionHandler(BindingInfo bindingInfo, Component context, Func<TFrom, TTo> conversionFunction)
			: base(bindingInfo, context, conversionFunction)
		{
			convertedCollection = new ObservableCollection<TTo>();
			ViewModel = new CollectionValueConverterViewModel<TTo>(convertedCollection);
			collectionBinding = new CollectionBinding<TFrom>(bindingInfo, context);
			collectionBinding.Property.Reset += BoundCollectionResetHandler;
			collectionBinding.Property.ItemAdded += BoundCollectionItemAddedHandler;
			collectionBinding.Property.ItemReplaced += BoundCollectionItemReplacedHandler;
			collectionBinding.Property.ItemRemoved += BoundCollectionItemRemovedHandler;
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
			convertedCollection.Clear();
		}
		
		private void BoundCollectionItemAddedHandler(int index, TFrom value)
		{
			convertedCollection.Insert(index, conversionFunction(value));
		}
		
		private void BoundCollectionItemReplacedHandler(int index, TFrom oldValue, TFrom newValue)
		{
			convertedCollection[index] = conversionFunction(newValue);
		}
		
		private void BoundCollectionItemRemovedHandler(int index, TFrom value)
		{
			convertedCollection.RemoveAt(index);
		}
	}
}