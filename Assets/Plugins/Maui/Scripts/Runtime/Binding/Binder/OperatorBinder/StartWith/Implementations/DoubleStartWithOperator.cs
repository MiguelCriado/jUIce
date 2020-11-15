using UnityEngine;

namespace Maui
{
	public class DoubleStartWithOperator : StartWithOperator<double>
	{
		protected override ConstantBindingInfo<double> InitialValue => initialValue;

		[SerializeField] private DoubleConstantBindingInfo initialValue = new DoubleConstantBindingInfo();
	}
}