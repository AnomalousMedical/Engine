using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Engine
{
    /// <summary>
    /// This is an info class for a single plugin.
    /// </summary>
    class PluginInfo : IDisposable
    {
        #region Fields

        private Assembly assembly;
        private ComponentPlugin plugin;
        private String path;
        private bool initialized = false;

        #endregion Fields

        #region Constructors  

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="path">The path of the assembly to load</param>
        /// <exception cref="Engine.InvalidPluginException">If there was an error loading the plugin this will detail why.</exception>
        public PluginInfo(String path)
        {
            try
            {
                this.path = path;
                assembly = Assembly.LoadFile(path);
                Type[] exportedTypes = assembly.GetExportedTypes();
                Type componentPlugin = null;
                foreach (Type type in exportedTypes)
                {
                    if(type.GetType().IsSubclassOf(typeof(ComponentPlugin)))
                    {
                        componentPlugin = type;
                        break;
                    }
                }
                if (componentPlugin != null)
                {
                    plugin = (ComponentPlugin)Activator.CreateInstance(componentPlugin);
                }
                else
                {
                    throw new InvalidPluginException(String.Format("Could not find a subclass of ComponentPlugin in plugin {0}.", path));
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginException(String.Format("Error loading the assembly for the plugin {0}.", e.Message));
            }
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Dispose function.
        /// </summary>
        public void Dispose()
        {
            plugin.Dispose();
        }

        /// <summary>
        /// Initialize the plugin.
        /// </summary>
        public void initialize()
        {
            plugin.initialize();
            initialized = true;
        }

        /// <summary>
        /// Shut down the plugin if it was initialized. Otherwise does nothing.
        /// </summary>
        public void shutDown()
        {
            if (initialized)
            {
                plugin.shutDown();
            }
        }

        #endregion Functions
    }
}
