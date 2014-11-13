using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editing
{
    public abstract class ListlikeEditInterface<T> : EditInterface
    {
        private ListlikeAbstractor<T> list;
        private List<ListItemEditableProperty<T>> properties = new List<ListItemEditableProperty<T>>();

        public ListlikeEditInterface(IList<T> list, String name, Validate validateCallback)
            : base(name)
        {
            init(new IListAbstractor<T>(list), name, validateCallback);
        }

        public ListlikeEditInterface(LinkedList<T> list, String name, Validate validateCallback)
            : base(name)
        {
            init(new LinkedListAbstractor<T>(list), name, validateCallback);
        }

        /// <summary>
        /// Sync the EditInterface for the list again, call if changes are made externally.
        /// </summary>
        public override void dataContentsChanged()
        {
            //Remove all old properties
            foreach (var property in properties)
            {
                removeEditableProperty(property);
            }
            properties.Clear();

            //Readd all properties
            for (int i = 0; i < list.Count; ++i)
            {
                addProperty(i);
            }
        }

        private void init(ListlikeAbstractor<T> list, String name, Validate validateCallback)
        {
            this.list = list;

            EditablePropertyInfo propertyInfo = new EditablePropertyInfo();
            propertyInfo.addColumn(new EditablePropertyColumn("Value", false));

            this.addPropertyCallback = add;
            this.removePropertyCallback = remove;
            this.validateCallback = validateCallback;

            setPropertyInfo(propertyInfo);

            for(int i = 0; i < list.Count; ++i)
            {
                addProperty(i);
            }
        }

        private void add(EditUICallback callback)
        {
            list.Add(createNew());
            addProperty(list.Count - 1);
        }

        private void remove(EditUICallback callback, EditableProperty property)
        {
            var listProp = property as ListItemEditableProperty<T>;
            int index = listProp.Index;

            //Remove the item from the real list
            var item = list[index];
            removed(item);
            list.RemoveAt(index);

            //Reindex the remaining properties
            foreach(var reindex in properties.Skip(index))
            {
                reindex.Index--;
            }

            //Remove the property from the edit interface and local listing.
            removeEditableProperty(listProp);
            properties.Remove(listProp);
        }

        private void addProperty(int index)
        {
            var prop = new ListItemEditableProperty<T>(index, this);
            properties.Add(prop);
            addEditableProperty(prop);
        }

        internal T this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                list[index] = value;
            }
        }

        protected internal abstract T createNew();

        protected internal abstract void removed(T value);

        protected internal abstract T parseString(String value);

        protected internal abstract bool canParseString(string value, out string errorMessage);
    }
}
