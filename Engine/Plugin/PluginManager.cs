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
using Engine.Command;
using Engine.Resources;

namespace Engine
{
    /// <summary>
    /// Callback for when the renderer plugin is found and needs to configure its window.
    /// </summary>
    /// <param name="defaultWindow">Assign this to a new DefaultWindowInfo instance.</param>
    public delegate void ConfigureDefaultWindow(out WindowInfo defaultWindow);

    /// <summary>
    /// This class loads and unloads the various plugins for the system.
    /// </summary>
    public class PluginManager : IDisposable
    {
        #region Static

        private static PluginManager instance;
        private static readonly char[] SPLIT = { ',' };

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

        private List<PluginInterface> loadedPlugins = new List<PluginInterface>();
        private List<AddSimElementCommand> addSimElementCommands = new List<AddSimElementCommand>();
        private List<AddSimElementManagerCommand> addElementManagerCommands = new List<AddSimElementManagerCommand>();
        private PlatformPlugin platformPlugin = null;
        private RendererPlugin rendererPlugin = null;
        private List<Assembly> pluginAssemblies = new List<Assembly>();
        private ResourceManager primaryResourceManager = new ResourceManager();
        private ResourceManager emptyResourceManager = new ResourceManager();
        private List<DebugInterface> debugInterfaces;
        private ConfigFile configFile;
        private String pluginDirectory = null;
        private VirtualFileSystem virtualFileSystem;
        private Dictionary<String, Type> slowSearchTypeCache = new Dictionary<string, Type>();//After a slow search is done for a type it will be stored here for faster lookup if it is needed again.

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
        public PluginManager(ConfigFile configFile)
        {
            if (instance == null)
            {
                this.configFile = configFile;
                instance = this;
                instance.addPlugin(new BehaviorInterface());
                virtualFileSystem = new VirtualFileSystem();
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
            virtualFileSystem.Dispose();
            //Unload plugins in reverse order
            for (int i = loadedPlugins.Count - 1; i >= 0; --i)
            {
                loadedPlugins[i].Dispose();
            }
            loadedPlugins.Clear();
        }

        /// <summary>
        /// Load all plugins for the assemblies that have been registered. This
        /// should be called before any plugins are used at the start of the
        /// program. The plugin assemblies need to be registered using
        /// addPluginAssembly or by using a PluginLoader.
        /// </summary>
        public void initializePlugins()
        {
            foreach (Assembly assembly in pluginAssemblies)
            {
                PluginEntryPointAttribute[] attributes = (PluginEntryPointAttribute[])assembly.GetCustomAttributes(typeof(PluginEntryPointAttribute));
                if (attributes.Length > 0)
                {
                    foreach (PluginEntryPointAttribute entryPointAttribute in attributes)
                    {
                        entryPointAttribute.createPluginInterfaces(this);
                    }
                }
                else
                {
                    throw new InvalidPluginException(String.Format("Cannot find PluginEntryPointAttribute in assembly {0}. Please add this property to the assembly.", assembly.FullName));
                }
            }
            if (platformPlugin == null)
            {
                throw new InvalidPluginException("No platform plugin defined. Please define a platform plugin.");
            }
            if(rendererPlugin == null)
            {
                throw new InvalidPluginException("No renderer plugin defined. Please define a renderer plugin.");
            }
        }

        /// <summary>
        /// Add a plugin instance to this manager. This should be called inside
        /// PluginEntryPoint subclasses. Doing it this way allows a dll to load
        /// multiple plugins.
        /// </summary>
        /// <param name="plugin">The plugin to add.</param>
        public void addPlugin(PluginInterface plugin)
        {
            Log.Default.sendMessage("Plugin {0} added.", LogLevel.Info, "Engine", plugin.getName());
            loadedPlugins.Add(plugin);
            plugin.initialize(this);
        }

        /// <summary>
        /// Search for a given type using an assemblyQualifiedName. First this
        /// will use the standard Type.GetType function. If that fails it will
        /// search all the plugin loaded assemblies for the type directly. If
        /// that fails the function will return null. If sucessful the matching
        /// type will be returned.
        /// </summary>
        /// <remarks>
        /// This function could be optimized. For now it will search all loaded
        /// assemblies in a loop when the Type.GetType function call fails.
        /// </remarks>
        /// <param name="assemblyQualifiedName">The AssemblyQualifiedName to search for.</param>
        /// <returns>The matching type or null if the type cannot be found.</returns>
        internal Type findType(String assemblyQualifiedName)
        {
            Type type = Type.GetType(assemblyQualifiedName, false);
            //If that fails do the slow search.
            if (type == null)
            {
                slowSearchTypeCache.TryGetValue(assemblyQualifiedName, out type);
                if (type == null)
                {
                    Log.Warning("Had to do slow search looking for type \'{0}\'. You should fix the source file searching for this type", assemblyQualifiedName);
                    String typeName = assemblyQualifiedName.Split(SPLIT)[0];
                    foreach (Assembly assembly in pluginAssemblies)
                    {
                        type = assembly.GetType(typeName);
                        if (type != null)
                        {
                            break;
                        }
                    }
                    //If that fails search all loaded assemblies.
                    if (type == null)
                    {
                        foreach (Assembly assembly in AppDomainShim.GetCurrentDomainAssemblies())
                        {
                            type = assembly.GetType(typeName);
                            if (type != null)
                            {
                                break;
                            }
                        }
                    }
                    if (type != null)
                    {
                        Log.Warning("Slow search match found for type \'{0}\'. Replacement type is \'{1}\'", assemblyQualifiedName, type.AssemblyQualifiedName);
                        slowSearchTypeCache.Add(assemblyQualifiedName, type);
                    }
                }
            }
            return type;
        }

        /// <summary>
        /// Add an assembly that will be searched for a plugin.
        /// </summary>
        /// <param name="assembly">The assembly to search.</param>
        public void addPluginAssembly(Assembly assembly)
        {
            pluginAssemblies.Add(assembly);
        }

        /// <summary>
        /// Get the plugin specified by path.
        /// </summary>
        /// <param name="name">The name of the plugin to get.</param>
        /// <returns>The ElementPlugin if it is found or null if it is not.</returns>
        public PluginInterface getPlugin(String name)
        {
            foreach(PluginInterface plugin in loadedPlugins)
            {
                if(plugin.getName() == name)
                {
                    return plugin;
                }
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
        public void setRendererPlugin(RendererPlugin plugin, out WindowInfo defaultWindow)
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
                    defaultWindow = new WindowInfo("Anomalous Engine", 640, 480);
                }
            }
            else
            {
                throw new InvalidPluginException("A second renderer plugin was added. It is only valid to specify one renderer plugin please correct this issue.");
            }
        }

        /// <summary>
        /// Call this to recall the default window function and setup the window again.
        /// </summary>
        /// <param name="defaultWindow">The parameters for the default window.</param>
        public void reconfigureDefaultWindow(out WindowInfo defaultWindow)
        {
            if (OnConfigureDefaultWindow != null)
            {
                OnConfigureDefaultWindow.Invoke(out defaultWindow);
            }
            else
            {
                defaultWindow = new WindowInfo("Anomalous Engine", 640, 480);
            }
        }

        /// <summary>
        /// Add a set of SubsystemResources to the primary ResourceManager. This
        /// will allow a plugin to get updates about the resources that are
        /// requested for a scene. The SubsystemResources passed to this
        /// function should be configured with a ResourceListener so it can get
        /// the appropriate callbacks.
        /// </summary>
        /// <param name="subsystem">A callback configured SubsystemResources instance.</param>
        public void addSubsystemResources(SubsystemResources subsystem)
        {
            primaryResourceManager.addSubsystemResource(subsystem);
            emptyResourceManager.addSubsystemResource(new SubsystemResources(subsystem));
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
            foreach (PluginInterface plugin in loadedPlugins)
            {
                plugin.setPlatformInfo(mainTimer, eventManager);
            }
        }

        /// <summary>
        /// Add a command to create a SimElementManagerDescription.
        /// </summary>
        /// <param name="command">A command that creates SimElementManagerDescriptions.</param>
        public void addCreateSimElementManagerCommand(AddSimElementManagerCommand command)
        {
            addElementManagerCommands.Add(command);
        }

        /// <summary>
        /// Get the list of commands from plugins that create SimElementManagers.
        /// </summary>
        /// <returns>The list of commands from plugins that create SimElementManagers.</returns>
        public IEnumerable<AddSimElementManagerCommand> getCreateSimElementManagerCommands()
        {
            return addElementManagerCommands;
        }

        /// <summary>
        /// Add a command to create a SimElementDescription.
        /// </summary>
        /// <param name="command">A command that creates SimElementDescriptions.</param>
        public void addCreateSimElementCommand(AddSimElementCommand command)
        {
            addSimElementCommands.Add(command);
        }

        /// <summary>
        /// Get the commands that create SimElements.
        /// </summary>
        /// <returns>A list of commands that create SimElements.</returns>
        public IEnumerable<AddSimElementCommand> getCreateSimElementCommands()
        {
            return addSimElementCommands;
        }

        /// <summary>
        /// Create the debugging commands for the various plugins.
        /// </summary>
        /// <returns>A list of command managers for each plugin.</returns>
        public List<CommandManager> createDebugCommands()
        {
            List<CommandManager> commands = new List<CommandManager>();
            foreach (PluginInterface plugin in loadedPlugins)
            {
                plugin.createDebugCommands(commands);
            }
            return commands;
        }

        /// <summary>
        /// Create a copy of the PrimaryResourceManager. This will contain all
        /// of the same resource definitions as the PrimaryResourceManager,
        /// however, it will not update the subsystems as it is changed. This
        /// can then be applied to the primary manager to make it update the
        /// subsystems to match the new definitions.
        /// </summary>
        /// <returns>A new ResourceManager that is a copy of the PrimaryResourceManager.</returns>
        public ResourceManager createSecondaryResourceManager()
        {
            return new ResourceManager(primaryResourceManager);
        }

        /// <summary>
        /// Create a ResourceManager that contains the SubsystemResources for
        /// each plugin, but not any groups or locations.
        /// </summary>
        /// <returns>A new empty ResourceManager.</returns>
        public ResourceManager createEmptyResourceManager()
        {
            return new ResourceManager(emptyResourceManager);
        }

        public IEnumerable<DebugInterface> DebugInterfaces
        {
            get
            {
                if (debugInterfaces == null)
                {
                    debugInterfaces = new List<DebugInterface>();
                    foreach (PluginInterface plugin in loadedPlugins)
                    {
                        DebugInterface debug = plugin.getDebugInterface();
                        if (debug != null)
                        {
                            debugInterfaces.Add(debug);
                        }
                    }
                }
                return debugInterfaces;
            }
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

        /// <summary>
        /// This is the ResourceManager that is hooked up to the plugins. By
        /// modifying this resource manager the plugins will get updates about
        /// the resource changes. If you need to change resources without
        /// modifying the actual loaded resources call
        /// createSecondaryResourceManager().
        /// </summary>
        public ResourceManager PrimaryResourceManager
        {
            get
            {
                return primaryResourceManager;
            }
        }

        /// <summary>
        /// The ConfigFile used by the system.
        /// </summary>
        public ConfigFile ConfigFile
        {
            get
            {
                return configFile;
            }
        }

        public String PluginDirectory
        {
            get
            {
                if (pluginDirectory == null)
                {
                    String[] args = EnvironmentShim.GetCommandLineArgs();
                    if (args.Length > 0)
                    {
                        pluginDirectory = Path.GetDirectoryName(args[0]);
                    }
                }
                return pluginDirectory;
            }
        }

        #endregion Properties
    }
}
