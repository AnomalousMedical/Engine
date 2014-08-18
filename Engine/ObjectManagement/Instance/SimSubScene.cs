using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// A SimSubScene is a group of SimElementManagers that define a specific
    /// layout of SimElementManagers. This may contain 0 to 1 instance of each
    /// type of SimElementManager currently loaded into the engine.
    /// </summary>
    /// <remarks>
    /// For example, if a SimScene is created with two physics scenes and one
    /// rendering scene a SimSubScene could be created that contains one of the
    /// two physics scenes and the rendering scene, but only one physics scene
    /// would be allowed. If the two physics scenes were both to draw into the
    /// same rendering scene then another SimSubScene could be created with the
    /// other physics scene and the rendering scene. This would give the
    /// illusion that both physics scenes were happening in the same space, when
    /// in reality their physics would likely not be able to interact.
    /// </remarks>
    public class SimSubScene
    {
        #region Fields

        private Dictionary<Type, SimElementManager> simElements = new Dictionary<Type, SimElementManager>();
        private String name;
        private SimScene scene;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the SimSubScene.</param>
        public SimSubScene(String name, SimScene scene)
        {
            this.name = name;
            this.scene = scene;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Check to see if the SimElementManager type specified is defined by
        /// this SubScene.
        /// </summary>
        /// <param name="t">The type to check for.</param>
        /// <returns>True if the SimElementManagerType is contained in this SimSubScene.</returns>
        public bool hasSimElementManagerType(Type t)
        {
            return simElements.ContainsKey(t);
        }

        /// <summary>
        /// Get the SimElementManager that is defined by the type T. The output
        /// is only valid if hasSimElementManagerType returns true.
        /// </summary>
        /// <typeparam name="T">The type of the SimElementManager to recover.</typeparam>
        /// <returns>The SimElementManager for the matching type.</returns>
        public T getSimElementManager<T>()
            where T : SimElementManager
        {
            Type type = typeof(T);
            if (simElements.ContainsKey(type))
            {
                return (T)simElements[type];
            }
            return default(T);
        }

        /// <summary>
        /// Add a SimElementManager. Only one of each type of SimElementManager can be added.
        /// </summary>
        /// <param name="manager">The manager to add.</param>
        public void addSimElementManager(SimElementManager manager)
        {
            Type t = manager.getSimElementManagerType();
            if (!simElements.ContainsKey(t))
            {
                simElements.Add(t, manager);
            }
            else
            {
                Log.Default.sendMessage("Attempted to add a second SimElementManager of type {0}. Only one SimElementManager of each type can be added to a SimSubScene. The duplicate has been ignored.", LogLevel.Warning, "Engine");
            }
        }

        /// <summary>
        /// Create a definition.
        /// </summary>
        /// <returns>A new defintion.</returns>
        public SimSubSceneDefinition createDefinition(SimSceneDefinition scene)
        {
            SimSubSceneDefinition definition = new SimSubSceneDefinition(name);
            definition.setScene(scene);
            foreach (SimElementManager manager in simElements.Values)
            {
                definition.addBinding(scene.getSimElementManagerDefinition(manager.getName()));
            }
            return definition;
        }

        /// <summary>
        /// Force the parent scene to run its build routines again. This will
        /// create any sim objects that are pending since the last build.
        /// </summary>
        public void buildScene(SceneBuildOptions options)
        {
            scene.buildScene(options);
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// The name of the SimSubScene.
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        #endregion Properties
    }
}
