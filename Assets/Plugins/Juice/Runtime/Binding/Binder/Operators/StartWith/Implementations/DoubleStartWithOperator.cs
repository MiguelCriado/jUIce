using UnityEngine;

namespace Juice
{
	public class DoubleStartWithOperator : StartWithOperator<double>
	{
		protected override ConstantBindingInfo<double> InitialValue => initialValue;

		[SerializeField] private DoubleConstantBindingInfo initialValue = new DoubleConstantBindingInfo();
	}
}