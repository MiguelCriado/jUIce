using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Juice
{
	public class IntMathOperationOperator : MathOperationOperator<int>
	{
		protected override int Add(IReadOnlyList<int> operands)
		{
			return operands.Aggregate((acc, x) => acc + x);
		}

		protected override int Subtract(IReadOnlyList<int> operands)
		{
			return operands.Aggregate((acc, x) => acc - x);
		}

		protected override int Multiply(IReadOnlyList<int> operands)
		{
			return operands.Aggregate((acc, x) => acc * x);
		}

		protected override int Divide(IReadOnlyList<int> operands)
		{
			return operands.Aggregate((acc, x) => acc / x);
		}

		protected override int Power(IReadOnlyList<int> operands)
		{
			return operands.Aggregate((acc, x) => Mathf.RoundToInt(Mathf.Pow(acc, x)));
		}
	}
}
