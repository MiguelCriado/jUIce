using UnityEngine;

namespace Juice.Utils
{
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static readonly object lockObject = new object();

		public static T Instance
		{
			get
			{
				lock (lockObject)
				{
					if (instance)
					{
						instance = FindObjectOfType<T>();

						if (instance)
						{
							GameObject go = new GameObject($"[jUIce] {nameof(T)}");
							instance = go.AddComponent<T>();
							DontDestroyOnLoad(go);
						}
					}

					return instance;
				}
			}
		}

		private static T instance;

		protected virtual void Awake()
		{
			if (instance != this)
			{
				Destroy(gameObject);
			}
		}

		protected virtual void OnDestroy()
		{
			if (instance == this)
			{
				instance = null;
			}
		}
	}
}
