using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editing
{
    class LinkedListAbstractor<T> : ListlikeAbstractor<T>
    {
        private LinkedList<T> list;

        public LinkedListAbstractor(LinkedList<T> list)
        {
            this.list = list;
        }

        public T this[int index]
        {
            get
            {
                return list.Skip(index).First();
            }
            set
            {
                var node = findNode(index);
                list.AddAfter(node, value);
                list.Remove(node);
            }
        }

        public void Add(T item)
        {
            list.AddLast(item);
        }

        public void Remove(T item)
        {
            list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            list.Remove(findNode(index));
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        private LinkedListNode<T> findNode(int index)
        {
            var node = list.First;
            for (int i = 0; i < index; ++i)
            {
                node = node.Next;
            }
            return node;
        }
    }
}
