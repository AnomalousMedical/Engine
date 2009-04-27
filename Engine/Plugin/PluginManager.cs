using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Logging;
using System.IO;
using Engine.ObjectManagement;
using Engine.Platform;
using Engine.Renderer;

namespace Engine
{
    /// <summary>
    /// Callback for when the renderer plugin is found and needs to configure its window.
    /// </summary>
    /// <param name="defaultWindow">Assign this to a new DefaultWindowInfo instance.</param>
    public delegate void ConfigureDefaultWindow(out DefaultWindowInfo defaultWindow);

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
        private CommandManager otherCommands = new CommandManager();
        private PlatformPlugin platformPlugin = null;
        private RendererPlugin rendererPlugin = null;

        #endregion Fields

        #region Events

        /// <summary>
        /// This event is called when the RendererPlugin is found, but before
        /// the Renderer is initialized. This gives the application a chance to
        /// modify the window auto-creation settings.
        /// </summary>
        public ConfigureDefaultWindow OnConfigureDefaultWindow;

        #endregion Events

        #region Constructors

        /// <summary>
        /// Constructor. Must be called once before plugins are used.
        /// </summary>
        public PluginManager()
        {
            if (instance == null)
            {
                instance = this;
                instance.addPlugin(new BehaviorInterface());
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
        /// Set the PlatformPlugin for this run of the engine. It is only valid
        /// to set this one time. Any attempts to set this after the first time
        /// will throw an InvalidPluginException.
        /// </summary>
        /// <param name="plugin">The PlatformPlugin to use.</param>
        public void setPlatformPlugin(PlatformPlugin plugin)
        {
            if (platformPlugin == null)
            {
                platformPlugin = plugin;
                Log.Default.sendMessage("Platform plugin set to {0}.", LogLevel.Info, "Engine", plugin.getName());
            }
            else
            {
                throw new InvalidPluginException("A second platform plugin was added. It is only valid to specify one platform plugin please correct this issue.");
            }
        }

        /// <summary>
        /// Set the RendererPlugin for the run of this engine. It is only valid
        /// to set this one time. Any attempts to set this after the first time
        /// will throw an InvalidPluginException. This will also fire the
        /// OnRendererPluginFound where the default window creation settings can
        /// be overwritten.
        /// </summary>
        /// <param name="plugin">The RendererPlugin to use.</param>
        /// <param name="defaultWindow">Out variable that will contain the setup for the default window.</param>
        public void setRendererPlugin(RendererPlugin plugin, out DefaultWindowInfo defaultWindow)
        {
            if (rendererPlugin == null)
            {
                rendererPlugin = plugin;
                Log.Default.sendMessage("Renderer plugin set to {0}.", LogLevel.Info, "Engine", plugin.getName());
                if (OnConfigureDefaultWindow != null)
                {
                    OnConfigureDefaultWindow.Invoke(out defaultWindow);
                    if (defaultWindow == null)
                    {
                        throw new InvalidPluginException("A custom window configure function was invoked, but it did not set the default window up correctly. The renderer cannot be initalized.");
                    }
                }
                else
                {
                    defaultWindow = new DefaultWindowInfo("Anomalous Engine", 640, 480);
                }
            }
            else
            {
                throw new InvalidPluginException("A second renderer plugin was added. It is only valid to specify one renderer plugin please correct this issue.");
            }
        }

        /// <summary>
        /// Set the classes from the platform that the plugins may be interested
        /// in. The timer can be subscribed to for updates and the EventManager
        /// will be updated with events every frame. This should be called as
        /// soon as possible and before any plugins are used besides the
        /// PlatformPlugin.
        /// </summary>
        /// <param name="mainTimer">The main update timer.</param>
        /// <param name="eventManager">The main event manager.</param>
        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            foreach (PluginInterface plugin in loadedPlugins.Values)
            {
                plugin.setPlatformInfo(mainTimer, eventManager);
            }
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

        /// <summary>
        /// Add a command to create a SimElementDescription.
        /// </summary>
        /// <param name="command">A command that creates SimElementDescriptions.</param>
        public void addCreateSimElementCommand(EngineCommand command)
        {
            createSimElementCommands.addCommand(command);
        }

        /// <summary>
        /// Get the commands that create SimElements.
        /// </summary>
        /// <returns>A list of commands that create SimElements.</returns>
        public IEnumerable<EngineCommand> getCreateSimElementCommands()
        {
            return createSimElementCommands.getCommandList();
        }

        /// <summary>
        /// Get a single command that creatse SimElements.
        /// </summary>
        /// <param name="name">The name of the command to get.</param>
        /// <returns>The EngineCommand specified by name.</returns>
        public EngineCommand getCreateSimElementCommand(String name)
        {
            return createSimElementCommands.getCommand(name);
        }

        /// <summary>
        /// Add an other command, which is a general purpose command for a
        /// plugin.
        /// </summary>
        /// <param name="command">The command to add.</param>
        public void addOtherCommand(EngineCommand command)
        {
            otherCommands.addCommand(command);
        }

        /// <summary>
        /// Get a list of all other commands registered by plugins.
        /// </summary>
        /// <returns>An enumerable over all commands.</returns>
        public IEnumerable<EngineCommand> getOtherCommands()
        {
            return otherCommands.getCommandList();
        }

        /// <summary>
        /// Get a specific other command by name.
        /// </summary>
        /// <param name="name">The name of the command to get.</param>
        /// <returns>The EngineCommand that matches name.</returns>
        public EngineCommand getOtherCommand(String name)
        {
            return otherCommands.getCommand(name);
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// The PlatformPlugin that has been loaded.
        /// </summary>
        public PlatformPlugin PlatformPlugin
        {
            get
            {
                return platformPlugin;
            }
        }

        /// <summary>
        /// The RendererPlugin that has been loaded.
        /// </summary>
        public RendererPlugin RendererPlugin
        {
            get
            {
                return rendererPlugin;
            }
        }

        #endregion Properties
    }
}
