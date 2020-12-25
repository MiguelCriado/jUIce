using UnityEngine;

namespace Juice
{
	public class SpriteDecorateCommandOperator : DecorateCommandOperator<Sprite>
	{
		protected override ConstantBindingInfo<Sprite> DecorationBindingInfo => decorationBindingInfo;

		[SerializeField] private SpriteConstantBindingInfo decorationBindingInfo = new SpriteConstantBindingInfo();
	}
}