using Maui.Tweening;
using UnityEngine;

namespace Maui
{
	public class TweenVariableBindingProcessor<T> : VariableBindingProcessor<T, T>
	{
		public delegate Tweener BuildTweener(Tweener<T>.Getter getter, Tweener<T>.Setter setter, T finalValue);
		
		private readonly BuildTweener tweenBuilder;
		
		public TweenVariableBindingProcessor(BindingInfo bindingInfo, Component context, BuildTweener tweenBuilder)
			: base(bindingInfo, context)
		{
			this.tweenBuilder = tweenBuilder;
		}

		protected override void BoundVariableChangedHandler(T newValue)
		{
			tweenBuilder(() => processedVariable.Value, x => processedVariable.Value = x, newValue);
		}

		protected override T ProcessValue(T value)
		{
			return value;
		}
	}
}