using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Juice.Plugins.Juice.Runtime.Utils
{
	[Serializable]
	public struct BindingPath
	{
		public string ComponentId => componentId;
		public string PropertyName => propertyName;

		[SerializeField] private string componentId;
		[SerializeField] private string propertyName;

		public BindingPath(string path)
		{
			string[] splitPath = path.Split('.');

			Assert.IsFalse(splitPath.Length > 2, $"Invalid path\"path\". It may only contain 1 separator ('.')");

			componentId = null;
			propertyName = null;

			if (splitPath.Length >= 2)
			{
				componentId = splitPath[0];
				propertyName = splitPath[1];
			}
			else if (splitPath.Length >= 1)
			{
				propertyName = splitPath[0];
			}
		}

		public BindingPath(string componentId, string propertyName)
		{
			this.componentId = componentId;
			this.propertyName = propertyName;
		}
	}
}
