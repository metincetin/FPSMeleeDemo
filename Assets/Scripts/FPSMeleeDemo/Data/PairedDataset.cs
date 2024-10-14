using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSMeleeDemo.Data
{
	public abstract class PairedDataset<K, V> : ScriptableObject where K: class where V: class
	{
		[System.Serializable]
		public class Data
		{
			public K Key;
			public V Value;
		}

		[SerializeField]
		private V _default;

		[SerializeField]
		private Data[] _data;

		public V Get(K key)
		{
			foreach(var d in _data)
			{
				if (d.Key == key) return d.Value;
			}
			return null;
		}
		public V GetOrDefault(K key)
		{
			if (key == null) return _default;
			if (TryGet(key, out var value))
			{
				return value;
			}
			return _default;
		}

		public bool TryGet(K key, out V value)
		{
			value = Get(key);

			if (value == null) return false;
			return true;
		}
    }
}
