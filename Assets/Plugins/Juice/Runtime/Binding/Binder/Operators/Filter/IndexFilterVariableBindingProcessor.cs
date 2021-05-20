using System;
using UnityEngine;

namespace Juice
{
	public class IndexFilterVariableBindingProcessor<T> : ToVariableBindingProcessor<int, T>
	{
		protected readonly CollectionBinding<T> collectionBinding;

		public IndexFilterVariableBindingProcessor(
			BindingInfo bindingInfo,
			Component context,
			Func<int, T> processFunction,
			CollectionBinding<T> collectionBinding)
			: base(bindingInfo, context, processFunction)
		{
			this.collectionBinding = collectionBinding;
			collectionBinding.Property.Reset += OnCollectionReset;
			collectionBinding.Property.ItemAdded += OnItemAdded;
			collectionBinding.Property.ItemMoved += OnItemMoved;
			collectionBinding.Property.ItemRemoved += OnItemRemoved;
			collectionBinding.Property.ItemReplaced += OnItemReplaced;
		}

		public override void Bind()
		{
			base.Bind();

			collectionBinding.Bind();
		}

		public override void Unbind()
		{
			base.Unbind();

			collectionBinding.Unbind();
		}

		private void OnCollectionReset()
		{
			Refresh();
		}

		private void OnItemAdded(int index, T value)
		{
			Refresh();
		}

		private void OnItemMoved(int oldIndex, int newIndex, T value)
		{
			Refresh();
		}

		private void OnItemRemoved(int index, T value)
		{
			Refresh();
		}

		private void OnItemReplaced(int index, T oldValue, T newValue)
		{
			Refresh();
		}
	}
}
