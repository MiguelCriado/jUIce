using Juice.Tweening;
using UnityEngine;

namespace Juice
{
	public abstract class TweenOperator<T> : ProcessorOperator<T, T>
	{
		private static readonly BindingType[] AllowedTypesConst = { BindingType.Variable, BindingType.Collection };
		
		protected override BindingType[] AllowedTypes => AllowedTypesConst;

		[SerializeField] private Ease ease;
		[SerializeField] private ConstantBindingInfo<float> duration = new ConstantBindingInfo<float>();
		
		private float Duration => durationBinding.Property.GetValue(0);

		private VariableBinding<float> durationBinding;
		
		protected override void Reset()
		{
			ease = Ease.InOutSine;
			duration.Constant = 0.3f;
		}
		
		protected override void Awake()
		{
			base.Awake();

			durationBinding = RegisterVariable<float>(duration).GetBinding();
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
			return BuildTweener(getter, setter, finalValue, Duration).SetEase(ease);
		}
	}
}