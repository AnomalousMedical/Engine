using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    public delegate void AddProperty(EditUICallback callback);
    public delegate void RemoveProperty(EditUICallback callback, EditableProperty property);
    public delegate bool Validate(out String errorMessage);

    public delegate void PropertyAdded(EditableProperty property);
    public delegate void PropertyRemoved(EditableProperty property);
    public delegate void SubInterfaceAdded(EditInterface editInterface);
    public delegate void SubInterfaceRemoved(EditInterface editInterface);

    /// <summary>
    /// This interface provides a view of an object that can be edited.
    /// </summary>
    public sealed class EditInterface
    {
        private String name;
        private AddProperty addPropertyCallback;
        private RemoveProperty removePropertyCallback;
        private Validate validateCallback;
        private LinkedList<EditableProperty> editableProperties = new LinkedList<EditableProperty>();
        private LinkedList<EditInterface> subInterfaces = new LinkedList<EditInterface>();
        private EditablePropertyInfo propertyInfo;
        private LinkedList<EditInterfaceCommand> commands = new LinkedList<EditInterfaceCommand>();

        public event PropertyAdded OnPropertyAdded;
        public event PropertyRemoved OnPropertyRemoved;
        public event SubInterfaceAdded OnSubInterfaceAdded;
        public event SubInterfaceRemoved OnSubInterfaceRemoved;

        public EditInterface(String name)
            :this(name, null, null, null)
        {

        }

        public EditInterface(String name, Validate validateCallback)
            :this(name, null, null, validateCallback)
        {

        }

        public EditInterface(String name, AddProperty addPropertyCallback, RemoveProperty removePropertyCallback)
            :this(name, addPropertyCallback, removePropertyCallback, null)
        {

        }

        public EditInterface(String name, AddProperty addPropertyCallback, RemoveProperty removePropertyCallback, Validate validateCallback)
        {
            this.name = name;
            this.addPropertyCallback = addPropertyCallback;
            this.removePropertyCallback = removePropertyCallback;
            this.validateCallback = validateCallback;
        }

        /// <summary>
        /// Get a name for this interface.
        /// </summary>
        /// <returns>A String with the name of the interface.</returns>
        public String getName()
        {
            return name;
        }

        public void addEditableProperty(EditableProperty property)
        {
            editableProperties.AddLast(property);
            if (OnPropertyAdded != null)
            {
                OnPropertyAdded.Invoke(property);
            }
        }

        public void removeEditableProperty(EditableProperty property)
        {
            editableProperties.Remove(property);
            if (OnPropertyRemoved != null)
            {
                OnPropertyRemoved.Invoke(property);
            }
        }

        /// <summary>
        /// Determine if this EditInterface has any EditableProperties.
        /// </summary>
        /// <returns>True if the interface has some EditableProperties.</returns>
        public bool hasEditableProperties()
        {
            return editableProperties.Count != 0;
        }

        /// <summary>
        /// This function will return all properties of an EditInterface.
        /// </summary>
        /// <returns>A enumerable over all properties in the EditInterface or null if there aren't any.</returns>
        public IEnumerable<EditableProperty> getEditableProperties()
        {
            return editableProperties;
        }

        public void setPropertyInfo(EditablePropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
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

        public void addSubInterface(EditInterface editInterface)
        {
            subInterfaces.AddLast(editInterface);
            if (OnSubInterfaceAdded != null)
            {
                OnSubInterfaceAdded.Invoke(editInterface);
            }
        }

        public void removeSubInterface(EditInterface editInterface)
        {
            subInterfaces.Remove(editInterface);
            if (OnSubInterfaceRemoved != null)
            {
                OnSubInterfaceRemoved.Invoke(editInterface);
            }
        }

        /// <summary>
        /// Determine if this EditInterface has any SubEditInterfaces.
        /// </summary>
        /// <returns>True if the interface has some SubEditInterfaces.</returns>
        public bool hasSubEditInterfaces()
        {
            return subInterfaces.Count != 0;
        }

        /// <summary>
        /// Get any SubEditInterfaces in this interface.
        /// </summary>
        /// <returns>An enumerable over all EditInterfaces that are part of this EditInterface or null if there aren't any.</returns>
        public IEnumerable<EditInterface> getSubEditInterfaces()
        {
            return subInterfaces;
        }

        public void addCommand(EditInterfaceCommand command)
        {
            commands.AddLast(command);
        }

        public void removeCommand(EditInterfaceCommand command)
        {
            commands.Remove(command);
        }

        /// <summary>
        /// Does this EditInterface have commands.
        /// </summary>
        /// <returns>True if this EditInterface has some commands.</returns>
        public bool hasCommands()
        {
            return commands.Count != 0;
        }

        /// <summary>
        /// Get the list of commands.
        /// </summary>
        /// <returns>An IEnumerable over all commands.</returns>
        public IEnumerable<EditInterfaceCommand> getCommands()
        {
            return commands;
        }

        /// <summary>
        /// Determine if this EditInterface can add and remove properties.
        /// </summary>
        /// <returns>True if this interface can add and remove properties.</returns>
        public bool canAddRemoveProperties()
        {
            return addPropertyCallback != null && removePropertyCallback != null;
        }

        /// <summary>
        /// Get the add property callback.
        /// </summary>
        /// <returns>The AddProperty callback.</returns>
        public AddProperty getAddPropertyCallback()
        {
            return addPropertyCallback;
        }

        /// <summary>
        /// Get the remove property callback.
        /// </summary>
        /// <returns>The RemoveProperty callback.</returns>
        public RemoveProperty getRemovePropertyCallback()
        {
            return removePropertyCallback;
        }

        /// <summary>
        /// This function will validate the data in the EditInterface and return
        /// true if it is valid. It will also fill out errorMessage with any
        /// errors that may occur.
        /// </summary>
        /// <param name="errorMessage">A string that will get an error message for the interface.</param>
        /// <returns>True if the settings are valid, false if they are not.</returns>
        public bool validate(out String errorMessage)
        {
            if (validateCallback != null)
            {
                return validateCallback.Invoke(out errorMessage);
            }
            errorMessage = null;
            return true;
        }

        /// <summary>
        /// Internal object for binding this EditInterface to its source object.
        /// Only used by EditInterfaceManager.
        /// </summary>
        internal Object ManagerBinding { get; set; }
    }
}
