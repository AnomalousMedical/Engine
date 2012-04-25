using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace libRocketPlugin
{
    public class RktDictionaryIterator : IEnumerator<RktEntry>
    {
        private RktDictionary dictionary;
        private int position;
        private RktEntry current = new RktEntry();

        public RktDictionaryIterator(RktDictionary dictionary)
        {
            this.dictionary = dictionary;
        }

        public void Dispose()
        {
            
        }

        public RktEntry Current
        {
            get
            {
                return current;
            }
        }

        object System.Collections.IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public bool MoveNext()
        {
            return dictionary.Iterate(ref position, out current.key, out current.value);
        }

        public void Reset()
        {
            position = 0;
        }
    }
}
