using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maui
{
	[Serializable]
	public class ObservableVariable<T> : IObservableVariable<T>
	{
		public event ObservableVariableEventHandler<T> Changed;

		public bool HasValue { get; protected set; }

		public T Value
		{
			get => value;

			set
			{
				if (Compare(value, this.value) == false)
				{
					SetValue(value);
					OnChanged(value);
				}
			}
		}

		[SerializeField] private T value;

		public ObservableVariable() : this(default)
		{

		}
		
		public ObservableVariable(T initialValue)
		{
			SetValue(initialValue);
		}

		protected virtual void OnChanged(T newValue)
		{
			Changed?.Invoke(newValue);
		}
		
		private bool Compare(T x, T y)
		{
			return EqualityComparer<T>.Default.Equals(x, y);
		}

		private void SetValue(T newValue)
		{
			value = newValue;
			HasValue = true;
		}
	}
}
