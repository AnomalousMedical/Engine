using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection;

namespace Engine
{
    /// <summary>
    /// This is an interface for the main plugin class. Each plugin must define
    /// a single class that inherits from this class and implements a blank
    /// constructor. That constructor will be invoked via reflection and then
    /// the plugin will be initalized.
    /// </summary>
    public interface PluginInterface : IDisposable
    {
        /// <summary>
        /// Do any initialization steps that need to be done on the plugin. Done
        /// just after the plugin is loaded and will only be called one time.
        /// You can also inject anything needed by the plugin into the container
        /// builder, which will make those things available for injection elsewhere.
        /// The scope is created after this funciton is called.
        /// </summary>
        void initialize(PluginManager pluginManager, IServiceCollection serviceCollection);

        /// <summary>
        /// Called after all plugins have been loaded in the PluginManager initializePlugins function.
        /// This function is called after initialize is called for each plugin. So any resources set
        /// up during that phase will be available here. At this point the dependency injection scope will
        /// be setup and can be used.
        /// </summary>
        void link(PluginManager pluginManager, IServiceScope globalScope);

        /// <summary>
        /// Get a name for this plugin. Care should be taken that this return
        /// value is unique. The best way would be to name it after the plugin
        /// dll.
        /// </summary>
        String Name { get; }
    }
}
