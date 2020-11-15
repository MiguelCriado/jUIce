using UnityEngine;

namespace Maui
{
	public class StringStartWithOperator : StartWithOperator<string>
	{
		protected override ConstantBindingInfo<string> InitialValue => initialValue;

		[SerializeField] private StringConstantBindingInfo initialValue = new StringConstantBindingInfo();
	}
}