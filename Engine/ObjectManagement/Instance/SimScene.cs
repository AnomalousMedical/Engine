using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;

namespace Engine
{
    /// <summary>
    /// A SimScene is a container that holds all SimElementManagers that are
    /// created to represent a given scene. The actual usage of these elements
    /// is defined by the individual SimSubScenes. Each SimScene must have at
    /// least one SimSubScene.
    /// </summary>
    public class SimScene : IDisposable
    {
        #region Fields

        private Dictionary<String, SimElementManager> simElementManagers = new Dictionary<string, SimElementManager>();
        private Dictionary<String, SimSubScene> simSubScenes = new Dictionary<string, SimSubScene>();
        private SimSubScene defaultScene;

        #endregion Fields

        #region Constructors 

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimScene()
        {

        }

        #endregion Constructors

        #region Functions
        
        /// <summary>
        /// Dispose function
        /// </summary>
        public void Dispose()
        {
            foreach (SimElementManager elementManager in simElementManagers.Values)
            {
                elementManager.Dispose();
            }
        }

        /// <summary>
        /// Add a SimElementManager.
        /// </summary>
        /// <param name="manager">The manager to add.</param>
        public void addSimElementManager(SimElementManager manager)
        {
            this.simElementManagers.Add(manager.getName(), manager);
        }

        /// <summary>
        /// Remove a SimElementManager.
        /// </summary>
        /// <param name="manager">The manager to remove.</param>
        public void removeSimElementManager(SimElementManager manager)
        {
            this.simElementManagers.Remove(manager.getName());
        }

        /// <summary>
        /// Get the SimElementManager specified by name.
        /// </summary>
        /// <param name="name">The name of the SimElementManager.</param>
        /// <returns>The specified SimElementManager or null if it cannot be found.</returns>
        public SimElementManager getSimElementManager(String name)
        {
            if (simElementManagers.ContainsKey(name))
            {
                return simElementManagers[name];
            }
            return null;
        }

        /// <summary>
        /// Add a SimSubScene.
        /// </summary>
        /// <param name="scene">The SimSubScene to add.</param>
        public void addSimSubScene(SimSubScene scene)
        {
            this.simSubScenes.Add(scene.Name, scene);
        }

        /// <summary>
        /// Remove a SimSubScene.
        /// </summary>
        /// <param name="scene">The scene to remove.</param>
        public void removeSimSubScene(SimSubScene scene)
        {
            this.simSubScenes.Remove(scene.Name);
        }

        /// <summary>
        /// Get the SimSubScene specified by name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>The specified SimSubScene or null if it does not exist.</returns>
        public SimSubScene getSubScene(String name)
        {
            if (simSubScenes.ContainsKey(name))
            {
                return simSubScenes[name];
            }
            return null;
        }

        /// <summary>
        /// Set the default SimSubScene. The SimSubScene passed to this function
        /// must have already been added to this SimScene. If this is not
        /// correct the log will report the error.
        /// </summary>
        /// <param name="scene">The scene to set as the default.</param>
        public void setDefaultSubScene(SimSubScene scene)
        {
            if (simSubScenes.ContainsKey(scene.Name))
            {
                defaultScene = scene;
            }
            else
            {
                Log.Default.sendMessage("Attempted to set the scene {0} as the default to the SimScene that is not part of the scene. This has not been set as the default.", LogLevel.Warning, "Engine", scene.Name);
            }
        }

        /// <summary>
        /// Get the default SimSubScene. This will be null if it has not been defined.
        /// </summary>
        /// <returns>The default SimSubScene.</returns>
        public SimSubScene getDefaultSubScene()
        {
            return defaultScene;
        }

        /// <summary>
        /// Create a definition.
        /// </summary>
        /// <returns>A new SimSceneDefinition.</returns>
        public SimSceneDefinition createDefinition()
        {
            SimSceneDefinition definition = new SimSceneDefinition();

            foreach (SimElementManager manager in simElementManagers.Values)
            {
                definition.addSimElementManagerDefinition(manager.createDefinition());
            }
            foreach (SimSubScene subScene in simSubScenes.Values)
            {
                definition.addSimSubSceneDefinition(subScene.createDefinition(definition));
            }
            if (defaultScene != null)
            {
                definition.DefaultSubScene = defaultScene.Name;
            }

            return definition;
        }

        #endregion Functions
    }
}
