using UnityEngine;

namespace Juice
{
	public class Vector2DecorateCommandOperator : DecorateCommandOperator<Vector2>
	{
		protected override ConstantBindingInfo<Vector2> DecorationBindingInfo => decorationBindingInfo;

		[SerializeField] private Vector2ConstantBindingInfo decorationBindingInfo = new Vector2ConstantBindingInfo();
		
	}
}