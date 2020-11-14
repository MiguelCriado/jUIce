using UnityEngine;

namespace Maui.Pooling
{
	public partial class ObjectPool
	{
		public static ObjectPool Global
		{
			get
			{
				if (global == null)
				{
					GameObject poolGameObject = new GameObject("Global ObjectPool");
					global = poolGameObject.AddComponent<ObjectPool>();
				}

				return global;
			}
		}

		private static ObjectPool global;
	}
}
