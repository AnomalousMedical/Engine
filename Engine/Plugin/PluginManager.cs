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
    public class PluginManager
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
        /// Initialize the plugin specified by path.
        /// </summary>
        /// <param name="path">The path of the plugin to initialize.</param>
        public void initializePlugin(String path)
        {
            if (loadedPlugins.ContainsKey(path))
            {
                loadedPlugins[path].initialize();
            }
            else
            {
                Log.Default.sendMessage("Attempted to initialize plugin {0} that is not loaded.", LogLevel.Warning, "Engine", path);
            }
        }

        /// <summary>
        /// Unload the plugin specified by path.
        /// </summary>
        /// <param name="path">The path of the plugin to unload.</param>
        public void unloadPlugin(String path)
        {
            if (loadedPlugins.ContainsKey(path))
            {
                PluginInfo info = loadedPlugins[path];
                info.shutDown();
                info.Dispose();
                loadedPlugins.Remove(path);
            }
            else
            {
                Log.Default.sendMessage("Attempted to shut down plugin {0} that is not loaded.", LogLevel.Warning, "Engine", path);
            }
        }

        #endregion Functions
    }
}
