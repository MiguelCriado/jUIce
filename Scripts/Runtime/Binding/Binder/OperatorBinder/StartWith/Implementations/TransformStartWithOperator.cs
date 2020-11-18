using UnityEngine;

namespace Juice
{
	public class TransformStartWithOperator : StartWithOperator<Transform>
	{
		protected override ConstantBindingInfo<Transform> InitialValue => initialValue;

		[SerializeField] private TransformConstantBindingInfo initialValue = new TransformConstantBindingInfo();
	}
}