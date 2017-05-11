using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// A wrapper for the built in OrderedDictionary that makes it generic.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class OrderedDictionary<TKey, TValue>
    {
        /// <summary>
        /// This class ensures that we always lookup the same key value pair
        /// </summary>
        private class Lookup
        {
            public Lookup()
            {

            }

            public Lookup(TKey key, TValue value)
            {
                this.Key = key;
                this.Value = value;
            }

            public TKey Key { get; set; }

            public TValue Value { get; set; }
        }

        private List<Lookup> order = new List<Lookup>();
        private Dictionary<TKey, Lookup> dictionary = new Dictionary<TKey, Lookup>();

        public OrderedDictionary()
        {

        }

        public void Insert(int index, TKey key, TValue value)
        {
            var lookup = new Lookup(key, value);
            order.Insert(index, lookup);
            dictionary[key] = lookup;
        }

        public void RemoveAt(int index)
        {
            var item = order[index];
            dictionary.Remove(item.Key);
            order.RemoveAt(index);
        }

        public TValue this[int index]
        {
            get
            {
                return order[index].Value;
            }
            set
            {
                order[index].Value = value; //Because the lookup object is the same changing this changes it in both places.
            }
        }

        public void Add(TKey key, TValue value)
        {
            var lookup = new Lookup(key, value);
            order.Add(lookup);
            dictionary[key] = lookup;
        }

        public void Clear()
        {
            order.Clear();
            dictionary.Clear();
        }

        public bool Contains(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                foreach (var key in order.Select(i => i.Key))
                {
                    yield return key;
                }
            }
        }

        public void Remove(TKey key)
        {
            var lookup = dictionary[key];
            order.Remove(lookup);
            dictionary.Remove(key);
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                foreach (var value in order.Select(i => i.Value))
                {
                    yield return value;
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return dictionary[key].Value;
            }
            set
            {
                dictionary[key].Value = value;
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            Lookup lookup;
            if (dictionary.TryGetValue(key, out lookup))
            {
                value = lookup.Value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        public int Count
        {
            get
            {
                return order.Count;
            }
        }
    }
}
