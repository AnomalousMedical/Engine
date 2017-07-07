using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Engine;

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
        void RegisterServices(ContainerBuilder builder);

        /// <summary>
        /// Called when initialization is complete.
        /// </summary>
        /// <param name="pharosApp"></param>
        void OnInit(PharosApp pharosApp, PluginManager pluginManager);

        /// <summary>
        /// The title of the app to use for its window. The user will see this.
        /// </summary>
        String Title { get; }
    }
}
