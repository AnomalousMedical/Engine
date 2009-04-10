using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Logging;

namespace Engine
{
    /// <summary>
    /// This is a definiton class for a SimSubScene.
    /// </summary>
    public class SimSceneDefinition
    {
        #region Static

        private static Dictionary<String, EngineCommand> commandList = new Dictionary<string, EngineCommand>();

        internal static void AddCreateSimElementManagerDefinitionCommand(EngineCommand command)
        {
            commandList.Add(command.Name, command);
        }

        #endregion Static

        #region Fields

        private Dictionary<String, SimElementManagerDefinition> elementManagers = new Dictionary<String,SimElementManagerDefinition>();
        private Dictionary<String, SimSubSceneDefinition> subSceneDefinitions = new Dictionary<string, SimSubSceneDefinition>();
        private Dictionary<String, EditInterface> elementManagerEditInterfaces = new Dictionary<string, EditInterface>();
        private Dictionary<String, EditInterface> subSceneManagerEditInterfaces = new Dictionary<string, EditInterface>();
        private Dictionary<EditInterfaceCommand, String> createSimElementManagerDefs = new Dictionary<EditInterfaceCommand, string>();
        private EditInterface editInterface;
        private EditInterfaceCommand destroySimElementManagerDef;

        private String defaultScene;
        
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public SimSceneDefinition()
        {
            
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Add a SimElementManagerDefinition.
        /// </summary>
        /// <param name="def">The definition to add.</param>
        public void addSimElementManagerDefinition(SimElementManagerDefinition def)
        {
            elementManagers.Add(def.Name, def);
            if (editInterface != null)
            {
                createEditInterface(def);
            }
        }

        /// <summary>
        /// Remove a SimElementManagerDefinition.
        /// </summary>
        /// <param name="def">The definition to remove.</param>
        public void removeSimElementManagerDefinition(SimElementManagerDefinition def)
        {
            elementManagers.Remove(def.Name);
            if (editInterface != null)
            {
                EditInterface defInterface = elementManagerEditInterfaces[def.Name];
                elementManagerEditInterfaces.Remove(def.Name);
                editInterface.removeSubInterface(defInterface);
            }
        }

        /// <summary>
        /// Get a SimElementManagerDefinition specified by name. Will return
        /// null if it cannot be found.
        /// </summary>
        /// <param name="name">The name of the definition to find.</param>
        /// <returns>The matching defintion or null if it cannot be found.</returns>
        public SimElementManagerDefinition getSimElementManagerDefinition(String name)
        {
            if (elementManagers.ContainsKey(name))
            {
                return elementManagers[name];
            }
            return null;
        }

        /// <summary>
        /// Check to see if a SimElementManagerDefinition is part of this
        /// definition.
        /// </summary>
        /// <param name="name">The name of the definition.</param>
        /// <returns>True if the name is one of the definitons for this scene.</returns>
        public bool hasSimElementManagerDefinition(String name)
        {
            return elementManagers.ContainsKey(name);
        }

        /// <summary>
        /// Add a SimSubSceneDefinition.
        /// </summary>
        /// <param name="def">The definition to add.</param>
        public void addSimSubSceneDefinition(SimSubSceneDefinition def)
        {
            subSceneDefinitions.Add(def.Name, def);
            if (editInterface != null)
            {
                createEditInterface(def);
            }
        }

        /// <summary>
        /// Remove a SimSubSceneDefinition.
        /// </summary>
        /// <param name="def">The definition to remove.</param>
        public void removeSimSubSceneDefinition(SimSubSceneDefinition def)
        {
            subSceneDefinitions.Remove(def.Name);
            if (editInterface != null)
            {
                EditInterface edit = subSceneManagerEditInterfaces[def.Name];
                subSceneManagerEditInterfaces.Remove(def.Name);
                editInterface.removeSubInterface(edit);
            }
        }

        /// <summary>
        /// Determine if this SimSceneDefinition has any subSceneDefinitions.
        /// </summary>
        /// <returns>True if there are some SubSceneDefinitions.</returns>
        public bool hasSimSubSceneDefinitions()
        {
            return subSceneDefinitions.Count != 0;
        }

        /// <summary>
        /// Check to see if a named definition exists.
        /// </summary>
        /// <param name="name">The name to check for.</param>
        /// <returns>True if the definition is part of this class.</returns>
        public bool hasSimSubSceneDefinition(String name)
        {
            return subSceneDefinitions.ContainsKey(name);
        }

        /// <summary>
        /// Get the EditInterface for this SimSceneDefinition.
        /// </summary>
        /// <returns>The EditInterface for this SimSceneDefinition.</returns>
        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, ReflectedEditInterface.DefaultScanner, "Sim Scene", null);
                foreach (EngineCommand command in commandList.Values)
                {
                    EditInterfaceCommand interfaceCommand = new EditInterfaceCommand(command.PrettyName, new EditInterfaceFunction(createSimElementManagerDefinition));
                    createSimElementManagerDefs.Add(interfaceCommand, command.Name);
                    editInterface.addCommand(interfaceCommand);
                }
                destroySimElementManagerDef = new EditInterfaceCommand("Destroy", new EditInterfaceFunction(destroySimElementManagerDefinition));
                foreach (SimElementManagerDefinition elementDef in elementManagers.Values)
                {
                    createEditInterface(elementDef);
                }
                foreach (SimSubSceneDefinition subSceneDef in subSceneDefinitions.Values)
                {
                    createEditInterface(subSceneDef);
                }
            }
            return editInterface;
        }

        /// <summary>
        /// Create and return a new SimScene from this definition.
        /// </summary>
        /// <returns>A new scene configured like this definition.</returns>
        public SimScene createScene()
        {
            SimScene scene = new SimScene();
            foreach (SimElementManagerDefinition elementManagerDef in elementManagers.Values)
            {
                scene.addSimElementManager(elementManagerDef.createSimElementManager());
            }
            foreach (SimSubSceneDefinition subSceneDef in subSceneDefinitions.Values)
            {
                subSceneDef.createSubScene(scene);
            }
            if (DefaultSubScene != null)
            {
                SimSubScene subScene = scene.getSubScene(DefaultSubScene);
                if (subScene != null)
                {
                    scene.setDefaultSubScene(subScene);
                }
                else
                {
                    Log.Default.sendMessage("The defined default scene {0} can not be found in the created scene. No default set.", LogLevel.Warning, "Engine", DefaultSubScene);
                }
            }
            else
            {
                Log.Default.sendMessage("No default scene defined. No default set.", LogLevel.Warning, "Engine");
            }
            return scene;
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// The name of the SimSubScene to use as the default scene for creating SimObjects.
        /// </summary>
        [Editable("The name of the SimSubScene to use as the default scene for creating SimObjects.")]
        public String DefaultSubScene
        {
            get
            {
                return defaultScene;
            }
            set
            {
                defaultScene = value;
            }
        }

        #endregion Properties

        #region Helper Functions

        /// <summary>
        /// Callback to create a SimElementManagerDefinition.
        /// </summary>
        /// <param name="callback">The EditUICallback to get more info from the user.</param>
        /// <param name="caller">The command that initiated this funciton call.</param>
        private void createSimElementManagerDefinition(EditUICallback callback, EditInterfaceCommand caller)
        {
            String name;
            bool accept = callback.getInputString("Enter a name.", out name);
            while (accept && this.hasSimElementManagerDefinition(name))
            {
                accept = callback.getInputString("That name is already in use. Please provide another.", name, out name);
            }
            if (accept)
            {
                SimElementManagerDefinition def = (SimElementManagerDefinition)commandList[createSimElementManagerDefs[caller]].execute(name);
                this.addSimElementManagerDefinition(def);
            }
        }

        /// <summary>
        /// Callback to destroy a SimElementManagerDefinition.
        /// </summary>
        /// <param name="callback">The EditUICallback to get more info from the user.</param>
        /// <param name="caller">The command that initiated this funciton call.</param>
        private void destroySimElementManagerDefinition(EditUICallback callback, EditInterfaceCommand caller)
        {
            EditInterface currentInterface = callback.getSelectedEditInterface();
            removeSimElementManagerDefinition((SimElementManagerDefinition)currentInterface.UserObject);
        }

        /// <summary>
        /// Callback to create a SimSubSceneDefinition.
        /// </summary>
        /// <param name="callback">The EditUICallback to get more info from the user.</param>
        /// <param name="caller">The command that initiated this funciton call.</param>
        private void createSimSubSceneDefinition(EditUICallback callback, EditInterfaceCommand caller)
        {
            String name;
            bool accept = callback.getInputString("Enter a name.", out name);
            while (accept && this.hasSimSubSceneDefinition(name))
            {
                accept = callback.getInputString("That name is already in use. Please provide another.", name, out name);
            }
            if (accept)
            {
                SimSubSceneDefinition def = new SimSubSceneDefinition(name, this);
                this.addSimSubSceneDefinition(def);
            }
        }

        /// <summary>
        /// Callback to destroy a SimSubSceneDefinition.
        /// </summary>
        /// <param name="callback">The EditUICallback to get more info from the user.</param>
        /// <param name="caller">The command that initiated this funciton call.</param>
        private void destroySimSubSceneDefinition(EditUICallback callback, EditInterfaceCommand caller)
        {
            EditInterface currentInterface = callback.getSelectedEditInterface();
            removeSimSubSceneDefinition((SimSubSceneDefinition)currentInterface.UserObject);
        }

        /// <summary>
        /// Helper function to create an edit interface.
        /// </summary>
        /// <param name="def"></param>
        private void createEditInterface(SimElementManagerDefinition def)
        {
            EditInterface defInterface = def.getEditInterface();
            defInterface.UserObject = def;
            defInterface.addCommand(destroySimElementManagerDef);
            elementManagerEditInterfaces.Add(def.Name, defInterface);
            editInterface.addSubInterface(defInterface);
        }

        /// <summary>
        /// Helper function to create an edit interface.
        /// </summary>
        /// <param name="def"></param>
        private void createEditInterface(SimSubSceneDefinition def)
        {
            EditInterface edit = def.getEditInterface();
            edit.UserObject = def;
            subSceneManagerEditInterfaces.Add(def.Name, edit);
            editInterface.addSubInterface(edit);
        }

        #endregion Helper Functions
    }
}
