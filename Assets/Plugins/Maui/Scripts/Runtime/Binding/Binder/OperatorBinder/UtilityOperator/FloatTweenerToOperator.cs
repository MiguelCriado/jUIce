using Maui.Tweening;
using UnityEngine;

namespace Maui
{
	public class FloatTweenerToOperator : ToOperator<float, float>
	{
		[SerializeField] private Ease ease;
		[SerializeField] private float duration;

		protected override void Reset()
		{
			ease = Ease.InOutSine;
			duration = 0.3f;
		}

		protected override float Convert(float value)
		{
			// Tween.To(() => convertedVariable.Value, x => convertedVariable.Value = x, value, duration).SetEase(ease);
			// return convertedVariable.Value;
			return value;
		}
	}
}