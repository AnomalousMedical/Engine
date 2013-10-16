using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class RemovableListIterator<T>
    {
        private int enumerationLength = -1;
        private int enumerationPosition = -1;
        private List<T> list;

        public RemovableListIterator(List<T> list)
        {
            this.list = list;
        }

        public IEnumerable<T> Values
        {
            get
            {
                return enumerate();
            }
        }

        public void RemoveCurrent()
        {
            list.RemoveAt(enumerationPosition);
            --enumerationPosition;
            --enumerationLength;
        }

        private IEnumerable<T> enumerate()
        {
            enumerationLength = list.Count;
            for (enumerationPosition = 0; enumerationPosition < enumerationLength; ++enumerationPosition)
            {
                yield return list[enumerationPosition];
            }
            enumerationLength = -1;
            enumerationPosition = -1;
        }
    }
}
