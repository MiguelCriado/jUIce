using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maui.Collections
{
	public abstract class SerializableDictionary
	{
		
	}
	
	[Serializable]
	public class SerializableDictionary<TKey, TValue> : SerializableDictionary, IDictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		public int Count => dictionary.Count;
		public bool IsReadOnly => false;
		public ICollection<TKey> Keys => dictionary.Keys;
		public ICollection<TValue> Values => dictionary.Values;

		public TValue this[TKey key]
		{
			get => dictionary[key];
			set => dictionary[key] = value;
		}
		
		[SerializeField] private List<TKey> keys = new List<TKey>();
		[SerializeField] private List<TValue> values = new List<TValue>();
		[SerializeField] private TKey newKeyHelper;
		[SerializeField] private TValue newValueHelper;
		
		private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

		private void Awake()
		{
			BuildDictionary();
		}

		public void Add(TKey key, TValue value)
		{
			dictionary.Add(key, value);
		}

		public bool ContainsKey(TKey key)
		{
			return dictionary.ContainsKey(key);
		}

		public bool Remove(TKey key)
		{
			return dictionary.Remove(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return dictionary.TryGetValue(key, out value);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return dictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			dictionary.Add(item.Key, item.Value);
		}

		public void Clear()
		{
			dictionary.Clear();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return dictionary.Contains(item);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			foreach (KeyValuePair<TKey,TValue> kvp in dictionary)
			{
				array[arrayIndex] = kvp;
				arrayIndex++;
			}
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			bool result = false;
			
			if (dictionary.TryGetValue(item.Key, out TValue value) &&
			    EqualityComparer<TValue>.Default.Equals(item.Value, value))
			{
				result = dictionary.Remove(item.Key);
			}

			return result;
		}
		
		public void OnBeforeSerialize()
		{
			keys = new List<TKey>();
			values = new List<TValue>();
			
			foreach (var kvp in dictionary)
			{
				keys.Add(kvp.Key);
				values.Add(kvp.Value);
			}
		}

		public void OnAfterDeserialize()
		{
			BuildDictionary();
		}
		
		private void BuildDictionary()
		{
			dictionary = new Dictionary<TKey, TValue>();

			for (int i = 0; i < keys.Count; i++)
			{
				dictionary[keys[i]] = values[i];
			}
		}
	}
}