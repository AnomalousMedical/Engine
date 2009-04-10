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
    public class SimScene
    {
        #region Fields

        private Dictionary<String, SimElementManager> simElementManagers = new Dictionary<string, SimElementManager>();
        private Dictionary<String, SimSubScene> simSubScene = new Dictionary<string, SimSubScene>();
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
        /// Add a SimSubScene.
        /// </summary>
        /// <param name="scene">The SimSubScene to add.</param>
        public void addSimSubScene(SimSubScene scene)
        {
            this.simSubScene.Add(scene.Name, scene);
        }

        /// <summary>
        /// Remove a SimSubScene.
        /// </summary>
        /// <param name="scene">The scene to remove.</param>
        public void removeSimSubScene(SimSubScene scene)
        {
            this.simSubScene.Remove(scene.Name);
        }

        /// <summary>
        /// Set the default SimSubScene. The SimSubScene passed to this function
        /// must have already been added to this SimScene. If this is not
        /// correct the log will report the error.
        /// </summary>
        /// <param name="scene">The scene to set as the default.</param>
        public void setDefaultSubScene(SimSubScene scene)
        {
            if (simSubScene.ContainsKey(scene.Name))
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

        #endregion Functions
    }
}
