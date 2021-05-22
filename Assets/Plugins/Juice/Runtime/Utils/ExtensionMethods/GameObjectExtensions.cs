using UnityEngine;

namespace Juice.Utils
{
	public static class GameObjectExtensions
	{
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			T result = gameObject.GetComponent<T>();

			if (!result)
			{
				result = gameObject.AddComponent<T>();
			}

			return result;
		}
	}
}
