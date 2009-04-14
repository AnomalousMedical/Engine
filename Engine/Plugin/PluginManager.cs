using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Logging;
using System.IO;
using Engine.ObjectManagement;

namespace Engine
{
    /// <summary>
    /// This class loads and unloads the various plugins for the system.
    /// </summary>
    public class PluginManager : IDisposable
    {
        #region Static

        private static PluginManager instance;

        /// <summary>
        /// Get the singleton for the PluginManager. It must first be created in
        /// a using statement with the constructor so it will be disposed.
        /// </summary>
        public static PluginManager Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region Fields

        private Dictionary<String, PluginInterface> loadedPlugins = new Dictionary<string, PluginInterface>();
        private CommandManager createSimElementCommands = new CommandManager();
        private CommandManager createSimElementManagerCommands = new CommandManager();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor. Must be called once before plugins are used.
        /// </summary>
        public PluginManager()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                throw new InvalidPluginException("Can only call the constructor for the PluginManager one time");
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Dispose function.
        /// </summary>
        public void Dispose()
        {
            foreach (PluginInterface plugin in loadedPlugins.Values)
            {
                plugin.Dispose();
            }
            loadedPlugins.Clear();
        }

        /// <summary>
        /// Add a plugin to the PluginManager.
        /// </summary>
        /// <param name="plugin">The plugin to add.</param>
        internal void addPlugin(PluginInterface plugin)
        {
            Log.Default.sendMessage("Plugin {0} added.", LogLevel.Info, "Engine", plugin.getName());
            loadedPlugins.Add(plugin.getName(), plugin);
            plugin.initialize(this);
        }

        /// <summary>
        /// Get the plugin specified by path.
        /// </summary>
        /// <param name="name">The name of the plugin to get.</param>
        /// <returns>The ElementPlugin if it is found or null if it is not.</returns>
        public PluginInterface getPlugin(String name)
        {
            if (loadedPlugins.ContainsKey(name))
            {
                return loadedPlugins[name];
            }
            return null;
        }

        /// <summary>
        /// Add a command to create a SimElementManagerDescription.
        /// </summary>
        /// <param name="command">A command that creates SimElementManagerDescriptions.</param>
        public void addCreateSimElementManagerCommand(EngineCommand command)
        {
            createSimElementManagerCommands.addCommand(command);
        }

        /// <summary>
        /// Add a command to create a SimElementDescription.
        /// </summary>
        /// <param name="command">A command that creates SimElementDescriptions.</param>
        public void addCreateSimElementCommand(EngineCommand command)
        {
            createSimElementCommands.addCommand(command);
        }

        /// <summary>
        /// Get the list of commands from plugins that create SimElementManagers.
        /// </summary>
        /// <returns>The list of commands from plugins that create SimElementManagers.</returns>
        public IEnumerable<EngineCommand> getCreateSimElementManagerCommands()
        {
            return createSimElementManagerCommands.getCommandList();
        }

        /// <summary>
        /// Get a specific command from plugins that creates a SimElementManager.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <returns>The command.</returns>
        public EngineCommand getCreateSimElementManagerCommand(String name)
        {
            return createSimElementManagerCommands.getCommand(name);
        }

        #endregion Functions
    }
}
