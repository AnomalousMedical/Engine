using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace Engine.Editing
{
    public delegate void AddProperty(EditUICallback callback);
    public delegate void RemoveProperty(EditUICallback callback, EditableProperty property);
    public delegate bool Validate(out String errorMessage);

    public delegate void PropertyAdded(EditableProperty property);
    public delegate void PropertyRemoved(EditableProperty property);
    public delegate void SubInterfaceAdded(EditInterface editInterface);
    public delegate void SubInterfaceRemoved(EditInterface editInterface);
    public delegate void ColorsChanged(EditInterface editInterface);
    public delegate void EditInterfaceEvent(EditInterface editInterface); //Generalized delegate, use this from now on.

    public enum EngineIcons
    {
        Node,
        Entity,
        Light,
        ManualObject,
        Camera,
        RigidBody,
        Joint,
        SimObject,
        Behavior,
        Scene,
        Resources
    }

    /// <summary>
    /// This interface provides a view of an object that can be edited. It is extremely unlikely that
    /// you would need to inherit from this class, instead you use callbacks to modify its data.
    /// </summary>
    [DoNotCopy]
    [DoNotSave]
    public class EditInterface
    {
        private String name;
        protected AddProperty addPropertyCallback;
        protected RemoveProperty removePropertyCallback;
        protected Validate validateCallback;
        private LinkedList<EditableProperty> editableProperties = new LinkedList<EditableProperty>();
        private LinkedList<EditInterface> subInterfaces = new LinkedList<EditInterface>();
        private EditablePropertyInfo propertyInfo;
        private LinkedList<EditInterfaceCommand> commands = new LinkedList<EditInterfaceCommand>();
        private Color backColor = Color.White;
        private Color foreColor = Color.Black;
        private Object iconReferenceTag = null;

        private EditablePropertyManager editablePropertyManager; //Handles keyed EditableProperties
        private Dictionary<Type, Object> editInterfaceManagers; //Handles EditInterfaceManagers, the values are Objects since we have to typecast to this generic class anyway.
        private Dictionary<Object, EditInterface> objectSubInterfaces; //Handles sub EditInterfaces added with an object, these are also put into the subInterfaces list.

        public event PropertyAdded OnPropertyAdded;
        public event PropertyRemoved OnPropertyRemoved;
        public event SubInterfaceAdded OnSubInterfaceAdded;
        public event SubInterfaceRemoved OnSubInterfaceRemoved;
        public event ColorsChanged OnBackColorChanged;
        public event ColorsChanged OnForeColorChanged;
        public event EditInterfaceEvent OnIconReferenceChanged;
        public event EditInterfaceEvent OnNameChanged;
        public event EditInterfaceEvent OnDataNeedsRefresh;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the interface.</param>
        public EditInterface(String name)
            :this(name, null, null, null)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the interface.</param>
        /// <param name="validateCallback">A callback to be called when the data should be validated.</param>
        public EditInterface(String name, Validate validateCallback)
            :this(name, null, null, validateCallback)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the interface.</param>
        /// <param name="addPropertyCallback">A callback to be called when a property is added.</param>
        /// <param name="removePropertyCallback">A callback to be called when a property is removed.</param>
        public EditInterface(String name, AddProperty addPropertyCallback, RemoveProperty removePropertyCallback)
            :this(name, addPropertyCallback, removePropertyCallback, null)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the interface.</param>
        /// <param name="addPropertyCallback">A callback to be called when a property is added.</param>
        /// <param name="removePropertyCallback">A callback to be called when a property is removed.</param>
        /// <param name="validateCallback">A callback to be called when the data should be validated.</param>
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

        /// <summary>
        /// Change the name of this EditInterface.
        /// </summary>
        /// <param name="name"></param>
        public void setName(String name)
        {
            if (this.name != name)
            {
                this.name = name;
                if (OnNameChanged != null)
                {
                    OnNameChanged.Invoke(this);
                }
            }
        }

        /// <summary>
        /// Add a property. This version has no key, you should not mix the key with non key versions.
        /// </summary>
        /// <param name="property">The property to add.</param>
        public void addEditableProperty(EditableProperty property)
        {
            editableProperties.AddLast(property);
            if (OnPropertyAdded != null)
            {
                OnPropertyAdded.Invoke(property);
            }
        }

        /// <summary>
        /// Add a property with an object key. You can use this key object to remove the property later if needed.
        /// </summary>
        /// <param name="key">The key of the property.</param>
        /// <param name="property">The EditableProperty for that key.</param>
        public void addEditableProperty(Object key, EditableProperty property)
        {
            if(editablePropertyManager == null)
            {
                editablePropertyManager = new EditablePropertyManager(this);
            }
            editablePropertyManager.addProperty(key, property);
        }

        /// <summary>
        /// Remove a property. This version has no key, you should not mix the key with non key versions.
        /// </summary>
        /// <param name="property">The property to remove.</param>
        public void removeEditableProperty(EditableProperty property)
        {
            editableProperties.Remove(property);
            if (OnPropertyRemoved != null)
            {
                OnPropertyRemoved.Invoke(property);
            }
        }

        /// <summary>
        /// Remove an EditableProperty based on a key.
        /// </summary>
        /// <param name="key"></param>
        public void removeEditableProperty(Object key)
        {
            if(editablePropertyManager != null)
            {
                editablePropertyManager.removeProperty(key);
            }
        }

        /// <summary>
        /// Reverse lookup of the original key object from the EditableProperty.
        /// Will throw an exception if the key cannot be typecast, the property cannot be found
        /// or no keyed properties have been added.
        /// </summary>
        /// <param name="property">The property associated with the key.</param>
        /// <returns>The key object that was used to set the property.</returns>
        public T getKeyObjectForProperty<T>(EditableProperty property)
        {
            if(editablePropertyManager != null)
            {
                return (T)editablePropertyManager[property];
            }
            throw new EditException("No properties added with keys.");
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

        /// <summary>
        /// Get the EditableProperty at the given index, EditableProperty indexes are in the order they are discovered
        /// by their scanner.
        /// </summary>
        /// <param name="index">The index of the property.</param>
        /// <returns>The property at the given index.</returns>
        public EditableProperty getEditablePropertyAt(int index)
        {
            return editableProperties.Skip(index).First();
        }

        /// <summary>
        /// Set the EditablePropertyInfo.
        /// </summary>
        /// <param name="propertyInfo">The info to set.</param>
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

        /// <summary>
        /// Add an EditInterface below this one.
        /// </summary>
        /// <param name="editInterface">The subinterface to add.</param>
        public void addSubInterface(EditInterface editInterface)
        {
            subInterfaces.AddLast(editInterface);
            editInterface.ParentEditInterface = this;
            if (OnSubInterfaceAdded != null)
            {
                OnSubInterfaceAdded.Invoke(editInterface);
            }
        }

        /// <summary>
        /// Add a SubEditInteface for a particluar object. This is most useful to bind to say a list
        /// instance or other value that will not change inside the client object, but the contents might
        /// that you might need to refresh. You could also use it to key a SubEditInterface to an object.
        /// </summary>
        /// <param name="fieldObject">The field's current value.</param>
        /// <param name="editInterface">The EditInteface to bind to that fields value.</param>
        public void addSubInterfaceForObject(Object fieldObject, EditInterface editInterface)
        {
            if(objectSubInterfaces == null)
            {
                objectSubInterfaces = new Dictionary<object, EditInterface>();
            }
            objectSubInterfaces.Add(fieldObject, editInterface);
            addSubInterface(editInterface);
        }

        /// <summary>
        /// Remove a sub interface.
        /// </summary>
        /// <param name="editInterface">The subinterface to remove.</param>
        public void removeSubInterface(EditInterface editInterface)
        {
            if (subInterfaces.Remove(editInterface))
            {
                if (objectSubInterfaces != null)
                {
                    //Reverse lookup editInterface in case it is part of the objectSubInterfaces.
                    KeyValuePair<Object, EditInterface> fieldRef = objectSubInterfaces.FirstOrDefault(i => i.Value == editInterface);
                    if (fieldRef.Key != null)
                    {
                        objectSubInterfaces.Remove(fieldRef.Key);
                    }
                }
                editInterface.ParentEditInterface = null;
                if (OnSubInterfaceRemoved != null)
                {
                    OnSubInterfaceRemoved.Invoke(editInterface);
                }
            }
        }

        /// <summary>
        /// Get the edit interface for a given key, if the key does not have an associated EditInterface null
        /// will be returned.
        /// </summary>
        /// <param name="key">The key to lookup.</param>
        /// <returns>The associated EditInterface or null if there isn't one.</returns>
        public EditInterface getEditInterfaceFor(Object key)
        {
            EditInterface ret = null;
            if(objectSubInterfaces != null)
            {
                objectSubInterfaces.TryGetValue(key, out ret);
            }
            return ret;
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

        /// <summary>
        /// Create an EditInterfaceManager that handles T objects.
        /// </summary>
        /// <typeparam name="T">The type the EditInterfaceManager handles.</typeparam>
        public EditInterfaceManager<T> createEditInterfaceManager<T>()
            where T : class
        {
            if(editInterfaceManagers == null)
            {
                editInterfaceManagers = new Dictionary<Type, object>();
            }
            var editInterfaceManager = new EditInterfaceManager<T>(this);
            editInterfaceManagers.Add(typeof(T), editInterfaceManager);
            return editInterfaceManager;
        }

        /// <summary>
        /// Add a keyed sub edit interface. You must define a EditInterfaceManager for type T before calling this function
        /// or it will throw an exception.
        /// </summary>
        /// <typeparam name="T">The type of the key.</typeparam>
        /// <param name="key">The key value.</param>
        /// <param name="subInterface">The subInterface to add.</param>
        public void addSubInterface<T>(T key, EditInterface subInterface)
            where T : class
        {
            getEditInterfaceManager<T>().addSubInterface(key, subInterface);
        }

        /// <summary>
        /// Remove a keyed sub edit interface. You must define a EditInterfaceManager for type T before calling this function
        /// or it will throw an exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public void removeSubInterface<T>(T key)
            where T : class
        {
            getEditInterfaceManager<T>().removeSubInterface(key);
        }

        /// <summary>
        /// Find the key object for the given sub edit interface. You must define a EditInterfaceManager for type T before calling this function
        /// or it will throw an exception.
        /// </summary>
        /// <typeparam name="T">The type of the key object. Must match the type defined for the EditInterfaceManager.</typeparam>
        /// <param name="subInterface">The EditInterface to lookup.</param>
        /// <returns></returns>
        public T resolveSourceObject<T>(EditInterface subInterface)
            where T : class
        {
            return getEditInterfaceManager<T>().resolveSourceObject(subInterface);
        }

        /// <summary>
        /// Find the EditInterfaceManager for T. You must define a EditInterfaceManager for type T before calling this function
        /// or it will throw an exception.
        /// </summary>
        /// <typeparam name="T">The key type for the EditInterfaceManager.</typeparam>
        /// <returns>The EditInterfaceManager for T types.</returns>
        public EditInterfaceManager<T> getEditInterfaceManager<T>()
            where T : class
        {
            return ((EditInterfaceManager<T>)editInterfaceManagers[typeof(T)]);
        }

        /// <summary>
        /// Add a command.
        /// </summary>
        /// <param name="command">The command to add.</param>
        public void addCommand(EditInterfaceCommand command)
        {
            commands.AddLast(command);
        }

        /// <summary>
        /// Remove a command.
        /// </summary>
        /// <param name="command">The command to remove.</param>
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
        /// Clear the list of commands.
        /// </summary>
        public void clearCommands()
        {
            commands.Clear();
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
        /// Fire the add property callback from the ui to the object.
        /// </summary>
        public void fireAddPropertyCallback(EditUICallback uiCallback)
        {
            if (addPropertyCallback != null)
            {
                addPropertyCallback.Invoke(uiCallback);
            }
        }

        /// <summary>
        /// Fire the remove property callback from the ui to the object.
        /// </summary>
        public void fireRemovePropertyCallback(EditUICallback uiCallback, EditableProperty property)
        {
            if (removePropertyCallback != null)
            {
                removePropertyCallback.Invoke(uiCallback, property);
            }
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
        /// This function should be called if this EditInterface wraps an object like a list
        /// who's data contents have changed. This will force the EditInterface to refresh
        /// its contents. This is not needed for default EditInterface instances because they
        /// will keep their contents up to date automatically, but if you are using one of the
        /// ListLikeEditInterfaces then you should call this funciton when the associated
        /// list changes.
        /// </summary>
        public virtual void dataContentsChanged()
        {
            //Does nothing in normal edit interfaces.
        }

        /// <summary>
        /// This method will fire the OnDataNeedsRefresh, however it should be
        /// called by the extension method with the same name but no leading _.
        /// This has the benefit of checking the editInterface for null before
        /// firing the event so client code does not have to.
        /// </summary>
        internal void _fireDataNeedsRefresh()
        {
            if (OnDataNeedsRefresh != null)
            {
                OnDataNeedsRefresh.Invoke(this);
            }
        }

        /// <summary>
        /// Internal object for binding this EditInterface to its source object.
        /// Only used by EditInterfaceManager.
        /// </summary>
        internal Object ManagerBinding { get; set; }

        /// <summary>
        /// Set the background color of this component on the UI.
        /// </summary>
        public Color BackColor
        {
            get
            {
                return backColor;
            }
            set
            {
                backColor = value;
                if (OnBackColorChanged != null)
                {
                    OnBackColorChanged.Invoke(this);
                }
            }
        }

        /// <summary>
        /// Set the foreground color of this component on the UI.
        /// </summary>
        public Color ForeColor
        {
            get
            {
                return foreColor;
            }
            set
            {
                foreColor = value;
                if (OnForeColorChanged != null)
                {
                    OnForeColorChanged.Invoke(this);
                }
            }
        }

        /// <summary>
        /// An objec that references the icon that should be shown for this object.
        /// </summary>
        public Object IconReferenceTag
        {
            get
            {
                return iconReferenceTag;
            }
            set
            {
                iconReferenceTag = value;
                if (OnIconReferenceChanged != null)
                {
                    OnIconReferenceChanged.Invoke(this);
                }
            }
        }

        public EditInterface ParentEditInterface { get; private set; }

        public ClipboardEntry ClipboardEntry { get; set; }

        public bool SupportsClipboard
        {
            get
            {
                return ClipboardEntry != null;
            }
        }

        public EditInterfaceRenderer Renderer { get; set; }
    }

    public static class EditInterfaceExtensions
    {
        /// <summary>
        /// This extension method allows an easy way to fire the
        /// DataNeedsRefresh function without having to check for null in the
        /// client code.
        /// </summary>
        /// <param name="editInterface"></param>
        public static void fireDataNeedsRefresh(this EditInterface editInterface)
        {
            if (editInterface != null)
            {
                editInterface._fireDataNeedsRefresh();
            }
        }
    }
}
