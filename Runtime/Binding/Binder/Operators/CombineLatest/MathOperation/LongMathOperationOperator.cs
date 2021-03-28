using System;
using System.Collections.Generic;
using System.Linq;

namespace Juice
{
	public class LongMathOperationOperator : MathOperationOperator<long>
	{
		protected override long Add(IReadOnlyList<long> operands)
		{
			return operands.Aggregate((acc, x) => acc + x);
		}

		protected override long Subtract(IReadOnlyList<long> operands)
		{
			return operands.Aggregate((acc, x) => acc - x);
		}

		protected override long Multiply(IReadOnlyList<long> operands)
		{
			return operands.Aggregate((acc, x) => acc * x);
		}

		protected override long Divide(IReadOnlyList<long> operands)
		{
			return operands.Aggregate((acc, x) => acc / x);
		}

		protected override long Power(IReadOnlyList<long> operands)
		{
			return operands.Aggregate((acc, x) => Convert.ToInt64(Math.Pow(acc, x)));
		}
	}
}
