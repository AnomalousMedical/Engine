using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This is an interface for the main plugin class for an ElementPlugin. An
    /// ElementPlugin can be any subsystem that will be created as part of a
    /// SimObject such as a plugin for graphics or physics. Each plugin must
    /// define a single class that inherits from this class and implements a
    /// blank constructor. That constructor will be invoked via reflection and
    /// then the plugin will be initalized and all of the commands it can
    /// execute will be recovered.
    /// </summary>
    public interface PluginInterface : IDisposable
    {
        /// <summary>
        /// Do any initialization steps that need to be done on the plugin. Done
        /// just after the plugin is loaded and will only be called one time.
        /// </summary>
        void initialize(PluginManager pluginManager);

        /// <summary>
        /// Get a name for this plugin. Care should be taken that this return
        /// value is unique. The best way would be to name it after the plugin
        /// dll.
        /// </summary>
        /// <returns>The name of the plugin.</returns>
        String getName();
    }
}
