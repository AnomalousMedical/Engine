using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using PhysXWrapper;
using EngineMath;

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

        #endregion Delegates

        #region Fields

        private PhysSDK physSDK = null;
        private CommandManager elementManagerCommands = new CommandManager();
        private CommandManager elementDefinitonCommands = new CommandManager();

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
            elementManagerCommands.addCommand(new EngineCommand("createPhysSceneDef", "Create PhysX Scene Definition", "Creates a new PhysX scene definition.", new CreateSceneDesc(createSceneDefinition)));

            elementDefinitonCommands.addCommand(new EngineCommand("createPhysActorDef", "Create PhysX Actor Definition", "Creates a new PhysX Actor Definition.", new CreatePhysActorDefinition(createPhysActorDefinition)));

            foreach (EngineCommand command in elementManagerCommands.getCommandList())
            {
                pluginManager.addCreateSimElementManagerCommand(command);
            }

            foreach (EngineCommand command in elementDefinitonCommands.getCommandList())
            {
                pluginManager.addCreateSimElementCommand(command);
            }
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
            return new PhysXSceneManager(definition.Name, scene, physSDK);
        }

        #endregion Creation

        #endregion Functions
    }
}
