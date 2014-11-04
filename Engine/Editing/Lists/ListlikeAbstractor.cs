using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editing
{
    interface ListlikeAbstractor<T> : IEnumerable<T>
    {
        T this[int index] { get; set; }

        void Add(T item);

        void Remove(T item);

        void RemoveAt(int index);

        int Count { get; }
    }
}
