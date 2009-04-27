using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using PhysXWrapper;
using EngineMath;
using Engine.Platform;
using Engine.ObjectManagement;

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

        #region Fields

        private PhysSDK physSDK = null;
        private UpdateTimer mainTimer;

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
            pluginManager.addCreateSimElementManagerCommand(new EngineCommand("createPhysSceneDef", "Create PhysX Scene Definition", "Creates a new PhysX scene definition.", new CreateElementManagerDefinition(PhysXSceneManagerDefinition.Create)));

            pluginManager.addCreateSimElementCommand(new EngineCommand("createPhysActorDef", "Create PhysX Actor", "Creates a new PhysX Actor Definition.", new CreateElementDefinition(PhysActorDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new EngineCommand("createPhysCylindricalJointDef", "Create PhysX Cylindrical Joint", "Creates a new PhysX Cylindrical Joint Definition.", new CreateElementDefinition(PhysCylindricalJointDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new EngineCommand("createPhysD6JointDef", "Create PhysX D6 Joint", "Creates a new PhysX D6 Joint Definition.", new CreateElementDefinition(PhysD6JointDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new EngineCommand("createPhysFixedJointDef", "Create PhysX Fixed Joint", "Creates a new PhysX Fixed Joint Definition.", new CreateElementDefinition(PhysFixedJointDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new EngineCommand("createPhysPointInPlaneJointDef", "Create PhysX PointInPlane Joint", "Creates a new PhysX PointInPlane Joint Definition.", new CreateElementDefinition(PhysPointInPlaneJointDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new EngineCommand("createPhysPointOnLineJointDef", "Create PhysX PointOnLine Joint", "Creates a new PhysX PointOnLine Joint Definition.", new CreateElementDefinition(PhysPointOnLineJointDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new EngineCommand("createPhysPrismaticJointDef", "Create PhysX Prismatic Joint", "Creates a new PhysX Prismatic Joint Definition.", new CreateElementDefinition(PhysPrismaticJointDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new EngineCommand("createPhysPulleyJointDef", "Create PhysX Pulley Joint", "Creates a new PhysX Pulley Joint Definition.", new CreateElementDefinition(PhysPulleyJointDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new EngineCommand("createPhysRevoluteJointDef", "Create PhysX Revolute Joint", "Creates a new PhysX Revolute Joint Definition.", new CreateElementDefinition(PhysRevoluteJointDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new EngineCommand("createPhysSphericalJointDef", "Create PhysX Spherical Joint", "Creates a new PhysX Spherical Joint Definition.", new CreateElementDefinition(PhysSphericalJointDefinition.Create)));
        }

        /// <summary>
        /// Set the classes from the platform that a plugin may be interested
        /// in. The timer can be subscribed to for updates and the EventManager
        /// will be updated with events every frame.
        /// </summary>
        /// <param name="mainTimer">The main update timer.</param>
        /// <param name="eventManager">The main event manager.</param>
        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
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
        public UpdateTimer getMainTimer()
        {
            return mainTimer;
        }

        #endregion ElementPlugin

        #region Creation

        /// <summary>
        /// Create a PhysXSceneManager.
        /// </summary>
        /// <param name="definition">The definition of the scene.</param>
        /// <returns>A new PhysXSceneManager.</returns>
        internal PhysXSceneManager createScene(PhysXSceneManagerDefinition definition)
        {
            PhysScene scene = physSDK.createScene(definition.SceneDesc);
            return new PhysXSceneManager(definition.Name, scene, physSDK, mainTimer);
        }

        #endregion Creation

        #endregion Functions
    }
}
