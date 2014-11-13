using Engine.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editing
{
    /// <summary>
    /// This class will create edit interfaces for a list of a given object type. It will manage adding
    /// and removing items from that list through another EditInterface with callbacks for when this happens.
    /// By default it will use the ReflectedEditInterface.DefaultScanner, but you can change this by passing
    /// another one to the constructor.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReflectedListLikeEditInterface<T> : EditInterface
    {
        private ListlikeAbstractor<T> list;
        private List<ReflectedListItemEditableProperty<T>> properties = new List<ReflectedListItemEditableProperty<T>>();
        private Func<T> createNewCallback;
        private Action<T> removedCallback;
        private MemberScanner memberScanner;

        /// <summary>
        /// Create a new instance. The removedCallback and memberScanner are optional, if a member scanner is not provided the ReflectedEditInterface.DefaultScanner
        /// will be used.
        /// </summary>
        /// <param name="list">The list to wrap.</param>
        /// <param name="name">The name of the EditInterface created.</param>
        /// <param name="createNewCallback">This function is called when a new item is added to the list, it is required.</param>
        /// <param name="removedCallback">This funciton is called when an item is removed from the list, it can be null.</param>
        /// <param name="validateCallback">Optional callback to validate list contents.</param>
        /// <param name="memberScanner">The member scanner to use to find items to add to each entry's EditInterface, if null it will be the ReflectedEditInterface.DefaultScanner.</param>
        public ReflectedListLikeEditInterface(IList<T> list, String name, Func<T> createNewCallback, Action<T> removedCallback = null, Validate validateCallback = null, MemberScanner memberScanner = null)
            :base(name)
        {
            init(new IListAbstractor<T>(list), name, createNewCallback, removedCallback, validateCallback, memberScanner);
        }

        /// <summary>
        /// Create a new instance. The removedCallback and memberScanner are optional, if a member scanner is not provided the ReflectedEditInterface.DefaultScanner
        /// will be used.
        /// </summary>
        /// <param name="list">The list to wrap.</param>
        /// <param name="name">The name of the EditInterface created.</param>
        /// <param name="createNewCallback">This function is called when a new item is added to the list, it is required.</param>
        /// <param name="removedCallback">This funciton is called when an item is removed from the list, it can be null.</param>
        /// <param name="validateCallback">Optional callback to validate list contents.</param>
        /// <param name="memberScanner">The member scanner to use to find items to add to each entry's EditInterface, if null it will be the ReflectedEditInterface.DefaultScanner.</param>
        public ReflectedListLikeEditInterface(LinkedList<T> list, String name, Func<T> createNewCallback, Action<T> removedCallback = null, Validate validateCallback = null, MemberScanner memberScanner = null)
            :base(name)
        {
            init(new LinkedListAbstractor<T>(list), name, createNewCallback, removedCallback, validateCallback, memberScanner);
        }

        /// <summary>
        /// Sync the EditInterface for the list again, call if changes are made externally.
        /// </summary>
        public override void alertDataContentsChanged()
        {
            //Remove all old properties
            foreach(var property in properties)
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

        private void init(ListlikeAbstractor<T> list, String name, Func<T> createNewCallback, Action<T> removedCallback, Validate validateCallback, MemberScanner memberScanner)
        {
            this.list = list;
            this.createNewCallback = createNewCallback;
            this.removedCallback = removedCallback;
            this.validateCallback = validateCallback;

            //If no member scanner passed, set it to the default one
            if(memberScanner == null)
            {
                memberScanner = ReflectedEditInterface.DefaultScanner;
            }
            
            this.memberScanner = memberScanner;

            EditablePropertyInfo propertyInfo = new EditablePropertyInfo();
            foreach(var item in this.memberScanner.getMatchingMembers(typeof(T)))
            {
                propertyInfo.addColumn(new EditablePropertyColumn(item.getWrappedName(), false));
            }

            addPropertyCallback = add;
            removePropertyCallback = remove;

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
            var listProp = property as ReflectedListItemEditableProperty<T>;
            int index = properties.IndexOf(listProp);

            //Remove the item from the real list
            var item = list[index];
            removed(item);
            list.RemoveAt(index);

            //Remove the property from the edit interface and local listing.
            removeEditableProperty(listProp);
            properties.Remove(listProp);
        }

        private void addProperty(int index)
        {
            var prop = new ReflectedListItemEditableProperty<T>(ReflectedEditInterface.createEditInterface(list[index], memberScanner, "ListItem", null));
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

        internal T createNew()
        {
            return createNewCallback();
        }

        internal void removed(T value)
        {
            if(removedCallback != null)
            {
                removedCallback.Invoke(value);
            }
        }
    }
}
