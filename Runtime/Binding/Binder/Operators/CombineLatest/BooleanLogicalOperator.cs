using System.Collections.Generic;
using UnityEngine;

namespace Juice
{
	public class BooleanLogicalOperator : CombineLatestOperator<bool, bool>
	{
		public enum LogicalOperation
		{
			And,
			Or
		}

		[SerializeField] private LogicalOperation operation;

		protected override bool Process(int triggerIndex, IReadOnlyList<bool> list)
		{
			bool result = list[0];
			int i = 1;

			while (i < list.Count && KeepEvaluating(result))
			{
				result = Evaluate(result, list[i]);
				i++;
			}

			return result;
		}

		private bool Evaluate(bool a, bool b)
		{
			switch (operation)
			{
				default:
				case LogicalOperation.And: return a && b;
				case LogicalOperation.Or: return a || b;
			}
		}

		private bool KeepEvaluating(bool currentValue)
		{
			switch (operation)
			{
				default:
				case LogicalOperation.And: return currentValue == true;
				case LogicalOperation.Or: return currentValue == false;
			}
		}
	}
}
