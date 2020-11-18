using System;
using UnityEngine;

namespace Juice
{
	public class LifecycleUtils : MonoBehaviour
	{
		public static event Action OnUpdate
		{
			add => Instance.onUpdate += value;
			remove => Instance.onUpdate -= value;
		}
		
		public static event Action OnLateUpdate
		{
			add => Instance.onLateUpdate += value;
			remove => Instance.onLateUpdate -= value;
		}

		private event Action onUpdate;
		private event Action onLateUpdate;
		
		private static LifecycleUtils Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject gameObject = new GameObject("LifecycleUtils");
					instance = gameObject.AddComponent<LifecycleUtils>();
					DontDestroyOnLoad(gameObject);
				}

				return instance;
			}
		}
		
		private static LifecycleUtils instance;

		private void Start()
		{
			if (instance != this)
			{
				Destroy(gameObject);
			}
		}

		private void Update()
		{
			onUpdate?.Invoke();
		}

		private void LateUpdate()
		{
			onLateUpdate?.Invoke();
		}
	}
}