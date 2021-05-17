using UnityEngine.Assertions;

namespace Juice.Plugins.Juice.Runtime.Utils
{
	public readonly struct BindingPath
	{
		public string ComponentId { get; }
		public string PropertyName { get; }

		public BindingPath(string path)
		{
			string[] splitPath = path.Split('.');

			Assert.IsFalse(splitPath.Length > 2, $"Invalid path\"path\". It may only contain 1 separator ('.')");

			ComponentId = null;
			PropertyName = null;

			if (splitPath.Length >= 2)
			{
				ComponentId = splitPath[0];
				PropertyName = splitPath[1];
			}
			else if (splitPath.Length >= 1)
			{
				PropertyName = splitPath[0];
			}
		}

		public BindingPath(string componentId, string propertyName)
		{
			ComponentId = componentId;
			PropertyName = propertyName;
		}
	}
}
