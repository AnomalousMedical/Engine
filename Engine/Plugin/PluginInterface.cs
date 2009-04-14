using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

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
        /// </summary>
        void initialize(PluginManager pluginManager);

        /// <summary>
        /// Set the classes from the platform that a plugin may be interested
        /// in. The timer can be subscribed to for updates and the EventManager
        /// will be updated with events every frame.
        /// </summary>
        /// <param name="mainTimer">The main update timer.</param>
        /// <param name="eventManager">The main event manager.</param>
        void setPlatformInfo(Timer mainTimer, EventManager eventManager);

        /// <summary>
        /// Get a name for this plugin. Care should be taken that this return
        /// value is unique. The best way would be to name it after the plugin
        /// dll.
        /// </summary>
        /// <returns>The name of the plugin.</returns>
        String getName();
    }
}
