using UnityEngine;

namespace Juice
{
	public class Vector2StartWithOperator : StartWithOperator<Vector2>
	{
		protected override ConstantBindingInfo<Vector2> InitialValue => initialValue;

		[SerializeField] private Vector2ConstantBindingInfo initialValue = new Vector2ConstantBindingInfo();
	}
}