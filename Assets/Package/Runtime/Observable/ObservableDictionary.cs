using System;
using System.Collections;
using System.Collections.Generic;

namespace Muui
{
	public class ObservableDictionary<TKey, TValue> : IObservableDictionary<TKey, TValue>, IDictionary
	{
		public event DictionaryAddDelegate<TKey, TValue> OnAdd;
		public event DictionaryCountChangeDelegate OnCountChange;
		public event DictionaryRemoveDelegate<TKey, TValue> OnRemove;
		public event DictionaryReplaceDelegate<TKey, TValue> OnReplace;
		public event DictionaryResetDelegate OnReset;

		public int Count => innerDictionary.Count;
		public bool IsSynchronized => ((IDictionary)innerDictionary).IsSynchronized;
		public object SyncRoot => ((IDictionary)innerDictionary).SyncRoot;
		public bool IsReadOnly  => ((IDictionary)innerDictionary).IsReadOnly;
		public bool IsFixedSize  => ((IDictionary)innerDictionary).IsFixedSize;
		public ICollection<TKey> Keys => innerDictionary.Keys;
		public ICollection<TValue> Values => innerDictionary.Values;

		int IReadOnlyObservableDictionary<TKey, TValue>.Count => Count;
		ICollection IDictionary.Keys => ((IDictionary)innerDictionary).Keys;
		ICollection IDictionary.Values => ((IDictionary)innerDictionary).Values;

		private readonly Dictionary<TKey, TValue> innerDictionary;

		public TValue this[TKey key]
		{
			get => innerDictionary[key];

			set
			{
				if (TryGetValue(key, out TValue oldValue))
				{
					innerDictionary[key] = value;
					OnReplace?.Invoke(key, oldValue, value);
				}
				else
				{
					innerDictionary[key] = value;
					OnAdd?.Invoke(key, value);
					OnCountChange?.Invoke(Count);
				}
			}
		}

		public object this[object key]
		{
			get => this[(TKey)key];
			set => this[(TKey)key] = (TValue)value;
		}

		public ObservableDictionary()
		{
			innerDictionary = new Dictionary<TKey, TValue>();
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return innerDictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return innerDictionary.GetEnumerator();
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IDictionary)innerDictionary).GetEnumerator();
		}

		public void Add(TKey key, TValue value)
		{
			innerDictionary.Add(key, value);

			OnAdd?.Invoke(key, value);
			OnCountChange?.Invoke(Count);
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			Add(item.Key, item.Value);
		}

		public void Add(object key, object value)
		{
			Add((TKey)key, (TValue)value);
		}

		public bool Remove(TKey key)
		{
			bool result = false;

			if (innerDictionary.TryGetValue(key, out TValue oldValue))
			{
				result = innerDictionary.Remove(key);

				if (result == true)
				{
					OnRemove?.Invoke(key, oldValue);
					OnCountChange?.Invoke(Count);
				}
			}

			return result;
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			bool result = false;

			if (TryGetValue(item.Key, out TValue value)
				&& EqualityComparer<TValue>.Default.Equals(value, item.Value))
			{
				result = Remove(item.Key);
			}

			return result;
		}

		public void Remove(object key)
		{
			Remove((TKey)key);
		}

		public void Clear()
		{
			int lastCount = Count;
			innerDictionary.Clear();

			OnReset?.Invoke();

			if (lastCount > 0)
			{
				OnCountChange?.Invoke(Count);
			}
		}

		public bool ContainsKey(TKey key)
		{
			return innerDictionary.ContainsKey(key);
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return ContainsKey(item.Key)
					&& EqualityComparer<TValue>.Default.Equals(this[item.Key], item.Value);
		}

		public bool Contains(object key)
		{
			return ContainsKey((TKey) key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return innerDictionary.TryGetValue(key, out value);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			((IDictionary)innerDictionary).CopyTo(array, arrayIndex);
		}

		public void CopyTo(Array array, int index)
		{
			((IDictionary)innerDictionary).CopyTo(array, index);
		}
	}
}
