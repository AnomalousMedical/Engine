using Engine.Platform;
using Engine.Threads;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Engine
{
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
        private List<Assembly> pluginAssemblies = new List<Assembly>();
        private String pluginDirectory = null;
        private IServiceCollection serviceCollection;
        private ServiceProvider serviceProvider;
        private IServiceScope globalScope;
        private bool shuttingDown = false; //This makes sure Shutdown is always called
        private ILogger<PluginManager> logger;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor. Must be called once before plugins are used.
        /// </summary>
        public PluginManager(IServiceCollection serviceCollection)
        {
            this.serviceCollection = serviceCollection;
            serviceCollection.TryAddSingleton<PluginManager>(this); //This is externally owned
            serviceCollection.TryAddSingleton<VirtualFileSystem>();

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

            logger = globalScope.ServiceProvider.GetRequiredService<ILogger<PluginManager>>();

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
            logger?.LogInformation("Plugin {0} added.", plugin.Name);
            loadedPlugins.Add(plugin);
            plugin.initialize(this, serviceCollection);
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

        #endregion Functions

        #region Properties

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
