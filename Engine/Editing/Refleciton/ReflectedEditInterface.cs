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
        private LinkedList<EngineCommand> createCommands = null;
        private EngineCommand destroyCommand = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor. Will scan all public and nonpublic fields and
        /// properties of the given type and create an interface from that
        /// information.
        /// </summary>
        /// <param name="target">The object to scan and create an interface for.</param>
        public ReflectedEditInterface(Object target)
            :this(target, new MemberScanner())
        {
            memberScanner.Filter = EditableAttributeFilter.Instance;
        }

        /// <summary>
        /// Constructor. Will scan the object using the given MemberScanner and
        /// create an interface from that information.
        /// </summary>
        /// <param name="target">The object to scan and create an interface for.</param>
        /// <param name="scanner">A customized MemberScanner that will find only what is desired.</param>
        public ReflectedEditInterface(Object target, MemberScanner scanner)
        {
            memberScanner = scanner;
            this.target = target;
            targetType = target.GetType();
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
            return targetType.Name;
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
        /// accept no arguments and return an EditInterface for the newly
        /// created object if it can be edited.
        /// </summary>
        /// <returns>An IEnumerable over all creation commands or null if there aren't any.</returns>
        public IEnumerable<EngineCommand> getCreateSubObjectCommands()
        {
            return createCommands;
        }

        /// <summary>
        /// Add a EngineCommand to create sub objects.
        /// </summary>
        /// <param name="command">The command to add.</param>
        public void addCreateSubObjectCommand(EngineCommand command)
        {
            if (createCommands == null)
            {
                createCommands = new LinkedList<EngineCommand>();
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
        /// accept no arguments. This is optional and can be null.
        /// </summary>
        /// <returns>A command that will destroy this EditInterface object or null if it cannot be destroyed.</returns>
        public EngineCommand getDestroyObjectCommand()
        {
            return destroyCommand;
        }

        /// <summary>
        /// Set the destroy command for this object.
        /// </summary>
        /// <param name="destroyCommand">The destroy command to run to remove this object.</param>
        public void setDestroyCommand(EngineCommand destroyCommand)
        {
            this.destroyCommand = destroyCommand;
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
                    interfaces.AddLast(new ReflectedEditInterface(subObject));
                }
            }
        }

        #endregion Helper Functions
    }
}
