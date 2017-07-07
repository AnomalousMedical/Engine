using Autofac;
using Engine;
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
        /// <param name="builder">The builder to register.</param>
        void ConfigureServices(ContainerBuilder builder);

        /// <summary>
        /// Called when initialization is complete.
        /// </summary>
        /// <param name="app"></param>
        void Initialized(GameApp app, PluginManager pluginManager);

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
