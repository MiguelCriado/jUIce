using System;
using System.Collections.Generic;
using System.Linq;

namespace Juice
{
	public class DoubleMathOperationOperator : MathOperationOperator<double>
	{
		protected override double Add(IReadOnlyList<double> operands)
		{
			return operands.Aggregate((acc, x) => acc + x);
		}

		protected override double Subtract(IReadOnlyList<double> operands)
		{
			return operands.Aggregate((acc, x) => acc - x);
		}

		protected override double Multiply(IReadOnlyList<double> operands)
		{
			return operands.Aggregate((acc, x) => acc * x);
		}

		protected override double Divide(IReadOnlyList<double> operands)
		{
			return operands.Aggregate((acc, x) => acc / x);
		}

		protected override double Power(IReadOnlyList<double> operands)
		{
			return operands.Aggregate(Math.Pow);
		}
	}
}
