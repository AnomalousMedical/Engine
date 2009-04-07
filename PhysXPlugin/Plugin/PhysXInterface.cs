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
    public class PhysXInterface : ElementPlugin
    {
        #region Static

        public const String PluginName = "PhysXPlugin";

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

        }

        #endregion Constructors

        #region Functions

        #region ElementPlugin

        public override void Dispose()
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
        public override void initialize()
        {
            physSDK = PhysSDK.Instance;
            elementManagerCommands.addCommand(new EngineCommand("createPhysSceneDef", "Create PhysX Scene Definition", "Creates a new PhysX scene definition.", new CreateSceneDesc(createSceneDefinition)));

            elementDefinitonCommands.addCommand(new EngineCommand("createPhysActorDef", "Create PhysX Actor Definition", "Creates a new PhysX Actor Definition.", new CreatePhysActorDefinition(createPhysActorDefinition)));
        }

        /// <summary>
        /// Get all commands for creating scenes.
        /// </summary>
        /// <returns>A command manager with the scene creation commands.</returns>
        public override CommandManager getCreateSimElementManagerCommands()
        {
            return elementManagerCommands;
        }

        /// <summary>
        /// Get all commands for creating SimElementDefinitions.
        /// </summary>
        /// <returns>A command manager with commands for creating SimElementDefintions.</returns>
        public override CommandManager getCreateSimElementDefinitionCommands()
        {
            return elementDefinitonCommands;
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
            return new PhysXSceneManagerDefinition(name, this);
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
