using UnityEngine;

namespace Maui
{
	public class IntStartWithOperator : StartWithOperator<int>
	{
		protected override ConstantBindingInfo<int> InitialValue => initialValue;

		[SerializeField] private IntConstantBindingInfo initialValue = new IntConstantBindingInfo();
	}
}