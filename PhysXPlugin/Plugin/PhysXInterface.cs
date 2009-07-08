using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using PhysXWrapper;
using Engine.Platform;
using Engine.ObjectManagement;
using Engine.Command;
using Engine.Resources;

namespace PhysXPlugin
{
    delegate void ConnectRemoteDebugger();

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
        private ShapeFileManager shapeManager = new ShapeFileManager();
        private PhysXDebugInterface debugInterface;

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
            pluginManager.addCreateSimElementManagerCommand(new AddSimElementManagerCommand("Create PhysX Scene Definition", PhysXSceneManagerDefinition.Create));

            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create PhysX Actor", PhysActorDefinition.Create));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create PhysX Cylindrical Joint", PhysCylindricalJointDefinition.Create));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create PhysX D6 Joint", PhysD6JointDefinition.Create));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create PhysX Fixed Joint", PhysFixedJointDefinition.Create));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create PhysX PointInPlane Joint", PhysPointInPlaneJointDefinition.Create));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create PhysX PointOnLine Joint", PhysPointOnLineJointDefinition.Create));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create PhysX Prismatic Joint", PhysPrismaticJointDefinition.Create));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create PhysX Pulley Joint", PhysPulleyJointDefinition.Create));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create PhysX Revolute Joint", PhysRevoluteJointDefinition.Create));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create PhysX Spherical Joint", PhysSphericalJointDefinition.Create));

            SubsystemResources physXResources = new SubsystemResources("PhysX");
            physXResources.addResourceListener(shapeManager);
            pluginManager.addSubsystemResources(physXResources);
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

        public DebugInterface getDebugInterface()
        {
            if (debugInterface == null)
            {
                debugInterface = new PhysXDebugInterface();
            }
            return debugInterface;
        }

        /// <summary>
        /// This function will create any debug commands for the plugin and add them to the commands list.
        /// </summary>
        /// <param name="commands">A list of CommandManagers to add debug commands to.</param>
        public void createDebugCommands(List<CommandManager> commands)
        {
            CommandManager debugCommands = new CommandManager("PhysX");
            debugCommands.addCommand(new EngineCommand("connectRemoteDebugger", "Connect Remote Debugger", "Connect to the PhysX remote debugger.", new ConnectRemoteDebugger(connectRemoteDebugger)));
            debugCommands.addCommand(new EngineCommand("disconnectRemoteDebugger", "Disonnect Remote Debugger", "Disconnect from the PhysX remote debugger.", new ConnectRemoteDebugger(disconnectRemoteDebugger)));
            commands.Add(debugCommands);
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

        private void connectRemoteDebugger()
        {
            physSDK.connectRemoteDebugger("localhost");
        }

        private void disconnectRemoteDebugger()
        {
            physSDK.disconnectRemoteDebugger();
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// The ShapeRepository with all shapes loaded by the plugin.
        /// </summary>
        internal ShapeRepository ShapeRepository
        {
            get
            {
                return shapeManager.getShapeRepository();
            }
        }

        #endregion Properties
    }
}
