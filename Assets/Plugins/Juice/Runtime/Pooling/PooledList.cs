using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Juice.Pooling
{
	public class PooledList<T> : List<T>, IDisposable
	{
		private static readonly Stack<PooledList<T>> Pool = new Stack<PooledList<T>>();

		private bool isActive;

		public static PooledList<T> Get()
		{
			PooledList<T> list;

			if (Pool.Count == 0)
			{
				list = new PooledList<T>
				{
					isActive = true
				};
			}
			else
			{
				list = Pool.Pop();
				list.isActive = true;
			}

			return list;
		}

		public void Dispose()
		{
			Assert.IsTrue(isActive);
			isActive = false;
			Clear();
			Pool.Push(this);
		}
	}
}
