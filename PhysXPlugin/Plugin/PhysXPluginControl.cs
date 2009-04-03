using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using PhysXWrapper;
using PhysXPlugin.Commands;

namespace PhysXPlugin
{
    /// <summary>
    /// This is the ComponentPlugin class for the PhysXPlugin.
    /// </summary>
    public class PhysXPluginControl : ComponentPlugin
    {
        #region Static

        public const String PluginName = "PhysXPlugin";

        #endregion Static

        #region Fields

        private PhysSDK physSDK;
        private PhysFactory physFactory = new PhysFactory();
        private CommandManager simComponentManagerCommands = new CommandManager();
        private CommandManager simComponentDefinitonCommands = new CommandManager();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public PhysXPluginControl()
        {

        }

        #endregion Constructors

        #region Functions

        public override void Dispose()
        {
            
        }

        /// <summary>
        /// Initalize the PhysXPlugin.
        /// </summary>
        public override void initialize()
        {
            physSDK = PhysSDK.Instance;
            simComponentManagerCommands.addCommand(new CreatePhysSceneCommand(physSDK, physFactory));
        }

        /// <summary>
        /// Shutdown the PhysXPlugin.
        /// </summary>
        public override void shutDown()
        {
            physSDK.Dispose();
        }

        /// <summary>
        /// Get all commands for creating scenes.
        /// </summary>
        /// <returns>A command manager with the scene creation commands.</returns>
        public override CommandManager getCreateSimComponentManagerCommands()
        {
            return simComponentManagerCommands;
        }

        /// <summary>
        /// Get all commands for creating SimComponentDefinitions.
        /// </summary>
        /// <returns>A command manager with commands for creating SimComponentDefintions.</returns>
        public override CommandManager getCreateSimComponentDefinitionCommands()
        {
            return simComponentDefinitonCommands;
        }

        #endregion Functions
    }
}
