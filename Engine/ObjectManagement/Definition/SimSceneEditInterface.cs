using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.Reflection;

namespace Engine
{
    /// <summary>
    /// This is an EditInterface for SimSceneDefinitions.
    /// </summary>
    class SimSceneEditInterface : EditInterface
    {
        #region Static

        private static MemberScanner memberScanner;

        static SimSceneEditInterface()
        {
            memberScanner = new MemberScanner(EditableAttributeFilter.Instance);
            memberScanner.ProcessFields = false;
        }

        #endregion Static

        #region Fields

        private ReflectedEditInterface reflectedInterface;
        private SimSceneEditElementManagers elementManagers;
        private SimSceneEditSubScenes subScenes;
        private SimSceneDefinition definition;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="definition">The definition to fill out with this interface.</param>
        public SimSceneEditInterface(SimSceneDefinition definition)
        {
            this.definition = definition;
            reflectedInterface = new ReflectedEditInterface(definition, memberScanner, "", "");
            elementManagers = new SimSceneEditElementManagers(definition);
            reflectedInterface.addManualSubInterface(elementManagers);
            subScenes = new SimSceneEditSubScenes(definition);
            reflectedInterface.addManualSubInterface(subScenes);
        }

        #endregion Constructors

        #region Functions

        #region EditInterface Members

        /// <summary>
        /// Get a name for this interface.
        /// </summary>
        /// <returns>A String with the name of the interface.</returns>
        public string getName()
        {
            return "Sim Scene";
        }

        /// <summary>
        /// Determine if this EditInterface has any EditableProperties.
        /// </summary>
        /// <returns>True if the interface has some EditableProperties.</returns>
        public bool hasEditableProperties()
        {
            return reflectedInterface.hasEditableProperties();
        }

        /// <summary>
        /// This function will return all properties of an EditInterface.
        /// </summary>
        /// <returns>A enumerable over all properties in the EditInterface or null if there aren't any.</returns>
        public IEnumerable<EditableProperty> getEditableProperties()
        {
            return reflectedInterface.getEditableProperties();
        }

        /// <summary>
        /// Return the EditablePropertyInfo for this interface that determines
        /// the layout of a single property. This can be null if
        /// hasEditableProperties is false.
        /// </summary>
        /// <returns>The EditablePropertyInfo for this interface.</returns>
        public EditablePropertyInfo getPropertyInfo()
        {
            return reflectedInterface.getPropertyInfo();
        }

        /// <summary>
        /// Determine if this EditInterface has any SubEditInterfaces.
        /// </summary>
        /// <returns>True if the interface has some SubEditInterfaces.</returns>
        public bool hasSubEditInterfaces()
        {
            return reflectedInterface.hasSubEditInterfaces();
        }

        /// <summary>
        /// Get any SubEditInterfaces in this interface.
        /// </summary>
        /// <returns>An enumerable over all EditInterfaces that are part of this EditInterface or null if there aren't any.</returns>
        public IEnumerable<EditInterface> getSubEditInterfaces()
        {
            return reflectedInterface.getSubEditInterfaces();
        }

        /// <summary>
        /// Determine if this EditInterface has any CreateSubObjectCommands.
        /// </summary>
        /// <returns>True if there are create commands.</returns>
        public bool hasCreateSubObjectCommands()
        {
            return reflectedInterface.hasCreateSubObjectCommands();
        }

        /// <summary>
        /// Get a list of commands for creating sub objects.
        /// </summary>
        /// <returns>An IEnumerable over all creation commands or null if there aren't any.</returns>
        public IEnumerable<CreateEditInterfaceCommand> getCreateSubObjectCommands()
        {
            return reflectedInterface.getCreateSubObjectCommands();
        }

        public bool hasDestroyObjectCommand()
        {
            return reflectedInterface.hasDestroyObjectCommand();
        }

        /// <summary>
        /// Get a command that will destroy this object. This command must
        /// accept a single argument that is a EditUICallback. This is optional
        /// and can be null.
        /// </summary>
        /// <returns>A command that will destroy this EditInterface object or null if it cannot be destroyed.</returns>
        public DestroyEditInterfaceCommand getDestroyObjectCommand()
        {
            return reflectedInterface.getDestroyObjectCommand();
        }

        /// <summary>
        /// Determine if this interface can create properties.
        /// </summary>
        /// <returns>True if this interface can create properties.</returns>
        public bool canAddRemoveProperties()
        {
            return reflectedInterface.canAddRemoveProperties();
        }

        /// <summary>
        /// Get the command that creates new properties.
        /// </summary>
        /// <returns>A CreateEditablePropertyCommand to create properties or null if it does not have one.</returns>
        public CreateEditablePropertyCommand getCreatePropertyCommand()
        {
            return reflectedInterface.getCreatePropertyCommand();
        }

        /// <summary>
        /// Get the command that destroys properties.
        /// </summary>
        /// <returns>A DestroyEditablePropertyCommand to destroy properties or null if it does not have one.</returns>
        public DestroyEditablePropertyCommand getDestroyPropertyCommand()
        {
            return reflectedInterface.getDestroyPropertyCommand();
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
            if (definition.hasSimSubSceneDefinitions())
            {
                if (definition.DefaultSubScene == null || definition.DefaultSubScene == String.Empty)
                {
                    errorMessage = "No default subscene defined. Please enter the name of one of the subscenes to act as a default.";
                    return false;
                }
                if (!definition.hasSimSubSceneDefinition(definition.DefaultSubScene))
                {
                    errorMessage = String.Format("The default subscene {0} is not a valid subscene for this sim scene. Please enter a name of an existing subscene.", definition.DefaultSubScene);
                    return false;
                }
            }
            errorMessage = null;
            return true;
        }

        #endregion

        #endregion Functions
    }
}
