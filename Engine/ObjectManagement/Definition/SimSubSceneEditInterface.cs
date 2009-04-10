using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Engine
{
    /// <summary>
    /// This is an EditInterface for SimSubSceneDefinitions.
    /// </summary>
    public class SimSubSceneEditInterface : EditInterface
    {
        #region Fields

        private SimSubSceneDefinition definition;
        private LinkedList<EditableProperty> bindings = new LinkedList<EditableProperty>();
        private DestroyEditInterfaceCommand destroyCommand;
        private DestroyEditablePropertyCommand destroyBindingCommand;
        private CreateEditablePropertyCommand createBindingCommand;
        private EditablePropertyInfo propertyInfo = new EditablePropertyInfo();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="definition">The SimSubSceneDefinition to wrap.</param>
        public SimSubSceneEditInterface(SimSubSceneDefinition definition)
        {
            this.definition = definition;
            destroyBindingCommand = new DestroyEditablePropertyCommand("destroySimElementManagerBinding", "Destroy SimElementManager Binding", "Destroy a binding to a SimElementManager.", new DestroyEditablePropertyCommand.DestroyProperty(destroyBinding));
            createBindingCommand = new CreateEditablePropertyCommand("createSimElemenManagerBinding", "Add SimElementManager Binding", "Add a binding to a SimElementManager.", new CreateEditablePropertyCommand.CreateProperty(createBinding));
            propertyInfo.addColumn(new EditablePropertyColumn("Name", false));
            propertyInfo.addColumn(new EditablePropertyColumn("Type", true));
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Add a SimSubSceneBinding.
        /// </summary>
        /// <returns>A new EditableProperty for the created binding.</returns>
        public EditableProperty addBinding()
        {
            SimSubSceneBinding binding = new SimSubSceneBinding(definition);
            bindings.AddLast(binding);
            return binding;
        }

        /// <summary>
        /// Remove a SimSubSceneBinding.
        /// </summary>
        /// <param name="binding"></param>
        public void removeBinding(SimSubSceneBinding binding)
        {
            bindings.Remove(binding);
        }

        /// <summary>
        /// Callback from the command to create a binding.
        /// </summary>
        /// <param name="callback">The UICallback to use.</param>
        /// <param name="subCommand">A subCommand to optionally use.</param>
        /// <returns>A new EditableProperty for the binding.</returns>
        private EditableProperty createBinding(EditUICallback callback, String subCommand)
        {
            return addBinding();
        }

        /// <summary>
        /// Callback from the command to destroy a binding.
        /// </summary>
        /// <param name="property">The EditableProperty for the property being destroyed.</param>
        /// <param name="callback">The UICallback to use.</param>
        /// <param name="subCommand">A subCommand to optionally use.</param>
        private void destroyBinding(EditableProperty property, EditUICallback callback, String subCommand)
        {
            removeBinding((SimSubSceneBinding)property);
        }

        /// <summary>
        /// Set the DestroyObjectCommand.
        /// </summary>
        /// <param name="destroyCommand">The DestroyEditInterfaceCommand to use.</param>
        public void setDestroyObjectCommand(DestroyEditInterfaceCommand destroyCommand)
        {
            this.destroyCommand = destroyCommand;
        }

        #region EditInterface Members

        /// <summary>
        /// Get a name for this interface.
        /// </summary>
        /// <returns>A String with the name of the interface.</returns>
        public string getName()
        {
            return definition.Name + " - Subscene";
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
            return bindings;
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
        /// Determine if this interface can create and destroy properties. If
        /// this returns true both getCreatePropertyCommand and
        /// getDestroyPropertyCommand must be implemented.
        /// </summary>
        /// <returns>True if this interface can create and destroy properties.</returns>
        public bool canAddRemoveProperties()
        {
            return true;
        }

        /// <summary>
        /// Get the command that creates new properties.
        /// </summary>
        /// <returns>A CreateEditablePropertyCommand to create properties or null if it does not have one.</returns>
        public CreateEditablePropertyCommand getCreatePropertyCommand()
        {
            return createBindingCommand;
        }

        /// <summary>
        /// Get the command that destroys properties.
        /// </summary>
        /// <returns>A DestroyEditablePropertyCommand to destroy properties or null if it does not have one.</returns>
        public DestroyEditablePropertyCommand getDestroyPropertyCommand()
        {
            return destroyBindingCommand;
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
            bool allFilled = true;
            foreach (SimSubSceneBinding binding in bindings)
            {
                Object bindingValue = binding.getValue(0);
                allFilled &= bindingValue != null && bindingValue.ToString() != String.Empty;
            }
            if (allFilled)
            {
                errorMessage = null;
                return true;
            }
            else
            {
                errorMessage = "Not all SimElementManager bindings are filled in. Please specify a value for all bindings or remove the empty bindings.";
                return false;
            }
        }

        #endregion

        #endregion Functions
    }
}
