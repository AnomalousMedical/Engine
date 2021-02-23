using Engine.Platform;
using Engine.Resources;
using Engine.Threads;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

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
        private List<PluginInterface> loadedPlugins = new List<PluginInterface>();
        private IServiceCollection serviceCollection;
        private ServiceProvider serviceProvider;
        private ILogger<PluginManager> logger;

        /// <summary>
        /// Constructor. Must be called once before plugins are used.
        /// </summary>
        public PluginManager(IServiceCollection serviceCollection)
        {
            this.serviceCollection = serviceCollection;
            serviceCollection.TryAddSingleton<PluginManager>(this); //This is externally owned
            serviceCollection.TryAddSingleton<VirtualFileSystem>();
            serviceCollection.TryAddSingleton<ICoroutineRunner>(s => s.GetRequiredService<CoroutineRunner>());
            serviceCollection.TryAddSingleton<CoroutineRunner>();
            serviceCollection.TryAddSingleton<UpdateTimer>();
            serviceCollection.AddSingleton(typeof(IResourceProvider<>), typeof(VirtualFilesystemResourceProvider<>));
            serviceCollection.AddSingleton<IObjectResolverFactory>(s => new ObjectResolverFactory(serviceProvider));
            serviceCollection.AddScoped<IDestructionRequest>(s => s.GetRequiredService<DestructionRequest>());
            serviceCollection.AddScoped<DestructionRequest>();
            serviceCollection.AddScoped<ResolvedObject>();

            serviceCollection.TryAddSingleton<MainThreadSynchronizationContext>(s =>
            {
                //This must run on the main thread that executes updates
                var syncContext = new MainThreadSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(syncContext);
                return syncContext;
            });
        }

        /// <summary>
        /// Dispose function. Calling this will dispose the global scope, which will cause the
        /// entire engine to dispose itself. After calling this the program needs to end or finish
        /// any additional cleanup it may need.
        /// </summary>
        public void Dispose()
        {
            serviceProvider.Dispose();

            //Unload plugins in reverse order
            for (int i = loadedPlugins.Count - 1; i >= 0; --i)
            {
                loadedPlugins[i].Dispose();
            }
            loadedPlugins.Clear();
            ThreadManager.cancelAll();
        }

        public void InitializePlugins()
        {
            serviceProvider = serviceCollection.BuildServiceProvider();

            logger = serviceProvider.GetRequiredService<ILogger<PluginManager>>();

            foreach (var plugin in loadedPlugins)
            {
                plugin.Link(this, serviceProvider);
            }
        }

        public void AddPlugin(PluginInterface plugin)
        {
            logger?.LogInformation("Plugin {0} added.", plugin.Name);
            loadedPlugins.Add(plugin);
            plugin.Initialize(this, serviceCollection);
        }

        public IServiceProvider ServiceProvider
        {
            get
            {
                return serviceProvider;
            }
        }
    }
}
