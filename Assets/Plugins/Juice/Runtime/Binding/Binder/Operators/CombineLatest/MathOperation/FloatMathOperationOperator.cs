using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Juice
{
	public class FloatMathOperationOperator : MathOperationOperator<float>
	{
		protected override float Add(IReadOnlyList<float> operands)
		{
			return operands.Aggregate((acc, x) => acc + x);
		}

		protected override float Subtract(IReadOnlyList<float> operands)
		{
			return operands.Aggregate((acc, x) => acc - x);
		}

		protected override float Multiply(IReadOnlyList<float> operands)
		{
			return operands.Aggregate((acc, x) => acc * x);
		}

		protected override float Divide(IReadOnlyList<float> operands)
		{
			return operands.Aggregate((acc, x) => acc / x);
		}

		protected override float Power(IReadOnlyList<float> operands)
		{
			return operands.Aggregate(Mathf.Pow);
		}
	}
}
