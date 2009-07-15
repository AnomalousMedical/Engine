using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Logging;

namespace Engine
{
    /// <summary>
    /// This loader will load plugins from the filesystem from paths specified.
    /// </summary>
    public class DynamicDLLPluginLoader : PluginLoader
    {
        private LinkedList<String> paths = new LinkedList<string>();

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
            //The application path will be detected and filled in if needed to load plugins.
            String applicationPath = null;
            foreach (String path in paths)
            {
                try
                {
                    String loadPath = path;
                    //If the path cannot be found search the current working directories.
                    if (!File.Exists(loadPath))
                    {
                        if (applicationPath == null)
                        {
                            String[] commandLine = Environment.GetCommandLineArgs();
                            if (commandLine.Length > 0)
                            {
                                applicationPath = Path.GetDirectoryName(commandLine[0]);
                            }
                        }
                        loadPath = applicationPath + Path.DirectorySeparatorChar + path;
                    }
                    Assembly assembly = Assembly.LoadFile(Path.GetFullPath(loadPath));
                    pluginManager.addPluginAssembly(assembly);
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
                }
            }
        }
    }
}
