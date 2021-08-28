using System;
using System.Collections.Generic;
using Juice.Collections;
using UnityEngine;

namespace Juice
{
	public abstract class EnumActivateWidgetsBinder<T> : ComponentBinder where T : Enum
	{
		[SerializeField] private BindingInfo enumValue = BindingInfo.Variable<T>();

		protected abstract SerializableDictionary<T, Widget> Mapper { get; }

		protected override void Awake()
		{
			base.Awake();

			RegisterVariable<T>(enumValue).OnChanged(OnEnumValueChanged);
		}

		private void OnEnumValueChanged(T newValue)
		{
			UpdateGameObjects(newValue);
		}

		private void UpdateGameObjects(T value)
		{
			foreach (KeyValuePair<T, Widget> pair in Mapper)
			{
				bool matchesEnum = pair.Key.Equals(value);
				Widget widget = pair.Value;

				if (matchesEnum)
				{
					widget.ShowWidget();
				}
				else
				{
					widget.HideWidget();
				}
			}
		}
	}
}