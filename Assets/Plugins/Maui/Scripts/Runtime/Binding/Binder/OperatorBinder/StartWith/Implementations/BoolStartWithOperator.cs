using UnityEngine;

namespace Maui
{
	public class BoolStartWithOperator : StartWithOperator<bool>
	{
		protected override ConstantBindingInfo<bool> InitialValue => initialValue;

		[SerializeField] private BoolConstantBindingInfo initialValue = new BoolConstantBindingInfo();
	}
}