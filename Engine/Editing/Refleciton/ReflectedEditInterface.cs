using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;
using System.Reflection;

namespace Engine.Editing
{
    /// <summary>
    /// This is an implementation of the EditInterface that uses reflection to
    /// automatically discover properties.
    /// </summary>
    public class ReflectedEditInterface : EditInterface
    {
        #region Fields

        private MemberScanner memberScanner;
        private Object target;
        private Type targetType;
        private LinkedList<EditableProperty> properties = new LinkedList<EditableProperty>();
        private LinkedList<EditInterface> interfaces = new LinkedList<EditInterface>();
        private LinkedList<CreateEditInterfaceCommand> createCommands = null;
        private DestroyEditInterfaceCommand destroyCommand = null;
        private String name;
        private EditablePropertyInfo propertyInfo = new EditablePropertyInfo();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor. Will scan all public and nonpublic fields and
        /// properties of the given type and create an interface from that
        /// information.
        /// </summary>
        /// <param name="target">The object to scan and create an interface for.</param>
        /// <param name="name">The name of the object.</param>
        public ReflectedEditInterface(Object target, String name)
            :this(target, name, target.GetType().Name)
        {

        }

        /// <summary>
        /// Constructor. Will scan all public and nonpublic fields and
        /// properties of the given type and create an interface from that
        /// information.
        /// </summary>
        /// <param name="target">The object to scan and create an interface for.</param>
        /// <param name="name">The name of the object.</param>
        /// <param name="objectName">The name of the type for the object.</param>
        public ReflectedEditInterface(Object target, String name, String objectName)
            : this(target, new MemberScanner(), name, objectName)
        {
            memberScanner.Filter = EditableAttributeFilter.Instance;
        }

        /// <summary>
        /// Constructor. Will scan the object using the given MemberScanner and
        /// create an interface from that information.
        /// </summary>
        /// <param name="target">The object to scan and create an interface for.</param>
        /// <param name="scanner">A customized MemberScanner that will find only what is desired.</param>
        public ReflectedEditInterface(Object target, MemberScanner scanner, String name)
            :this(target, scanner, name, target.GetType().Name)
        {

        }

        /// <summary>
        /// Constructor. Will scan the object using the given MemberScanner and
        /// create an interface from that information.
        /// </summary>
        /// <param name="target">The object to scan and create an interface for.</param>
        /// <param name="scanner">A customized MemberScanner that will find only what is desired.</param>
        /// <param name="name">The name of the object.</param>
        /// <param name="objectName">The name of the type for the object.</param>
        public ReflectedEditInterface(Object target, MemberScanner scanner, String name, String objectName)
        {
            memberScanner = scanner;
            this.target = target;
            targetType = target.GetType();
            if (name != null)
            {
                this.name = name + " - " + objectName;
            }
            else
            {
                name = objectName;
            }
            propertyInfo.addColumn(new EditablePropertyColumn("Name", true));
            propertyInfo.addColumn(new EditablePropertyColumn("Value", false));
            buildInterface();
        }

        #endregion Constructors

        #region Functions

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
            return properties.Count != 0;
        }

        /// <summary>
        /// This function will return all properties of an EditInterface.
        /// </summary>
        /// <returns>A enumerable over all properties in the EditInterface or null if there aren't any.</returns>
        public IEnumerable<EditableProperty> getEditableProperties()
        {
            return properties;
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
        /// Add a property manually to this interface.
        /// </summary>
        /// <param name="manualProperty">The property to add.</param>
        public void addManualProperty(EditableProperty manualProperty)
        {
            properties.AddLast(manualProperty);
        }

        /// <summary>
        /// Determine if this EditInterface has any SubEditInterfaces.
        /// </summary>
        /// <returns>True if the interface has some SubEditInterfaces.</returns>
        public bool hasSubEditInterfaces()
        {
            return interfaces.Count != 0;
        }

        /// <summary>
        /// Get any SubEditInterfaces in this interface.
        /// </summary>
        /// <returns>An enumerable over all EditInterfaces that are part of this EditInterface or null if there aren't any.</returns>
        public IEnumerable<EditInterface> getSubEditInterfaces()
        {
            return interfaces;
        }

        /// <summary>
        /// Add a SubInterface manually to this interface.
        /// </summary>
        /// <param name="manualInterface">The interface to add.</param>
        public void addManualSubInterface(EditInterface manualInterface)
        {
            interfaces.AddLast(manualInterface);
        }

        /// <summary>
        /// Determine if this EditInterface has any CreateSubObjectCommands.
        /// </summary>
        /// <returns>True if there are create commands.</returns>
        public bool hasCreateSubObjectCommands()
        {
            return createCommands != null && createCommands.Count != 0;
        }


        /// <summary>
        /// Get a list of commands for creating sub objects. These commands must
        /// accept a single argument that is a EditUICallback and return an
        /// EditInterface for the newly created object if it can be edited. This
        /// is optional and can be null.
        /// </summary>
        /// <returns>An IEnumerable over all creation commands or null if there aren't any.</returns>
        public IEnumerable<CreateEditInterfaceCommand> getCreateSubObjectCommands()
        {
            return createCommands;
        }

        /// <summary>
        /// Add a EngineCommand to create sub objects.
        /// </summary>
        /// <param name="command">The command to add.</param>
        public void addCreateSubObjectCommand(CreateEditInterfaceCommand command)
        {
            if (createCommands == null)
            {
                createCommands = new LinkedList<CreateEditInterfaceCommand>();
            }
            createCommands.AddLast(command);
        }

        /// <summary>
        /// Determine if this interface has a command to destroy itself.
        /// </summary>
        /// <returns>True if there is a destroy command.</returns>
        public bool hasDestroyObjectCommand()
        {
            return destroyCommand != null;
        }


        /// <summary>
        /// Get a command that will destroy this object. This command must
        /// accept a single argument that is a EditUICallback. This is optional
        /// and can be null.
        /// </summary>
        /// <returns>A command that will destroy this EditInterface object or null if it cannot be destroyed.</returns>
        public DestroyEditInterfaceCommand getDestroyObjectCommand()
        {
            return destroyCommand;
        }

        /// <summary>
        /// Set the destroy command for this object.
        /// </summary>
        /// <param name="destroyCommand">The destroy command to run to remove this object.</param>
        public void setDestroyCommand(DestroyEditInterfaceCommand destroyCommand)
        {
            this.destroyCommand = destroyCommand;
        }


        /// <summary>
        /// Get the object that will be sent as the target to the create and
        /// destroy commands. This is the target object for the ReflectedEditInterface.
        /// </summary>
        /// <returns>The object that will be sent as the target to the create and destroy commands.</returns>
        public object getCommandTargetObject()
        {
            return target;
        }

        /// <summary>
        /// Determine if this interface can create properties.
        /// </summary>
        /// <returns>True if this interface can create properties.</returns>
        public bool hasCreatePropertyCommand()
        {
            return false;
        }

        /// <summary>
        /// Get the command that creates new properties.
        /// </summary>
        /// <returns>A CreateEditablePropertyCommand to create properties or null if it does not have one.</returns>
        public CreateEditablePropertyCommand getCreatePropertyCommand()
        {
            return null;
        }

        /// <summary>
        /// Determine if this interface can destroy properties.
        /// </summary>
        /// <returns>True if this interface can destroy properties.</returns>
        public bool hasDestroyPropertyCommand()
        {
            return false;
        }

        /// <summary>
        /// Get the command that destroys properties.
        /// </summary>
        /// <returns>A DestroyEditablePropertyCommand to destroy properties or null if it does not have one.</returns>
        public DestroyEditablePropertyCommand getDestroyPropertyCommand()
        {
            return null;
        }

        #endregion Functions

        #region Helper Functions

        /// <summary>
        /// Build the EditInterface for the given type.
        /// </summary>
        private void buildInterface()
        {
            LinkedList<MemberWrapper> members = memberScanner.getMatchingMembers(targetType);
            foreach (MemberWrapper memberWrapper in members)
            {
                if (ReflectedVariable.canCreateVariable(memberWrapper.getWrappedType()))
                {
                    properties.AddLast(new ReflectedEditableProperty(memberWrapper.getWrappedName(), ReflectedVariable.createVariable(memberWrapper, target)));
                }
                else
                {
                    Object subObject = memberWrapper.getValue(target, null);
                    interfaces.AddLast(new ReflectedEditInterface(subObject, null));
                }
            }
        }

        #endregion Helper Functions
    }
}
