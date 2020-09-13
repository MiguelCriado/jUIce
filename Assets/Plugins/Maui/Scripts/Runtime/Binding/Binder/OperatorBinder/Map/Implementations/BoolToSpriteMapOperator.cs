using Maui.Collections;
using UnityEngine;

namespace Maui
{
	public class BoolToSpriteMapOperator : MapOperator<bool, Sprite>
	{
		protected override SerializableDictionary<bool, Sprite> Mapper => mapper;
		protected override ConstantBindingInfo Fallback => fallback;

		[SerializeField] private BoolSpriteDictionary mapper;
		[SerializeField] private SpriteConstantBindingInfo fallback;
	}
}