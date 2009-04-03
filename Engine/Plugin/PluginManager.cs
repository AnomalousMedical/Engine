using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Logging;

namespace Engine
{
    /// <summary>
    /// This class loads and unloads the various plugins for the system.
    /// </summary>
    public class PluginManager : IDisposable
    {
        #region Fields

        private Dictionary<String, PluginInfo> loadedPlugins = new Dictionary<string, PluginInfo>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public PluginManager()
        {

        }

        #endregion

        #region Functions

        /// <summary>
        /// Dispose function.
        /// </summary>
        public void Dispose()
        {
            foreach (PluginInfo info in loadedPlugins.Values)
            {
                info.shutDown();
                info.Dispose();
            }
            loadedPlugins.Clear();
        }

        /// <summary>
        /// Load the plugin specified by path.
        /// </summary>
        /// <param name="path">The path of the plugin to load.</param>
        /// <returns>True if the plugin was loaded sucessfully.</returns>
        public bool loadPlugin(String path)
        {
            try
            {
                PluginInfo plugin = new PluginInfo(path);
                loadedPlugins.Add(path, plugin);
                return true;
            }
            catch (Exception e)
            {
                Log.Default.sendMessage("{0}", LogLevel.Error, "Engine", e.Message);
                return false;
            }
        }

        /// <summary>
        /// Get the plugin specified by path.
        /// </summary>
        /// <param name="path">The path of the plugin to get.</param>
        /// <returns>The ComponentPlugin if it is found or null if it is not.</returns>
        public ComponentPlugin getPlugin(String path)
        {
            if (loadedPlugins.ContainsKey(path))
            {
                return loadedPlugins[path].Plugin;
            }
            return null;
        }

        #endregion Functions
    }
}
