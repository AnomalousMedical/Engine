using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This class is backed by a dictionary and a list. The list provides fast
    /// non garbage enumeration over the values in the dictionary. Note that the
    /// values in either collection will not be in any specific order in
    /// relation to each other due to the different ways these collections are
    /// stored in the background.
    /// </summary>
    /// <typeparam name="Key">The key type.</typeparam>
    /// <typeparam name="Value">The value type.</typeparam>
    public class FastIteratorMap<Key, Value> : IEnumerable<Value>
    {
        private Dictionary<Key, Value> dictionary = new Dictionary<Key, Value>();
        private List<Value> list = new List<Value>();

        public void Add(Key key, Value value)
        {
            dictionary.Add(key, value);
            list.Add(value);
        }

        public void Remove(Key key)
        {
            Value value;
            if(dictionary.TryGetValue(key, out value))
            {
                list.Remove(value);
                dictionary.Remove(key);
            }
        }

        public bool TryGetValue(Key key, out Value value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        public void Clear()
        {
            list.Clear();
            dictionary.Clear();
        }

        public Value this[Key key]
        {
            get
            {
                return dictionary[key];
            }
            set
            {
                dictionary[key] = value;
            }
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        /// <summary>
        /// Get the backing list. This is not a copy so the contents should not be modified.
        /// </summary>
        public List<Value> List
        {
            get
            {
                return list;
            }
        }

        public IEnumerator<Value> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
