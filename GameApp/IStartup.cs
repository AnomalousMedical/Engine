using Engine;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Anomalous.GameApp
{
    /// <summary>
    /// This interface is used to setup a pharos app.
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        /// Register any services needed.
        /// </summary>
        /// <param name="serviceCollection">The service collection to register services with.</param>
        void ConfigureServices(IServiceCollection serviceCollection);

        /// <summary>
        /// Called when initialization is complete.
        /// </summary>
        /// <param name="app"></param>
        void Initialized(GameApp app, PluginManager pluginManager);

        /// <summary>
        /// Called when GameApp is disposing. It is called before any of the game app
        /// objects are disposed.
        /// </summary>
        /// <param name="app">The app instance.</param>
        /// <param name="pluginManager">The plugin manager.</param>
        void Disposing(GameApp app, PluginManager pluginManager);

        /// <summary>
        /// The title of the app to use for its window. The user will see this.
        /// </summary>
        String Title { get; }

        /// <summary>
        /// The name of the app to use for folders. This will be used to create folders on the user's os.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Any additional assemblies you want to load beyond the basics to start an app.
        /// </summary>
        IEnumerable<Assembly> AdditionalPluginAssemblies { get; }
    }
}
