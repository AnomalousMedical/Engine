using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine.Editing
{
    class ReflectedCollectionEditInterface<T> : EditInterface
    {
        private Dictionary<T, EditableProperty> items = new Dictionary<T, EditableProperty>();
        private MemberScanner memberScanner;
        private String name;
        private EditablePropertyInfo propertyInfo = new EditablePropertyInfo();
        private DestroyEditInterfaceCommand destroyInterfaceCommand = null;
        private CreateEditablePropertyCommand createPropertyCommand = null;
        private DestroyEditablePropertyCommand destroyPropertyCommand = null;
        private Object commandTarget = null;

        public ReflectedCollectionEditInterface(String name, IEnumerable<T> collection)
            : this(name, collection, new MemberScanner(EditableAttributeFilter.Instance))
        {

        }

        public ReflectedCollectionEditInterface(String name, IEnumerable<T> collection, MemberScanner scanner)
        {
            this.memberScanner = scanner;
            this.name = name;
            LinkedList<MemberWrapper> matches = memberScanner.getMatchingMembers(typeof(T));
            foreach (MemberWrapper wrapper in matches)
            {
                if (ReflectedVariable.canCreateVariable(wrapper.getWrappedType()))
                {
                    propertyInfo.addColumn(new EditablePropertyColumn(wrapper.getWrappedName(), false));
                }
            }
            foreach (T item in collection)
            {
                addItem(item);
            }
        }

        #region Functions

        public ReflectedObjectEditableProperty addItem(T item)
        {
            ReflectedObjectEditableProperty prop = new ReflectedObjectEditableProperty(item, memberScanner);
            items.Add(item, prop);
            return prop;
        }

        public void removeItem(T item)
        {
            items.Remove(item);
        }

        public string getName()
        {
            return name;
        }

        public bool hasEditableProperties()
        {
            return true;
        }

        public IEnumerable<EditableProperty> getEditableProperties()
        {
            return items.Values;
        }

        public EditablePropertyInfo getPropertyInfo()
        {
            return propertyInfo;
        }

        public bool hasSubEditInterfaces()
        {
            return false;
        }

        public IEnumerable<EditInterface> getSubEditInterfaces()
        {
            return null;
        }

        public bool hasCreateSubObjectCommands()
        {
            return false;
        }

        public IEnumerable<CreateEditInterfaceCommand> getCreateSubObjectCommands()
        {
            return null;
        }

        public bool hasDestroyObjectCommand()
        {
            return destroyInterfaceCommand != null;
        }

        public DestroyEditInterfaceCommand getDestroyObjectCommand()
        {
            return destroyInterfaceCommand;
        }

        public void setDestroyObjectCommand(DestroyEditInterfaceCommand command)
        {
            this.destroyInterfaceCommand = command;
        }

        public bool canAddRemoveProperties()
        {
            return createPropertyCommand != null && destroyPropertyCommand != null;
        }

        public CreateEditablePropertyCommand getCreatePropertyCommand()
        {
            return createPropertyCommand;
        }

        public void setCreatePropertyCommand(CreateEditablePropertyCommand command)
        {
            this.createPropertyCommand = command;
        }

        public DestroyEditablePropertyCommand getDestroyPropertyCommand()
        {
            return destroyPropertyCommand;
        }

        public void setDestroyPropertyCommand(DestroyEditablePropertyCommand command)
        {
            this.destroyPropertyCommand = command;
        }

        public object getCommandTargetObject()
        {
            return commandTarget;
        }

        public void setCommandTarget(Object target)
        {
            this.commandTarget = target;
        }

        #endregion
    }
}
