using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This is an interface for classes that load plugins. This way the
    /// specifics about how plugins are loaded can be changed.
    /// </summary>
    public interface PluginLoader
    {
        /// <summary>
        /// Load the plugins and add them to the PluginManager.
        /// </summary>
        /// <param name="pluginManager">The PluginManager to add the plugins to.</param>
        void loadPlugins(PluginManager pluginManager);
    }
}
