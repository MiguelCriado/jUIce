using UnityEngine;

namespace Juice
{
	public class Vector3DecorateCommandOperator : DecorateCommandOperator<Vector3>
	{
		protected override ConstantBindingInfo<Vector3> DecorationBindingInfo => decorationBindingInfo;

		[SerializeField] private Vector3ConstantBindingInfo decorationBindingInfo = new Vector3ConstantBindingInfo();
	}
}