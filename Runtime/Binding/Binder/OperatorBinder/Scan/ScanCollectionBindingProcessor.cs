using System;
using UnityEngine;

namespace Juice
{
	public class ScanCollectionBindingProcessor<T> : CollectionBindingProcessor<T, T>
	{
		private readonly Func<T, T, T> scanFunction;
		private readonly Func<T> initialAccumulatedValueGetter;

		public ScanCollectionBindingProcessor(
			BindingInfo bindingInfo,
			Component context,
			Func<T, T, T> scanFunction,
			Func<T> initialAccumulatedValueGetter)
			: base(bindingInfo, context)
		{
			this.scanFunction = scanFunction;
			this.initialAccumulatedValueGetter = initialAccumulatedValueGetter;
		}

		protected override T ProcessValue(T newValue, T oldValue, bool isNewItem)
		{
			return scanFunction(newValue, oldValue);
		}

		protected override void BoundCollectionItemAddedHandler(int index, T value)
		{
			T processedValue = ProcessValue(value, initialAccumulatedValueGetter(), true);
			processedCollection.Insert(index, processedValue);
		}

		protected override void BoundCollectionItemReplacedHandler(int index, T oldValue, T newValue)
		{
			T processedValue = ProcessValue(newValue, oldValue, false);
			processedCollection[index] = processedValue;
		}
	}
}