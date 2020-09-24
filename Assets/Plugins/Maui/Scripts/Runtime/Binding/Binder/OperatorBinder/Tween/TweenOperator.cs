using Maui.Tweening;
using UnityEngine;

namespace Maui
{
	public abstract class TweenOperator<T> : ProcessorOperatorBinder<T, T>
	{
		private static readonly BindingType[] AllowedTypesConst = { BindingType.Variable, BindingType.Collection };
		
		protected override BindingType[] AllowedTypes => AllowedTypesConst;

		[SerializeField] private Ease ease;
		[SerializeField] private float duration;
		
		protected override void Reset()
		{
			ease = Ease.InOutSine;
			duration = 0.3f;
		}
		
		protected override IBindingProcessor GetBindingProcessor(BindingType bindingType, BindingInfo fromBinding)
		{
			IBindingProcessor result = null;

			switch (bindingType)
			{
				case BindingType.Variable: 
					result = new TweenVariableBindingProcessor<T>(fromBinding, this, BuildTweener);
					break;
				case BindingType.Collection:
					result = new TweenCollectionBindingProcessor<T>(fromBinding, this, BuildTweener);
					break;
			}
			
			return result;
		}

		protected abstract Tweener<T> BuildTweener(Tweener<T>.Getter getter, Tweener<T>.Setter setter, T finalValue, float duration);

		private Tweener BuildTweener(Tweener<T>.Getter getter, Tweener<T>.Setter setter, T finalValue)
		{
			return BuildTweener(getter, setter, finalValue, duration).SetEase(ease);
		}
	}
}