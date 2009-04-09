using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine.Editing
{
    /// <summary>
    /// This class will provide an EditInterface to a collection of objects.
    /// These objects can be placed in any type of collection as this class does
    /// not direcly handle the adding/removing of objects, but instead defers
    /// these operations to CreateEditablePropertyCommands and
    /// DestroyEditablePropertyCommands.
    /// </summary>
    /// <typeparam name="T">The type in the collection this interface wraps. Not the type of the collection itself.</typeparam>
    class ReflectedCollectionEditInterface<T> : EditInterface
    {
        #region Functions

        private Dictionary<T, EditableProperty> items = new Dictionary<T, EditableProperty>();
        private MemberScanner memberScanner;
        private String name;
        private EditablePropertyInfo propertyInfo = new EditablePropertyInfo();
        private DestroyEditInterfaceCommand destroyInterfaceCommand = null;
        private CreateEditablePropertyCommand createPropertyCommand = null;
        private DestroyEditablePropertyCommand destroyPropertyCommand = null;
        private Object commandTarget = null;

        #endregion Functions

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">A name for this interface.</param>
        /// <param name="collection">The collection that has the elements this interface will hold.</param>
        public ReflectedCollectionEditInterface(String name, IEnumerable<T> collection)
            : this(name, collection, new MemberScanner(EditableAttributeFilter.Instance))
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">A name for this interface.</param>
        /// <param name="collection">The collection that has the elements this interface will hold.</param>
        /// <param name="scanner">A MemberScanner to discover the properties of T with.</param>
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

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Add an item to the collection. This will scan the item with a
        /// ReflectedObjectEditableProperty and return that property. It does
        /// not actually add the given item to the wrapped collection that must
        /// be done by the caller.
        /// </summary>
        /// <param name="item">The item to add an property for.</param>
        /// <returns>A new ReflectedObjectEditableProperty for item.</returns>
        public ReflectedObjectEditableProperty addItem(T item)
        {
            ReflectedObjectEditableProperty prop = new ReflectedObjectEditableProperty(item, memberScanner);
            items.Add(item, prop);
            return prop;
        }

        /// <summary>
        /// Removes an item's property from the collection. Note this does not
        /// actually remove the item from the wrapped collection that is up to
        /// the caller.
        /// </summary>
        /// <param name="item">The item to remove the property for.</param>
        public void removeItem(T item)
        {
            items.Remove(item);
        }

        /// <summary>
        /// Get a name for this interface.
        /// </summary>
        /// <returns>A String with the name of the interface.</returns>
        public string getName()
        {
            return name;
        }

        /// <summary>
        /// Determine if this EditInterface has any EditableProperties.
        /// </summary>
        /// <returns>True if the interface has some EditableProperties.</returns>
        public bool hasEditableProperties()
        {
            return true;
        }

        /// <summary>
        /// This function will return all properties of an EditInterface.
        /// </summary>
        /// <returns>A enumerable over all properties in the EditInterface or null if there aren't any.</returns>
        public IEnumerable<EditableProperty> getEditableProperties()
        {
            return items.Values;
        }

        /// <summary>
        /// Return the EditablePropertyInfo for this interface that determines
        /// the layout of a single property. This can be null if
        /// hasEditableProperties is false.
        /// </summary>
        /// <returns>The EditablePropertyInfo for this interface.</returns>
        public EditablePropertyInfo getPropertyInfo()
        {
            return propertyInfo;
        }

        /// <summary>
        /// Determine if this EditInterface has any SubEditInterfaces.
        /// </summary>
        /// <returns>True if the interface has some SubEditInterfaces.</returns>
        public bool hasSubEditInterfaces()
        {
            return false;
        }

        /// <summary>
        /// Get any SubEditInterfaces in this interface.
        /// </summary>
        /// <returns>An enumerable over all EditInterfaces that are part of this EditInterface or null if there aren't any.</returns>
        public IEnumerable<EditInterface> getSubEditInterfaces()
        {
            return null;
        }

        /// <summary>
        /// Determine if this EditInterface has any CreateSubObjectCommands.
        /// </summary>
        /// <returns>True if there are create commands.</returns>
        public bool hasCreateSubObjectCommands()
        {
            return false;
        }

        /// <summary>
        /// Get a list of commands for creating sub objects.
        /// </summary>
        /// <returns>An IEnumerable over all creation commands or null if there aren't any.</returns>
        public IEnumerable<CreateEditInterfaceCommand> getCreateSubObjectCommands()
        {
            return null;
        }

        /// <summary>
        /// Determine if this interface has a command to destroy itself.
        /// </summary>
        /// <returns>True if there is a destroy command.</returns>
        public bool hasDestroyObjectCommand()
        {
            return destroyInterfaceCommand != null;
        }

        /// <summary>
        /// Get a command that will destroy this object. This command must
        /// accept a single argument that is a EditUICallback. This is optional
        /// and can be null.
        /// </summary>
        /// <returns>A command that will destroy this EditInterface object or null if it cannot be destroyed.</returns>
        public DestroyEditInterfaceCommand getDestroyObjectCommand()
        {
            return destroyInterfaceCommand;
        }

        /// <summary>
        /// Se the command to destroy this object if needed.
        /// </summary>
        /// <param name="command">The command to set.</param>
        public void setDestroyObjectCommand(DestroyEditInterfaceCommand command)
        {
            this.destroyInterfaceCommand = command;
        }


        /// <summary>
        /// Determine if this interface can create and destroy properties. If
        /// this returns true both getCreatePropertyCommand and
        /// getDestroyPropertyCommand must be implemented.
        /// </summary>
        /// <returns>True if this interface can create and destroy properties.</returns>
        public bool canAddRemoveProperties()
        {
            return createPropertyCommand != null && destroyPropertyCommand != null;
        }

        /// <summary>
        /// Get the command that creates new properties.
        /// </summary>
        /// <returns>A CreateEditablePropertyCommand to create properties or null if it does not have one.</returns>
        public CreateEditablePropertyCommand getCreatePropertyCommand()
        {
            return createPropertyCommand;
        }

        /// <summary>
        /// Set the command used for creating objects. Both this and the
        /// DestroyObjectCommand must be set before items can be created.
        /// </summary>
        /// <param name="command">The command to set.</param>
        public void setCreatePropertyCommand(CreateEditablePropertyCommand command)
        {
            this.createPropertyCommand = command;
        }

        /// <summary>
        /// Get the command that destroys properties.
        /// </summary>
        /// <returns>A DestroyEditablePropertyCommand to destroy properties or null if it does not have one.</returns>
        public DestroyEditablePropertyCommand getDestroyPropertyCommand()
        {
            return destroyPropertyCommand;
        }

        /// <summary>
        /// Set the command used for destroying objects. Both this and the
        /// CreateObjectCommand must be set before items can be created.
        /// </summary>
        /// <param name="command">The command to set.</param>
        public void setDestroyPropertyCommand(DestroyEditablePropertyCommand command)
        {
            this.destroyPropertyCommand = command;
        }

        /// <summary>
        /// Set the object that is the target for commands.
        /// </summary>
        /// <param name="target">The object that will be passed as the target object to the commands.</param>
        public void setCommandTarget(Object target)
        {
            this.commandTarget = target;
        }

        #endregion
    }
}
