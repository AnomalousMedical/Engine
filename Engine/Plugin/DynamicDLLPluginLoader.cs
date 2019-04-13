using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Logging;
using Engine.Shim;

namespace Engine
{
    /// <summary>
    /// This loader will load plugins from the filesystem from paths specified.
    /// </summary>
    public class DynamicDLLPluginLoader : PluginLoader
    {
        private LinkedList<String> paths = new LinkedList<string>();
        private HashSet<String> loadedAssemblies = new HashSet<string>();

        /// <summary>
        /// Add a path of a plugin to load.
        /// </summary>
        /// <param name="path">The path of the plugin to load.</param>
        public void addPath(String path)
        {
            paths.AddLast(path);
        }

        /// <summary>
        /// Remove an added path. This does nothing if the plugins are already loaded.
        /// </summary>
        /// <param name="path">The path to remove.</param>
        public void removePath(String path)
        {
            paths.Remove(path);
        }

        /// <summary>
        /// Load all defined plugins and add them to the PluginManager.
        /// </summary>
        /// <param name="pluginManager"></param>
        public void loadPlugins(PluginManager pluginManager)
        {
            foreach (String path in paths)
            {
                try
                {
                    String assemblyFileName = Path.GetFileName(path);
                    if (!loadedAssemblies.Contains(assemblyFileName))
                    {
                        String loadPath = path;
                        //If the path cannot be found search the current working directories.
                        if (!File.Exists(loadPath))
                        {
                            loadPath = pluginManager.PluginDirectory + Path.DirectorySeparatorChar + path;
                        }
                        Assembly assembly = Assembly.LoadFile(Path.GetFullPath(loadPath));
                        pluginManager.addPluginAssembly(assembly);
                        loadedAssemblies.Add(assemblyFileName);
                    }
                }
                catch (Exception e)
                {
                    Log.Default.sendMessage("Error loading plugin {1}: {0}", LogLevel.Error, "Engine", e.Message, path);
                    e = e.InnerException;
                    while (e != null)
                    {
                        Log.Default.sendMessage("--Inner Exception: {0}", LogLevel.Error, "Engine", e.Message);
                        e = e.InnerException;
                    }
                }
            }
        }
    }
}
