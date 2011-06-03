using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MyGUIPlugin
{
    public class TableElementCollection<CollectionType> : IEnumerable<CollectionType>, IDisposable
        where CollectionType : TableElement
    {
        private List<CollectionType> items;
        private Table table;

        internal TableElementCollection()
        {
            items = new List<CollectionType>();
        }

        /// <summary>
        /// All items in this collection will be disposed when it is disposed or cleared.
        /// </summary>
        public void Dispose()
        {
            foreach (CollectionType item in items)
            {
                item.Dispose();
            }
        }

        public virtual void add(CollectionType item)
        {
            items.Add(item);
            item.Table = table;
        }

        /// <summary>
        /// If an item is removed from the collection the user must dispose it.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        public virtual void remove(CollectionType item)
        {
            items.Remove(item);
            item.Table = null;
        }

        /// <summary>
        /// Clear all elements in this collection. This will call dispose on the
        /// elements that are in the collection.
        /// </summary>
        public virtual void clear()
        {
            foreach (CollectionType item in items)
            {
                item.Dispose();
            }
            items.Clear();
        }

        public int getItemIndex(CollectionType item)
        {
            return items.IndexOf(item);
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        internal Table Table
        {
            get
            {
                return table;
            }
            set
            {
                this.table = value;
                foreach (CollectionType item in items)
                {
                    item.Table = table;
                }
            }
        }

        public CollectionType this[int i]
        {
            get
            {
                return items[i];
            }
            set
            {
                items.Insert(i, value);
                if (items.Count > i + 1)
                {
                    items.RemoveAt(i + 1);
                }
            }
        }

        public IEnumerator<CollectionType> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
