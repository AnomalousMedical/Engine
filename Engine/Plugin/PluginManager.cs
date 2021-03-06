﻿using System;
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
using Engine.Saving;
using Engine.Threads;
using Engine.Shim;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Engine
{
    /// <summary>
    /// Callback for when the renderer plugin is found and needs to configure its window.
    /// </summary>
    /// <param name="defaultWindow">Assign this to a new DefaultWindowInfo instance.</param>
    public delegate void ConfigureDefaultWindow(out WindowInfo defaultWindow);

    /// <summary>
    /// This class loads and unloads the various plugins for the system.
    /// It will also maintain the di scope using a builder you supply or one it creates.
    /// Any instance of this class will be added to the builder automatically, in addition
    /// they will maintain the main app container and globlal lifetime scope. To shut down
    /// these containers call the Dispose function on the PluginManager. This will initiate
    /// cleanup correctly. If you create a PluginManager you are responsible to call Dispose on it.
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

        private List<PluginInterface> loadedPlugins = new List<PluginInterface>();
        private List<AddSimElementCommand> addSimElementCommands = new List<AddSimElementCommand>();
        private List<AddSimElementManagerCommand> addElementManagerCommands = new List<AddSimElementManagerCommand>();
        private RendererPlugin rendererPlugin = null;
        private List<Assembly> pluginAssemblies = new List<Assembly>();
        private ResourceManager prototypeResourceManager = new ResourceManager();
        private List<DebugInterface> debugInterfaces;
        private ConfigFile configFile;
        private String pluginDirectory = null;
        private VirtualFileSystem virtualFileSystem;
        private Dictionary<String, Type> typeCache = new Dictionary<string, Type>();//After a type is found for an AssemblyQualifiedName it will be stored here.
        private RenamedTypeMap renamedTypeMap = null;
        private IServiceCollection serviceCollection;
        private ServiceProvider serviceProvider;
        private IServiceScope globalScope;
        private bool shuttingDown = false; //This makes sure Shutdown is always called

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
        public PluginManager(ConfigFile configFile, IServiceCollection serviceCollection)
        {
            this.serviceCollection = serviceCollection;
            serviceCollection.TryAddSingleton<PluginManager>(this); //This is externally owned

            if (instance == null)
            {
                this.configFile = configFile;
                instance = this;
                instance.addPlugin(new BehaviorPluginInterface());
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
        /// Dispose function. Calling this will dispose the global scope, which will cause the
        /// entire engine to dispose itself. After calling this the program needs to end or finish
        /// any additional cleanup it may need.
        /// </summary>
        /// <remarks>
        /// Internally this will end up being called twice. Once when the user disposes the PluginManager
        /// and once when the di system disposes the actual instance.
        /// </remarks>
        public void Dispose()
        {
            if (shuttingDown)
            {
                //TODO: Refactor this to be less dumb logic wise.
                //For ms di it seems like we can do this all at once, still investigating lifecycles.
            }
            else
            {
                //If we are not in dispose from a shutdown set this to true and dispose
                //the di containers, that will eventually call the real dispose at the correct
                //time for the shutdown order. This keeps the di system in charge of dispose
                //order and keeps us from having to do other weird stuff.
                shuttingDown = true;
                globalScope.Dispose();
                serviceProvider.Dispose();

                //Then virtual file system
                virtualFileSystem.Dispose();
                //Unload plugins in reverse order
                for (int i = loadedPlugins.Count - 1; i >= 0; --i)
                {
                    loadedPlugins[i].Dispose();
                }
                loadedPlugins.Clear();
                ThreadManager.cancelAll();
            }
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

            serviceProvider = serviceCollection.BuildServiceProvider();
            globalScope = serviceProvider.CreateScope();

            foreach (var plugin in loadedPlugins)
            {
                plugin.link(this);
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
            Log.Default.sendMessage("Plugin {0} added.", LogLevel.Info, "Engine", plugin.Name);
            loadedPlugins.Add(plugin);
            plugin.initialize(this, serviceCollection);
            if (renamedTypeMap != null)
            {
                plugin.setupRenamedSaveableTypes(renamedTypeMap);
            }
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
            Type type;
            if (!typeCache.TryGetValue(assemblyQualifiedName, out type))
            {
                type = Type.GetType(assemblyQualifiedName, false);
                //If that fails do the slow search.
                if (type == null)
                {
                    String typeName = DefaultTypeFinder.GetTypeNameWithoutAssembly(assemblyQualifiedName);

                    //If there is not yet a renamed type map, create it.
                    if (renamedTypeMap == null)
                    {
                        renamedTypeMap = new RenamedTypeMap();
                        foreach (var plugin in loadedPlugins)
                        {
                            plugin.setupRenamedSaveableTypes(renamedTypeMap);
                        }
                    }

                    //Check the rename cache
                    if (!renamedTypeMap.tryGetType(typeName, out type))
                    {
                        Log.Warning("TypeSearch: Had to do slow search looking for type \'{0}\'. You should fix the source file searching for this type or add an entry to the renamed type maps for '{1}'", assemblyQualifiedName, typeName);

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
                            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                            {
                                type = assembly.GetType(typeName);
                                if (type != null)
                                {
                                    break;
                                }
                            }
                        }
                    }

                    //If we found something put it in the slow search cache so it can be found quicker later
                    if (type != null)
                    {
                        Log.Warning("TypeSearch: Replacement found for type \'{0}\'. Replacement type is \'{1}\'", assemblyQualifiedName, type.AssemblyQualifiedName);
                    }
                    else
                    {
                        Log.Warning("TypeSearch: Unable to find replacement for type \'{0}\'.", assemblyQualifiedName);
                    }
                }
                typeCache.Add(assemblyQualifiedName, type);
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
            foreach (PluginInterface plugin in loadedPlugins)
            {
                if (plugin.Name == name)
                {
                    return plugin;
                }
            }
            return null;
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
                Log.Default.sendMessage("Renderer plugin set to {0}.", LogLevel.Info, "Engine", plugin.Name);
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
        public void addSubsystemResources(String name, ResourceListener subsystemListener)
        {
            prototypeResourceManager.addSubsystemResource(new SubsystemResources(name, subsystemListener));
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
            GlobalContextEventHandler.setEventManager(eventManager);
            foreach (PluginInterface plugin in loadedPlugins)
            {
                plugin.setPlatformInfo(mainTimer, eventManager);
            }
            mainTimer.addUpdateListener(new ThreadManagerUpdate());
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
        /// Create a live ResourceManager that contains the SubsystemResources for each plugin, but not any groups or locations. 
        /// Live means that changes to this resource manager will be applied to subsystems. If you add resources to this resource manager
        /// the subsystems will attempt to load them. It is not very likely that you will create a lot of live resource managers, because even
        /// though you may create multiple of them they likely map back to a singleton in the subsystems they are hooked up to. Also be careful
        /// of name collisions between the ResourceManagers you create. The most likely usage is to create one of these for scene resources and
        /// one for persistant resources, being careful not to have collisions between them.
        /// You provide a namespace for this ResourceManager that helps to avoid these collisions. You should take care that each live ResourceManager
        /// you create has a unique name.
        /// This is the official way to create ResourceManagers because it is unknown until the engine is configured what subsystems need resources.
        /// </summary>
        /// <returns>A new empty ResourceManager that is hooked up to the subsystems.</returns>
        public ResourceManager createLiveResourceManager(String resourceNamespace)
        {
            return new ResourceManager(prototypeResourceManager, true)
            {
                Namespace = resourceNamespace
            };
        }

        /// <summary>
        /// Create a scratch ResourceManager that contains the SubsystemResources for each plugin, but not any groups or locations. 
        /// Scratch means that resources added to this resource manager will not alert the subsystems of changes. You could then apply this resource manager
        /// to a live resource manager or save it or do something else. These will not alter loaded resources.
        /// This is the official way to create ResourceManagers because it is unknown until the engine is configured what subsystems need resources.
        /// </summary>
        /// <returns>A new empty ResourceManager that is not hooked up to the subsystems.</returns>
        public ResourceManager createScratchResourceManager()
        {
            return new ResourceManager(prototypeResourceManager, false);
        }

        /// <summary>
        /// Create a resource manager with one subsystem specified by customSubsystemName using the passed listener. This
        /// does not have any relation to the live and scratch resource managers.
        /// </summary>
        /// <param name="customSubsystemName"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public ResourceManager createResourceManagerForListener(String customSubsystemName, ResourceListener listener)
        {
            ResourceManager resourceManager = new ResourceManager();
            resourceManager.addSubsystemResource(new SubsystemResources(customSubsystemName, listener));
            return resourceManager;
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
                    String[] args = Environment.GetCommandLineArgs();
                    if (args.Length > 0)
                    {
                        pluginDirectory = Path.GetDirectoryName(args[0]);
                    }
                }
                return pluginDirectory;
            }
        }

        public IServiceScope GlobalScope
        {
            get
            {
                return globalScope;
            }
        }

        #endregion Properties
    }
}
