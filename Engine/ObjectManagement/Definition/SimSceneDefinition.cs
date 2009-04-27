using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Logging;
using Engine.Saving;
using Engine.Command;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This is a definiton class for a SimSubScene.
    /// </summary>
    public class SimSceneDefinition : Saveable
    {
        #region Fields

        private Dictionary<String, SimElementManagerDefinition> elementManagers = new Dictionary<String,SimElementManagerDefinition>();
        private Dictionary<String, SimSubSceneDefinition> subSceneDefinitions = new Dictionary<string, SimSubSceneDefinition>();
        private EditInterfaceManager<SimElementManagerDefinition> elementManagerInterfaces;
        private EditInterfaceManager<SimSubSceneDefinition> subSceneInterfaces;
        private Dictionary<EditInterfaceCommand, AddSimElementManagerCommand> createSimElementManagerDefs = new Dictionary<EditInterfaceCommand, AddSimElementManagerCommand>();
        private EditInterface editInterface;
        private EditInterface simElementEditor;
        private EditInterface subScenes;
        private EditInterfaceCommand destroySimElementManagerDef;
        private EditInterfaceCommand destroySubScene;

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
                elementManagerInterfaces.removeSubInterface(def);
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
            def.setScene(this);
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
            def.setScene(null);
            if (editInterface != null)
            {
                subSceneInterfaces.removeSubInterface(def);
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
                editInterface = ReflectedEditInterface.createEditInterface(this, ReflectedEditInterface.DefaultScanner, "Sim Scene", validate);
        
                simElementEditor = new EditInterface("Sim Element Managers");
                elementManagerInterfaces = new EditInterfaceManager<SimElementManagerDefinition>(simElementEditor);
                foreach (AddSimElementManagerCommand command in PluginManager.Instance.getCreateSimElementManagerCommands())
                {
                    EditInterfaceCommand interfaceCommand = new EditInterfaceCommand(command.Name, new EditInterfaceFunction(createSimElementManagerDefinition));
                    createSimElementManagerDefs.Add(interfaceCommand, command);
                    simElementEditor.addCommand(interfaceCommand);
                }
                destroySimElementManagerDef = new EditInterfaceCommand("Destroy", destroySimElementManagerDefinition);
                editInterface.addSubInterface(simElementEditor);

                subScenes = new EditInterface("Subscenes");
                subSceneInterfaces = new EditInterfaceManager<SimSubSceneDefinition>(subScenes);
                EditInterfaceCommand createSubSceneCommand = new EditInterfaceCommand("Create Subscene", new EditInterfaceFunction(createSimSubSceneDefinition));
                subScenes.addCommand(createSubSceneCommand);
                editInterface.addSubInterface(subScenes);
                destroySubScene = new EditInterfaceCommand("Destroy", destroySimSubSceneDefinition);

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
                SimElementManagerDefinition def = createSimElementManagerDefs[caller].execute(name, callback);
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
            removeSimElementManagerDefinition(elementManagerInterfaces.resolveSourceObject(currentInterface));
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
                SimSubSceneDefinition def = new SimSubSceneDefinition(name);
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
            removeSimSubSceneDefinition(subSceneInterfaces.resolveSourceObject(currentInterface));
        }

        /// <summary>
        /// Helper function to create an edit interface.
        /// </summary>
        /// <param name="def"></param>
        private void createEditInterface(SimElementManagerDefinition def)
        {
            EditInterface defInterface = def.getEditInterface();
            defInterface.addCommand(destroySimElementManagerDef);
            elementManagerInterfaces.addSubInterface(def, defInterface);
        }

        /// <summary>
        /// Helper function to create an edit interface.
        /// </summary>
        /// <param name="def"></param>
        private void createEditInterface(SimSubSceneDefinition def)
        {
            EditInterface edit = def.getEditInterface();
            edit.addCommand(destroySubScene);
            subSceneInterfaces.addSubInterface(def, edit);
        }

        private bool validate(out String message)
        {
            if (hasSimSubSceneDefinitions())
            {
                if (DefaultSubScene == null)
                {
                    message = "Please specify one of the Subscenes as the default.";
                    return false;
                }
                if (!hasSimSubSceneDefinition(DefaultSubScene))
                {
                    message = String.Format("{0} is not a valid Subscene. Please specify an existing scene.");
                    return false;
                }
            }

            message = null;
            return true;
        }

        #endregion Helper Functions

        #region Saveable Members

        private const String ELEMENT_MANAGERS_BASE = "ElementManager";
        private const String SUB_SCENE_BASE = "SubScene";
        private const String DEFAULT_SUB_SCENE = "DefaultSubScene";

        private SimSceneDefinition(LoadInfo info)
        {
            DefaultSubScene = info.GetString(DEFAULT_SUB_SCENE);
            for (int i = 0; info.hasValue(ELEMENT_MANAGERS_BASE + i); i++)
            {
                addSimElementManagerDefinition(info.GetValue<SimElementManagerDefinition>(ELEMENT_MANAGERS_BASE + i));
            }
            for (int i = 0; info.hasValue(SUB_SCENE_BASE + i); i++)
            {
                addSimSubSceneDefinition(info.GetValue<SimSubSceneDefinition>(SUB_SCENE_BASE + i));
            }
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(DEFAULT_SUB_SCENE, DefaultSubScene);
            int i = 0;
            foreach (SimElementManagerDefinition manager in elementManagers.Values)
            {
                info.AddValue(ELEMENT_MANAGERS_BASE + i++, manager);
            }
            i = 0;
            foreach (SimSubSceneDefinition subScene in subSceneDefinitions.Values)
            {
                info.AddValue(SUB_SCENE_BASE + i++, subScene);
            }
        }

        #endregion
    }
}
