using UnityEngine;

namespace Juice
{
	public class LongScanOperator : ScanOperator<long>
	{
		[SerializeField] private MathOperationType operation;

		private int callCount;

		protected override void OnEnable()
		{
			callCount = 0;
			
			base.OnEnable();
		}

		protected override long Scan(long value, long accumulatedValue)
		{
			long result = value;
			
			switch (operation)
			{
				case MathOperationType.Addition: result = accumulatedValue + value; break;
				case MathOperationType.Substraction: result = accumulatedValue - value; break;
				case MathOperationType.Multiplication: result = accumulatedValue * value; break;
				case MathOperationType.Division: result = accumulatedValue / value; break;
				case MathOperationType.Power: result = callCount == 0 ? value : (long)Mathf.Pow(accumulatedValue, value); break;
			}

			callCount++;

			return result;
		}

		protected override long GetInitialAccumulatedValue()
		{
			long result = 0;
			
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