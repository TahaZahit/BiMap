using System;
using System.Collections.Generic;
using System.Text;

namespace BiMap
{
    public class BiMap<TKey,TValue>
    {
        private Dictionary<TKey,TValue> normal;
        private Dictionary<TValue,TKey> reverse;

        public BiMap()
        {
            normal = new Dictionary<TKey, TValue>();
            reverse = new Dictionary<TValue, TKey>();
        }
        public BiMap(Dictionary<TKey,TValue> dictionary)
        {
            normal = new Dictionary<TKey, TValue>(dictionary);
            reverse = new Dictionary<TValue, TKey>();

            foreach (var item in normal)
            {
                reverse.Add(item.Value, item.Key);
            }
        }

        public TValue GetValue(TKey key)
        {
            TValue value;
            normal.TryGetValue(key, out value);
            return value;
        }
        public TKey GetKey(TValue value)
        {
            TKey key;
            reverse.TryGetValue(value, out key);
            return key;
        }
        public void Add(TKey key,TValue value)
        {
            normal.Add(key, value);
            reverse.Add(value, key);
        }
    }
}
