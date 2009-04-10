using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Logging;
using System.IO;

namespace Engine
{
    /// <summary>
    /// This class loads and unloads the various plugins for the system.
    /// </summary>
    public class PluginManager : IDisposable
    {
        #region Fields

        private Dictionary<String, ElementPlugin> loadedPlugins = new Dictionary<string, ElementPlugin>();

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
            foreach (ElementPlugin plugin in loadedPlugins.Values)
            {
                plugin.Dispose();
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
                Assembly assembly = Assembly.LoadFile(Path.GetFullPath(path));
                Type[] exportedTypes = assembly.GetExportedTypes();
                Type elementPlugin = null;
                foreach (Type type in exportedTypes)
                {
                    if (type.IsSubclassOf(typeof(ElementPlugin)))
                    {
                        elementPlugin = type;
                        break;
                    }
                }
                if (elementPlugin != null)
                {
                    ElementPlugin plugin = (ElementPlugin)Activator.CreateInstance(elementPlugin);
                    loadedPlugins.Add(path, plugin);
                    plugin.initialize();
                    foreach (EngineCommand command in plugin.getCreateSimElementManagerCommands().getCommandList())
                    {
                        SimSceneDefinition.AddCreateSimElementManagerDefinitionCommand(command);
                    }
                    return true;
                }
                else
                {
                    throw new InvalidPluginException(String.Format("Could not find a subclass of ElementPlugin in plugin {0}.", path));
                }
            }
            catch (Exception e)
            {
                Log.Default.sendMessage("Error loading plugin: {0}", LogLevel.Error, "Engine", e.Message);
                e = e.InnerException;
                while (e != null)
                {
                    Log.Default.sendMessage("--Inner Exception: {0}", LogLevel.Error, "Engine", e.Message);
                    e = e.InnerException;
                }
                return false;
            }
        }

        /// <summary>
        /// Get the plugin specified by path.
        /// </summary>
        /// <param name="path">The path of the plugin to get.</param>
        /// <returns>The ElementPlugin if it is found or null if it is not.</returns>
        public ElementPlugin getPlugin(String path)
        {
            if (loadedPlugins.ContainsKey(path))
            {
                return loadedPlugins[path];
            }
            return null;
        }

        #endregion Functions
    }
}
