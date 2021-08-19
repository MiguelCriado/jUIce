using Juice.Collections;
using UnityEngine;

namespace Juice
{
	public class TransitionPickerOptions : MonoBehaviour
	{
		[SerializeField] private SerializableDictionary<string, ComponentTransition> options = default;

		public bool TryGetTransition(string key, out ComponentTransition option)
		{
			return options.TryGetValue(key, out option);
		}
	}
}
