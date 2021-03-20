using System.Collections.Generic;
using UnityEngine;

namespace Juice
{
	public abstract class MathOperationOperator<T> : BindingListOperatorBinder<T, T>
	{
		[SerializeField] private MathOperationType operation;

		protected override T Process(int triggerIndex, IReadOnlyList<T> list)
		{
			T result = default;

			switch (operation)
			{
				case MathOperationType.Addition: result = Add(list); break;
				case MathOperationType.Subtraction: result = Subtract(list); break;
				case MathOperationType.Multiplication: result = Multiply(list); break;
				case MathOperationType.Division: result = Divide(list); break;
				case MathOperationType.Power: result = Power(list); break;
			}

			return result;
		}

		protected abstract T Add(IReadOnlyList<T> operands);
		protected abstract T Subtract(IReadOnlyList<T> operands);
		protected abstract T Multiply(IReadOnlyList<T> operands);
		protected abstract T Divide(IReadOnlyList<T> operands);
		protected abstract T Power(IReadOnlyList<T> operands);
	}
}
