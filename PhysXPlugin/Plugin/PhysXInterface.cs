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
        delegate PhysXSceneManager CreateDefaultScene();

        #endregion Delegates

        #region Fields

        private PhysSDK physSDK = null;
        private PhysFactory physFactory = new PhysFactory();
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
            elementManagerCommands.addCommand(new EngineCommand("createPhysScene", "Create PhysX Scene", "Creates a new PhysX scene with the given parameters.", new CreateDefaultScene(createScene)));

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

        /// <summary>
        /// Get the SimElementFactory that creates objects for this plugin.
        /// </summary>
        /// <returns>The PhysFactory that creates objects for this plugin.</returns>
        public override SimElementFactory getElementFactory()
        {
            return physFactory;
        }

        #endregion ElementPlugin

        #region Creation

        public PhysActorDefinition createPhysActorDefinition(String name)
        {
            return new PhysActorDefinition(name, physFactory);
        }

        public PhysXSceneManager createScene()
        {
            using (PhysSceneDesc sceneDesc = new PhysSceneDesc())
            {
                sceneDesc.Gravity = new Vector3(0.0f, -9.8f, 0.0f);
                PhysScene scene = physSDK.createScene(sceneDesc);
                return new PhysXSceneManager(scene, physSDK, physFactory);
            }
        }

        #endregion Creation

        #endregion Functions
    }
}
