using UnityEngine;

namespace Maui
{
	public class DoubleScanOperator : ScanOperator<double>
	{
		[SerializeField] private MathOperationType operation;

		private int callCount;

		protected override void OnEnable()
		{
			callCount = 0;
			
			base.OnEnable();
		}

		protected override double Scan(double value, double accumulatedValue)
		{
			double result = value;
			
			switch (operation)
			{
				case MathOperationType.Addition: result = accumulatedValue + value; break;
				case MathOperationType.Substraction: result = accumulatedValue - value; break;
				case MathOperationType.Multiplication: result = accumulatedValue * value; break;
				case MathOperationType.Division: result = accumulatedValue / value; break;
				case MathOperationType.Power: result = callCount == 0 ? value : Mathf.Pow((float)accumulatedValue, (float)value); break;
			}

			callCount++;

			return result;
		}

		protected override double GetInitialAccumulatedValue()
		{
			double result = 0;
			
			switch (operation)
			{
				case MathOperationType.Addition: result = 0; break;
				case MathOperationType.Substraction: result = 0; break;
				case MathOperationType.Multiplication: result = 1; break;
				case MathOperationType.Division: result = 1; break;
				case MathOperationType.Power: result = 1; break;
			}

			return result;
		}
	}
}