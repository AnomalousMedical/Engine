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
        void link(PluginManager pluginManager);

        /// <summary>
        /// Set the classes from the platform that a plugin may be interested
        /// in. The timer can be subscribed to for updates and the EventManager
        /// will be updated with events every frame.
        /// </summary>
        /// <param name="mainTimer">The main update timer.</param>
        /// <param name="eventManager">The main event manager.</param>
        void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager);

        /// <summary>
        /// Get a name for this plugin. Care should be taken that this return
        /// value is unique. The best way would be to name it after the plugin
        /// dll.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// This funciton will be called if a type name is missed on loading if the findType function is used on the PluginManager. 
        /// Plugins can use it to add any types that have been renamed to the RenamedTypeMap.
        /// </summary>
        /// <param name="renamedTypeMap">The map to add renamed types to.</param>
        void setupRenamedSaveableTypes(RenamedTypeMap renamedTypeMap);
    }
}
