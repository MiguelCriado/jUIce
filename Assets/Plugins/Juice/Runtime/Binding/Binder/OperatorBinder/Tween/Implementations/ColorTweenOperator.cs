using Juice.Tweening;
using UnityEngine;

namespace Juice
{
	public class ColorTweenOperator : TweenOperator<Color>
	{
		protected override Tweener<Color> BuildTweener(Tweener<Color>.Getter getter, Tweener<Color>.Setter setter, Color finalValue, float duration)
		{
			return Tween.To(getter, setter, finalValue, duration);
		}
	}
}