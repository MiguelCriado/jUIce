using Maui.Collections;
using UnityEngine;

namespace Maui
{
	public class BoolToSpriteMapOperator : MapOperator<bool, Sprite>
	{
		protected override SerializableDictionary<bool, Sprite> Mapper => mapper;

		[SerializeField] private BoolSpriteDictionary mapper;
	}
}