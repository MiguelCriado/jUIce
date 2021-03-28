using Juice.Collections;
using UnityEngine;

namespace Juice
{
	public class BoolToSpriteOperator : MapOperator<bool, Sprite>
	{
		protected override SerializableDictionary<bool, Sprite> Mapper => mapper;
		protected override ConstantBindingInfo Fallback => fallback;

		[SerializeField] private BoolSpriteDictionary mapper;
		[SerializeField] private SpriteConstantBindingInfo fallback;
	}
}
