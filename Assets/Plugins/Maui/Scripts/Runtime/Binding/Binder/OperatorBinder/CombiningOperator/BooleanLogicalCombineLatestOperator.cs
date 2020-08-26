using System.Collections.Generic;
using UnityEngine;

namespace Maui
{
	public class BooleanLogicalCombineLatestOperator : CombiningOperator<bool, bool>
	{
		public enum LogicalOperation
		{
			AND,
			OR
		}

		[SerializeField] private LogicalOperation operation;

		protected override bool Combine(int triggerIndex, IReadOnlyList<bool> list)
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
				case LogicalOperation.AND: return a && b;
				case LogicalOperation.OR: return a || b;
			}
		}

		private bool KeepEvaluating(bool currentValue)
		{
			switch (operation)
			{
				default:
				case LogicalOperation.AND: return currentValue == true;
				case LogicalOperation.OR: return currentValue == false;
			}
		}
	}
}