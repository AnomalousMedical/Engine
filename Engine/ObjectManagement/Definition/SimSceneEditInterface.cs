using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Engine
{
    class SimSceneEditInterface : EditInterface
    {
        #region Static

        private static CommandManager createCommandManager = new CommandManager();

        /// <summary>
        /// Add a command from a subsystem for creating new SimElementManagerDefinitions.
        /// </summary>
        /// <param name="command">The command to add.</param>
        public static void AddCreateSimElementManagerDefinitionCommand(EngineCommand command)
        {
            createCommandManager.addCommand(command);
        }

        #endregion Static

        #region Fields

        private LinkedList<EditInterface> editInterfaces = new LinkedList<EditInterface>();
        private Dictionary<EditInterface, SimElementManagerDefinition> simElementInterfaces = new Dictionary<EditInterface, SimElementManagerDefinition>();
        private LinkedList<CreateEditInterfaceCommand> createCommands = new LinkedList<CreateEditInterfaceCommand>();
        private DestroyEditInterfaceCommand destroySimElementManager;
        private SimSceneDefinition definition;

        #endregion Fields

        #region Constructors

        public SimSceneEditInterface(SimSceneDefinition definition)
        {
            this.definition = definition;
            createCommands.AddLast(new CreateEditInterfaceCommand("createSubSceneDefinition", "Create Sub Scene", "Create a new Sub Scene Definition", new CreateEditInterfaceCommand.CreateSubObject(createSimSubSceneDefinition)));
            foreach (EngineCommand command in createCommandManager.getCommandList())
            {
                CreateEditInterfaceCommand createSubObj = new CreateEditInterfaceCommand(command.Name, command.PrettyName, command.HelpText, new CreateEditInterfaceCommand.CreateSubObject(createSimElementManagerDefinition));
                createSubObj.SubCommand = command.Name;
                createCommands.AddLast(createSubObj);
            }
            destroySimElementManager = new DestroyEditInterfaceCommand("destroySimElementManager", "Delete", "Destroy the selected Sim Object Manager.", new DestroyEditInterfaceCommand.DestroySubObject(destroySimElementManagerDefinition));
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
                    SimSubSceneDefinition subScene = new SimSubSceneDefinition(name);
                    definition.addSimSubSceneDefinition(subScene);
                    EditInterface editInterface = subScene.getEditInterface();
                    editInterfaces.AddLast(editInterface);
                    return editInterface;
                }
            }
            return null;
        }

        /// <summary>
        /// Callback to create new SimElementManagerDefinitions.
        /// </summary>
        /// <param name="target">The SimSceneDefinition that will get the new SimElementManagerDefinition.</param>
        /// <param name="callback">A callback to get additional input from the user.</param>
        /// <param name="subCommand">The name of the command to run to create the SimElementManagerDefinition.</param>
        /// <returns>An EditInterface to the newly created SimElementManagerDefinition or null if there was an error.</returns>
        private EditInterface createSimElementManagerDefinition(EditUICallback callback, String subCommand)
        {
            String name;
            bool accept = callback.getInputString("Enter a name.", out name);
            if (accept)
            {
                while (accept && definition.hasSimElementManagerDefinition(name))
                {
                    accept = callback.getInputString("The given name is already in use. Please provide another.", name, out name);
                }
                if (accept)
                {
                    EngineCommand command = createCommandManager.getCommand(subCommand);
                    SimElementManagerDefinition elementDef = (SimElementManagerDefinition)command.execute(name);
                    definition.addSimElementManagerDefinition(elementDef);
                    EditInterface editInterface = elementDef.getEditInterface(destroySimElementManager);
                    editInterfaces.AddLast(editInterface);
                    simElementInterfaces.Add(editInterface, elementDef);
                    return editInterface;
                }
            }
            return null;
        }

        /// <summary>
        /// This function will remove a SimElementManagerDefinition from this definition.
        /// </summary>
        /// <param name="editInterface">The EditInterface that holds the definition to remove.</param>
        /// <param name="callback">The callback to the UI.</param>
        /// <param name="subCommand">The SubCommand if required.</param>
        private void destroySimElementManagerDefinition(EditInterface editInterface, EditUICallback callback, String subCommand)
        {
            SimElementManagerDefinition elementManager = simElementInterfaces[editInterface];
            definition.removeSimElementManagerDefinition(elementManager);
            elementManager.Dispose();
            simElementInterfaces.Remove(editInterface);
            editInterfaces.Remove(editInterface);
        }

        #region EditInterface Members

        public string getName()
        {
            return "SimSceneDefiniton";
        }

        public bool hasEditableProperties()
        {
            return false;
        }

        public IEnumerable<EditableProperty> getEditableProperties()
        {
            return null;
        }

        public EditablePropertyInfo getPropertyInfo()
        {
            return null;
        }

        public bool hasSubEditInterfaces()
        {
            return true;
        }

        public IEnumerable<EditInterface> getSubEditInterfaces()
        {
            return editInterfaces;
        }

        public bool hasCreateSubObjectCommands()
        {
            return true;
        }

        public IEnumerable<CreateEditInterfaceCommand> getCreateSubObjectCommands()
        {
            return createCommands;
        }

        public bool hasDestroyObjectCommand()
        {
            return false;
        }

        public DestroyEditInterfaceCommand getDestroyObjectCommand()
        {
            return null;
        }

        /// <summary>
        /// Determine if this interface can create properties.
        /// </summary>
        /// <returns>True if this interface can create properties.</returns>
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
            return null;
        }

        /// <summary>
        /// Get the command that destroys properties.
        /// </summary>
        /// <returns>A DestroyEditablePropertyCommand to destroy properties or null if it does not have one.</returns>
        public DestroyEditablePropertyCommand getDestroyPropertyCommand()
        {
            return null;
        }

        #endregion

        #endregion Functions
    }
}
