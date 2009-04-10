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
        #region Fields

        private Dictionary<String, SimElementManagerDefinition> elementManagers = new Dictionary<String,SimElementManagerDefinition>();
        private Dictionary<String, SimSubSceneDefinition> subSceneDefinitions = new Dictionary<string, SimSubSceneDefinition>();
        private EditInterface editInterface;
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
        }

        /// <summary>
        /// Remove a SimElementManagerDefinition.
        /// </summary>
        /// <param name="def">The definition to remove.</param>
        public void removeSimElementManagerDefinition(SimElementManagerDefinition def)
        {
            elementManagers.Remove(def.Name);
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
        /// Determine if this SimSceneDefinition has any subSceneDefinitions.
        /// </summary>
        /// <returns>True if there are some SubSceneDefinitions.</returns>
        public bool hasSimSubSceneDefinitions()
        {
            return subSceneDefinitions.Count != 0;
        }

        /// <summary>
        /// Add a SimSubSceneDefinition.
        /// </summary>
        /// <param name="def">The definition to add.</param>
        public void addSimSubSceneDefinition(SimSubSceneDefinition def)
        {
            subSceneDefinitions.Add(def.Name, def);
        }

        /// <summary>
        /// Remove a SimSubSceneDefinition.
        /// </summary>
        /// <param name="def">The definition to remove.</param>
        public void removeSimSubSceneDefinition(SimSubSceneDefinition def)
        {
            subSceneDefinitions.Remove(def.Name);
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
                editInterface = new SimSceneEditInterface(this);
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

        /// <summary>
        /// Internal function so the edit interfaces can get the
        /// ElementManagerDefinitions. Should only be called by that class.
        /// </summary>
        /// <returns>An enumerable over the ElementManagerDefinitions.</returns>
        internal IEnumerable<SimElementManagerDefinition> _getElementManagerDefinitions()
        {
            return elementManagers.Values;
        }

        /// <summary>
        /// Internal function so the edit interfaces can get the
        /// SubSceneDefintions. Should only be called by that class.
        /// </summary>
        /// <returns>An enumerable over the SubSceneDefintions.</returns>
        internal IEnumerable<SimSubSceneDefinition> _getSubSceneDefintions()
        {
            return subSceneDefinitions.Values;
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
    }
}
