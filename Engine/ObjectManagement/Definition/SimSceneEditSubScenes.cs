using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Engine
{
    class SimSceneEditSubScenes : EditInterface
    {
        #region Fields

        private LinkedList<EditInterface> editInterfaces = new LinkedList<EditInterface>();
        private Dictionary<EditInterface, SimSubSceneDefinition> simSubSceneDefinitions = new Dictionary<EditInterface, SimSubSceneDefinition>();
        private LinkedList<CreateEditInterfaceCommand> createCommands = new LinkedList<CreateEditInterfaceCommand>();
        private DestroyEditInterfaceCommand destroySimSubScene;
        private SimSceneDefinition definition;

        #endregion Fields

        #region Constructors

        public SimSceneEditSubScenes(SimSceneDefinition definition)
        {
            createCommands.AddLast(new CreateEditInterfaceCommand("createSubSceneDefinition", "Create Sub Scene", "Create a new Sub Scene Definition", new CreateEditInterfaceCommand.CreateSubObject(createSimSubSceneDefinition)));
            destroySimSubScene = new DestroyEditInterfaceCommand("destroySubScene", "Delete", "Destroy the selected subscene.", new DestroyEditInterfaceCommand.DestroySubObject(destroySimSubSceneDefinition));
            this.definition = definition;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// This function will create a new SimSubSceneDefinition.
        /// </summary>
        /// <param name="target">The SimSceneDefinition that will get the SubSceneDefinition.</param>
        /// <param name="callback">A callback to get additional input from the user.</param>
        /// <param name="subCommand">The name of the command to run to create the SimElementManagerDefinition.</param>
        /// <returns>An EditInterface to the newly created SimSubSceneDefinition or null if there was an error.</returns>
        private EditInterface createSimSubSceneDefinition(EditUICallback callback, String subCommand)
        {
            String name;
            bool accept = callback.getInputString("Enter a name for the subscene.", out name);
            if (accept)
            {
                while (accept && definition.hasSimElementManagerDefinition(name))
                {
                    accept = callback.getInputString("The given name is already in use. Please provide another.", name, out name);
                }
                if (accept)
                {
                    SimSubSceneDefinition subScene = new SimSubSceneDefinition(name, definition);
                    definition.addSimSubSceneDefinition(subScene);
                    SimSubSceneEditInterface editInterface = subScene.getEditInterface();
                    editInterfaces.AddLast(editInterface);
                    simSubSceneDefinitions.Add(editInterface, subScene);
                    editInterface.setDestroyObjectCommand(destroySimSubScene);
                    return editInterface;
                }
            }
            return null;
        }

        /// <summary>
        /// This function will destroy a SimSubSceneDefinition.
        /// </summary>
        /// <param name="editInterface">The EditInterface for the SimSubScene.</param>
        /// <param name="callback">A callback to get additional input from the user.</param>
        /// <param name="subCommand">The name of the command to run to create the SimElementManagerDefinition.</param>
        private void destroySimSubSceneDefinition(EditInterface editInterface, EditUICallback callback, String subCommand)
        {
            SimSubSceneDefinition subScene = simSubSceneDefinitions[editInterface];
            definition.removeSimSubSceneDefinition(subScene);
            simSubSceneDefinitions.Remove(editInterface);
            editInterfaces.Remove(editInterface);
        }

        #region EditInterface Members

        /// <summary>
        /// Get a name for this interface.
        /// </summary>
        /// <returns>A String with the name of the interface.</returns>
        public string getName()
        {
            return "Sub Scenes";
        }

        /// <summary>
        /// Determine if this EditInterface has any EditableProperties.
        /// </summary>
        /// <returns>True if the interface has some EditableProperties.</returns>
        public bool hasEditableProperties()
        {
            return false;
        }

        /// <summary>
        /// This function will return all properties of an EditInterface.
        /// </summary>
        /// <returns>A enumerable over all properties in the EditInterface or null if there aren't any.</returns>
        public IEnumerable<EditableProperty> getEditableProperties()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return the EditablePropertyInfo for this interface that determines
        /// the layout of a single property. This can be null if
        /// hasEditableProperties is false.
        /// </summary>
        /// <returns>The EditablePropertyInfo for this interface.</returns>
        public EditablePropertyInfo getPropertyInfo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determine if this EditInterface has any SubEditInterfaces.
        /// </summary>
        /// <returns>True if the interface has some SubEditInterfaces.</returns>
        public bool hasSubEditInterfaces()
        {
            return true;
        }

        /// <summary>
        /// Get any SubEditInterfaces in this interface.
        /// </summary>
        /// <returns>An enumerable over all EditInterfaces that are part of this EditInterface or null if there aren't any.</returns>
        public IEnumerable<EditInterface> getSubEditInterfaces()
        {
            return editInterfaces;
        }

        /// <summary>
        /// Determine if this EditInterface has any CreateSubObjectCommands.
        /// </summary>
        /// <returns>True if there are create commands.</returns>
        public bool hasCreateSubObjectCommands()
        {
            return true;
        }

        /// <summary>
        /// Get a list of commands for creating sub objects.
        /// </summary>
        /// <returns>An IEnumerable over all creation commands or null if there aren't any.</returns>
        public IEnumerable<CreateEditInterfaceCommand> getCreateSubObjectCommands()
        {
            return createCommands;
        }

        /// <summary>
        /// Determine if this interface has a command to destroy itself.
        /// </summary>
        /// <returns>True if there is a destroy command.</returns>
        public bool hasDestroyObjectCommand()
        {
            return false;
        }

        /// <summary>
        /// Get a command that will destroy this object. This command must
        /// accept a single argument that is a EditUICallback. This is optional
        /// and can be null.
        /// </summary>
        /// <returns>A command that will destroy this EditInterface object or null if it cannot be destroyed.</returns>
        public DestroyEditInterfaceCommand getDestroyObjectCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determine if this interface can create and destroy properties. If
        /// this returns true both getCreatePropertyCommand and
        /// getDestroyPropertyCommand must be implemented.
        /// </summary>
        /// <returns>True if this interface can create and destroy properties.</returns>
        public bool canAddRemoveProperties()
        {
            return false;
        }

        /// <summary>
        /// Get the command that creates new properties.
        /// </summary>
        /// <returns>A CreateEditablePropertyCommand to create properties or null if it does not have one.</returns>
        public CreateEditablePropertyCommand getCreatePropertyCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the command that destroys properties.
        /// </summary>
        /// <returns>A DestroyEditablePropertyCommand to destroy properties or null if it does not have one.</returns>
        public DestroyEditablePropertyCommand getDestroyPropertyCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function will validate the data in the EditInterface and return
        /// true if it is valid. It will also fill out errorMessage with any
        /// errors that may occur.
        /// </summary>
        /// <param name="errorMessage">A string that will get an error message for the interface.</param>
        /// <returns>True if the settings are valid, false if they are not.</returns>
        public bool validate(out string errorMessage)
        {
            errorMessage = null;
            return true;
        }

        #endregion

        #endregion Functions
    }
}
