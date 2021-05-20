using Juice.Tweening;
using UnityEngine;

namespace Juice
{
	public class TweenCollectionBindingProcessor<T> : CollectionBindingProcessor<T, T>
	{
		public delegate Tweener BuildTweener(Tweener<T>.Getter getter, Tweener<T>.Setter setter, T finalValue);
		
		private readonly BuildTweener tweenBuilder;

		private Tweener lastTweener;
		
		public TweenCollectionBindingProcessor(BindingInfo bindingInfo, Component context, BuildTweener tweenBuilder)
			: base(bindingInfo, context)
		{
			this.tweenBuilder = tweenBuilder;
		}

		protected override T ProcessValue(T newValue, T oldValue, bool isNewItem)
		{
			return newValue;
		}

		protected override void OnBoundCollectionItemReplaced(int index, T oldValue, T newValue)
		{
			lastTweener?.Kill();
			lastTweener = tweenBuilder(() => processedCollection[index], x => processedCollection[index] = x, newValue);
		}
	}
}