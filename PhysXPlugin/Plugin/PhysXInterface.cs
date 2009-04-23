using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using PhysXWrapper;
using EngineMath;
using Engine.Platform;

namespace PhysXPlugin
{
    /// <summary>
    /// This is the ElementPlugin class for the PhysXPlugin.
    /// </summary>
    public class PhysXInterface : PluginInterface
    {
        #region Static

        public const String PluginName = "PhysXPlugin";
        private static PhysXInterface instance;
        public static PhysXInterface Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion Static

        #region Delegates

        delegate PhysActorDefinition CreatePhysActorDefinition(String name);
        delegate PhysXSceneManagerDefinition CreateSceneDesc(String name);
        delegate PhysFixedJointDefinition CreateFixedJointDefinition(String name);
        delegate PhysD6JointDefinition CreateD6JointDefinition(String name);

        #endregion Delegates

        #region Fields

        private PhysSDK physSDK = null;
        private Timer mainTimer;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public PhysXInterface()
        {
            if (Instance == null)
            {
                instance = this;
            }
            else
            {
                throw new Exception("Cannot create the PhysXInterface more than once. Only call the constructor one time.");
            }
        }

        #endregion Constructors

        #region Functions

        #region ElementPlugin

        public void Dispose()
        {
            if (physSDK != null)
            {
                physSDK.Dispose();
                physSDK = null;
            }
        }

        /// <summary>
        /// Initalize the PhysXPlugin.
        /// </summary>
        public void initialize(PluginManager pluginManager)
        {
            physSDK = PhysSDK.Instance;
            pluginManager.addCreateSimElementManagerCommand(new EngineCommand("createPhysSceneDef", "Create PhysX Scene Definition", "Creates a new PhysX scene definition.", new CreateSceneDesc(createSceneDefinition)));

            pluginManager.addCreateSimElementCommand(new EngineCommand("createPhysActorDef", "Create PhysX Actor", "Creates a new PhysX Actor Definition.", new CreatePhysActorDefinition(createPhysActorDefinition)));
            pluginManager.addCreateSimElementCommand(new EngineCommand("createPhysFixedJointDef", "Create PhysX Fixed Joint", "Creates a new PhysX Fixed Joint Definition.", new CreateFixedJointDefinition(createPhysFixedJointDefinition)));
            pluginManager.addCreateSimElementCommand(new EngineCommand("createPhysD6JointDef", "Create PhysX D6 Joint", "Creates a new PhysX D6 Joint Definition.", new CreateD6JointDefinition(createPhysD6JointDefinition)));
        }

        /// <summary>
        /// Set the classes from the platform that a plugin may be interested
        /// in. The timer can be subscribed to for updates and the EventManager
        /// will be updated with events every frame.
        /// </summary>
        /// <param name="mainTimer">The main update timer.</param>
        /// <param name="eventManager">The main event manager.</param>
        public void setPlatformInfo(Timer mainTimer, EventManager eventManager)
        {
            this.mainTimer = mainTimer;
        }

        /// <summary>
        /// Get a name for this plugin. Care should be taken that this return
        /// value is unique. The best way would be to name it after the plugin
        /// dll.
        /// </summary>
        /// <returns>The name of the plugin.</returns>
        public string getName()
        {
            return PluginName;
        }

        /// <summary>
        /// Get the main timer used by the engine.
        /// </summary>
        /// <returns>The timer that has been set as the main timer.</returns>
        public Timer getMainTimer()
        {
            return mainTimer;
        }

        #endregion ElementPlugin

        #region Creation

        /// <summary>
        /// Create a PhysActorDefinition.
        /// </summary>
        /// <param name="name">The name of the PhysActor.</param>
        /// <returns>A new PhysActorDefintion.</returns>
        public PhysActorDefinition createPhysActorDefinition(String name)
        {
            return new PhysActorDefinition(name);
        }

        public PhysFixedJointDefinition createPhysFixedJointDefinition(String name)
        {
            return new PhysFixedJointDefinition(name);
        }

        public PhysD6JointDefinition createPhysD6JointDefinition(String name)
        {
            return new PhysD6JointDefinition(name);
        }

        /// <summary>
        /// Create a PhysXSceneManagerDefinition.
        /// </summary>
        /// <param name="name">The name of the scene.</param>
        /// <returns>A new PhysXSceneManagerDefinition.</returns>
        public PhysXSceneManagerDefinition createSceneDefinition(String name)
        {
            return new PhysXSceneManagerDefinition(name);
        }

        /// <summary>
        /// Create a PhysXSceneManager.
        /// </summary>
        /// <param name="definition">The definition of the scene.</param>
        /// <returns>A new PhysXSceneManager.</returns>
        public PhysXSceneManager createScene(PhysXSceneManagerDefinition definition)
        {
            PhysScene scene = physSDK.createScene(definition.SceneDesc);
            return new PhysXSceneManager(definition.Name, scene, physSDK, mainTimer);
        }

        #endregion Creation

        #endregion Functions
    }
}
