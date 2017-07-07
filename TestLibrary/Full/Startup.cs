using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Engine;
using System.Reflection;

namespace Anomalous.Minimus.Full
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
        /// <param name="pharosApp"></param>
        void Initialized(PharosApp pharosApp, PluginManager pluginManager);

        /// <summary>
        /// The title of the app to use for its window. The user will see this.
        /// </summary>
        String Title { get; }

        /// <summary>
        /// Any additional assemblies you want to load beyond the basics to start an app.
        /// </summary>
        IEnumerable<Assembly> AdditionalPluginAssemblies { get; }
    }
}
