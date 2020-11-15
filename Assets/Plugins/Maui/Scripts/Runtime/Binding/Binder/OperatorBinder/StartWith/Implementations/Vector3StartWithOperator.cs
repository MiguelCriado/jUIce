using UnityEngine;

namespace Maui
{
	public class Vector3StartWithOperator : StartWithOperator<Vector3>
	{
		protected override ConstantBindingInfo<Vector3> InitialValue => initialValue;

		[SerializeField] private Vector3ConstantBindingInfo initialValue = new Vector3ConstantBindingInfo();
	}
}